// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "detours.h"

#define TEST_PIPE_NAME L"\\\\.\\pipe\\MediviaTaskReader"

DWORD_PTR BaseAddress;

void start() {
	BaseAddress = (DWORD_PTR)GetModuleHandle(NULL);
}

HMODULE HandleModule;
HANDLE TestPipe;
DWORD TestPipeThreadID;

DWORD WINAPI TestPipeThread(LPVOID) {
	do
	{
		TestPipe = CreateFile(TEST_PIPE_NAME,
			GENERIC_READ,
			0,
			NULL,
			CREATE_NEW,
			0,
			NULL);
	} while (TestPipe == INVALID_HANDLE_VALUE);

	FreeLibraryAndExitThread(HandleModule, 0);
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
		DisableThreadLibraryCalls(hModule);
		MessageBoxA(NULL, "Meme", "Meme", MB_OK);
		CreateThread(NULL, NULL, &TestPipeThread, NULL, NULL, &TestPipeThreadID);
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

