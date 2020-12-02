#include <Windows.h>
#include <TlHelp32.h>
#include <stdio.h>
#include <Psapi.h>

#define ScanAddress(Address) (Address - 0x400000 + (DWORD)GetModuleHandle("AMRTS.exe"))

DWORD Something = 0x007288E0;

char aStudio[] = "Studio";

typedef void(__thiscall *RegisterRuncode) (DWORD *tshis, char *a2, int a3, int a4, int a5, int a6, int a7);
RegisterRuncode RegRunCode = (RegisterRuncode)(ScanAddress(0x4B6520));

typedef int(*StudioProcess) (void);
StudioProcess StudioPrss = (StudioProcess)(ScanAddress(0x5D02B0));

typedef int(__cdecl *StudioInt) (void);
StudioInt StudioInit = (StudioInt)(ScanAddress(0x5D01E0));

typedef void(__cdecl *StudioDN) (void);
StudioDN StudioDone = (StudioDN)(ScanAddress(0x5D0270));

typedef void(__cdecl *StudioPostInt) (void);
StudioPostInt StudioPostInit = (StudioPostInt)(ScanAddress(0x5D0260));

typedef void(__fastcall *SetMode)(int a1, unsigned int a2, unsigned int a3, int a4);
SetMode SetVidMode = (SetMode)(ScanAddress(0x41DD50));