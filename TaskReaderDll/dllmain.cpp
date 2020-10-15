// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "detours.h"
#include <atlstr.h>;
#include <iostream>;


#define TEST_PIPE_NAME L"\\\\.\\pipe\\MediviaTaskReader"

typedef void(__fastcall* ProcessTextMessage)(DWORD_PTR textPtr);
DWORD_PTR BaseAddress;
HMODULE HandleModule;
HANDLE SendDataPipe;
DWORD SendDataPipeThreadID;
BOOL isRunning;
ProcessTextMessage ProcessTextMessagePointer;

BOOL sendMessage(const char* message) {
	bool writeSuccess = false;
	DWORD bytesWritten;

	do
	{
		writeSuccess = WriteFile(SendDataPipe, message, strlen(message), &bytesWritten, NULL);
	} while (!writeSuccess && isRunning);

	if (writeSuccess)
		return true;
	else
		return false;
}

DWORD_PTR getBufferAddress() {
	DWORD_PTR bufferBaseAddress;
	bufferBaseAddress = BaseAddress + 0x0D45D60;
	bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)bufferBaseAddress;
	bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0xF0);
	bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x10);
	bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x2B8);
	bufferBaseAddress += 0x668;
	return bufferBaseAddress;
}

void __fastcall hookProcessTextMessage(DWORD_PTR textPtr) {
	DWORD_PTR bufferAddress = getBufferAddress();
	long messageType = *(long*)(bufferAddress);

	if (messageType == 16) {
		DWORD_PTR messageAddress = *(DWORD_PTR*)(bufferAddress - 0x8);
		sendMessage((char*)messageAddress);
	}

	return ProcessTextMessagePointer(textPtr);
}


BOOL writeCharPointer(HANDLE& pipe, const char* value)
{
	bool readSuccess = false;
	bool writeSuccess = false;
	DWORD bytesWritten;

	do
	{
		writeSuccess = WriteFile(pipe, value, strlen(value), &bytesWritten, NULL);
	} while (!writeSuccess && isRunning);

	if (writeSuccess)
		return true;
	else
		return false;
}

DWORD WINAPI DataSendThread(LPVOID) {
	do
	{
		SendDataPipe = CreateFile(TEST_PIPE_NAME,
			GENERIC_READ | GENERIC_WRITE | PIPE_WAIT,
			0,
			NULL,
			CREATE_NEW,
			0,
			NULL);
	} while (SendDataPipe == INVALID_HANDLE_VALUE);

	return 0;
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
		HandleModule = hModule;
		BaseAddress = (DWORD_PTR)GetModuleHandle(NULL);
		ProcessTextMessagePointer = (ProcessTextMessage)(BaseAddress + 0x22B40);
		DetourTransactionBegin();
		DetourUpdateThread(GetCurrentThread());
		DetourAttach(&(PVOID&)(ProcessTextMessagePointer), (PVOID)hookProcessTextMessage);
		DetourTransactionCommit();
		isRunning = true;

		CreateThread(NULL, NULL, &DataSendThread, NULL, NULL, &SendDataPipeThreadID);
		break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

