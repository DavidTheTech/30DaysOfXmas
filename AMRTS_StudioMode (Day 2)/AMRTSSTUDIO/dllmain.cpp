#include <Windows.h>
#include <stdio.h>
#include "Header.h"

HANDLE InjectionHandle = (HANDLE) NULL;

void Inject()
{
	AllocConsole();
	Sleep(200);
	printf("Hello, Welcome to Army Men RTS Studio\n");
	printf("Unlocked after 18 years\n");
	printf("This was unlocked by @DavidTheTech\n");

	RegRunCode((DWORD *)Something, "Studio", (int)StudioPrss, (int)StudioInit, (int)StudioDone, (int)StudioPostInit, 0);
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		InjectionHandle = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)Inject, (LPVOID) NULL, NULL, NULL);
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

