// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "detours.h"
#include <atlstr.h>;
#include <iostream>;


#define SEND_DATA_PIPE_NAME L"\\\\.\\pipe\\MediviaTaskReaderSend"
#define RECEIVE_DATA_PIPE_NAME L"\\\\.\\pipe\\MediviaTaskReaderReceive"

typedef void(__fastcall* ProcessTextMessage)(DWORD_PTR textPtr);
DWORD_PTR BaseAddress;
HMODULE HandleModule;
HANDLE SendDataPipe;
HANDLE ReceiveDataPipe;
DWORD ReceiveDataPipeThreadID;
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
	DWORD_PTR bufferBaseAddress = 0;
	try {
		//bufferBaseAddress = BaseAddress + 0x0D45368;
		//bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)bufferBaseAddress;
		//bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x1E0);
		//bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x110);
		//bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x660);
		//bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x20);
		//bufferBaseAddress += 0x338;
		bufferBaseAddress = BaseAddress + 0x0D54F38;
		bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)bufferBaseAddress;
		bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x58);
		bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x38);
		bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x18);
		bufferBaseAddress = (DWORD_PTR) * (DWORD_PTR*)(bufferBaseAddress + 0x20);
		bufferBaseAddress = bufferBaseAddress > 0 ? bufferBaseAddress + 0x618 : 0;
	}
	catch(int e){

	}
	return bufferBaseAddress;
}

void __fastcall hookProcessTextMessage(DWORD_PTR textPtr) {
	DWORD_PTR bufferAddress = getBufferAddress();
	try {
		long long messageType = *(long long*)(bufferAddress);
		CString intStr;
		if (messageType == 16) {
			DWORD_PTR messageAddress = *(DWORD_PTR*)(bufferAddress - 0x8);
			sendMessage((char*)messageAddress);
		}
	}
	catch (int e) {

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

DWORD WINAPI ReceiveDataThread(LPVOID) {
	do
	{
		ReceiveDataPipe = CreateFile(RECEIVE_DATA_PIPE_NAME,
			GENERIC_READ | GENERIC_WRITE | PIPE_WAIT,
			0,
			NULL,
			CREATE_NEW,
			0,
			NULL);
	} while (ReceiveDataPipe == INVALID_HANDLE_VALUE);

	bool readSuccess = false;

	unsigned char buffer[512];

	DWORD byteReads;

	while (true) {
		readSuccess = false;

		do
		{
			readSuccess = ReadFile(ReceiveDataPipe, buffer, 512 * sizeof(wchar_t), &byteReads, nullptr);
		} while (!readSuccess);

		if (((byte)* buffer) == 0xFF) {
			break;
		}
	}

	MessageBoxA(NULL, "MEME", "MEME", MB_OK);



	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	// this will hook the function
	DetourDetach(&(PVOID&)(ProcessTextMessagePointer), &hookProcessTextMessage);
	DetourTransactionCommit();
	FreeLibraryAndExitThread(HandleModule, 0);
	return 0;
}

void start() {
	BaseAddress = (DWORD_PTR)GetModuleHandle(NULL);
	ProcessTextMessagePointer = (ProcessTextMessage)(BaseAddress + 0x22B40);
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach(&(PVOID&)(ProcessTextMessagePointer), (PVOID)hookProcessTextMessage);
	DetourTransactionCommit();
	isRunning = true;

	do
	{
		SendDataPipe = CreateFile(SEND_DATA_PIPE_NAME,
			GENERIC_WRITE | GENERIC_READ | PIPE_WAIT,
			0,
			NULL,
			CREATE_NEW,
			0,
			NULL);
	} while (SendDataPipe == INVALID_HANDLE_VALUE);

	CreateThread(NULL, NULL, &ReceiveDataThread, NULL, NULL, &ReceiveDataPipeThreadID);
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
		DisableThreadLibraryCalls(hModule);
		start();
		break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

