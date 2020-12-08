#include <Windows.h>
#include <string>
#include <tlhelp32.h>

HANDLE hProcess;

using namespace std;

namespace Offsets
{
	DWORD Client;
	DWORD dwLocal = 0x7A394C; //OLD: 0x7A394C; //OLD: 0x7CA4BC; //OLD: 0x6FD9D8;
	DWORD dwJump = 0x739948; //OLD: 0x739948; //OLD: 0x6BA20C; //OLD: 0x739890; //OLD: 0x72DCF0;
	DWORD dwHealth = 0x0EC;
	DWORD dwFlags = 0x0F0;
}

#define FL_ONGROUND ( 1 << 0 )

uintptr_t GetModuleBaseAddress(DWORD procId, const wchar_t* modName)
{
	uintptr_t modBaseAddr = 0;
	HANDLE hSnap = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE | TH32CS_SNAPMODULE32, procId);
	if (hSnap != INVALID_HANDLE_VALUE)
	{
		MODULEENTRY32 modEntry;
		modEntry.dwSize = sizeof(modEntry);
		if (Module32First(hSnap, &modEntry))
		{
			do
			{
				if (!_wcsicmp((const wchar_t*)modEntry.szModule, modName))
				{
					modBaseAddr = (uintptr_t)modEntry.modBaseAddr;
					break;
				}
			} while (Module32Next(hSnap, &modEntry));
		}
	}
	CloseHandle(hSnap);
	return modBaseAddr;
}

DWORD GetProcId(const wchar_t* procName)
{
	DWORD procId = 0;
	HANDLE hSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	if (hSnap != INVALID_HANDLE_VALUE)
	{
		PROCESSENTRY32 procEntry;
		procEntry.dwSize = sizeof(procEntry);

		if (Process32First(hSnap, &procEntry))
		{
			do
			{
				if (!_wcsicmp((const wchar_t*)procEntry.szExeFile, procName))
				{
					procId = procEntry.th32ProcessID;
					break;
				}
			} while (Process32Next(hSnap, &procEntry));

		}
	}
	CloseHandle(hSnap);
	return procId;
}

template <class T>
T RM(HANDLE hd, DWORD adress)
{
	T out;
	//ReadProcessMemory(pMemory->HandleProcess, (LPVOID)adress, &out, sizeof(T), NULL);
	ReadProcessMemory(hd, (LPVOID)adress, &out, sizeof(T), NULL);
	return out;
}

template <class T>
void WM(HANDLE hd, DWORD adress, T value)
{
	//WriteProcessMemory(pMemory->HandleProcess, (LPVOID)adress, &value, sizeof(T), 0);
	WriteProcessMemory(hd, (LPVOID)adress, &value, sizeof(T), 0);
}

struct Player
{
	DWORD Base()
	{
		return RM<DWORD>(hProcess, Offsets::Client + Offsets::dwLocal);
	}

	int Jump()
	{
		return RM<int>(hProcess, Offsets::Client + Offsets::dwJump);
	}

	int Flags()
	{
		DWORD Base = this->Base();
		return RM<int>(hProcess, Base + Offsets::dwFlags);
	}

	void SetJump(int v)
	{
		WM<int>(hProcess, Offsets::Client + Offsets::dwJump, v);
	}

	int GetHealth()
	{
		DWORD Base = this->Base();
		return RM<int>(hProcess, Base + Offsets::dwHealth);
	}
};

struct Player *pPlayer;

DWORD WINAPI Loop(PVOID pThreadParameter)
{
	for (;; Sleep(7))
	{
		//printf("Health: %d\n", pPlayer->GetHealth());
		if (pPlayer->Jump() == 5)
		{
			while (GetAsyncKeyState(VK_SPACE))
			{
				if (pPlayer->Flags() & FL_ONGROUND)
				{
					pPlayer->SetJump(5);
				}
				else
				{
					pPlayer->SetJump(4);
				}
			}
		}

		if (!FindWindow(NULL, L"Left 4 Dead 2") || GetAsyncKeyState(VK_END))
		{
			exit(0);
		}
	}
}

void Console()
{
	printf("Auto Bunny Hop By DavidTheTech\n");
	printf("Press End to exit\n");
}

int main()
{
	SetConsoleTitle(L"Auto bunny hop");
	if (!FindWindow(NULL, L"Left 4 Dead 2"))
	{
		printf("\nPlease open Left 4 Dead 2\n");
		system("pause");
		exit(0);
	}
	
	DWORD ProcID = GetProcId(L"left4dead2.exe");
	hProcess = OpenProcess(PROCESS_ALL_ACCESS, NULL, ProcID);

	Offsets::Client = GetModuleBaseAddress(ProcID, L"client.dll");
	//printf("Client.dll: %ld\n", Offsets::Client);

	Console();
	HANDLE Threads[] =
	{
		CreateThread(0, 0, &Loop, 0, 0, 0),
	};
	WaitForMultipleObjects(sizeof(Threads) / sizeof(HANDLE), Threads, TRUE, INFINITE);
	return 0;
}