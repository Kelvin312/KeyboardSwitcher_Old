#define WIN32_LEAN_AND_MEAN
#include <windows.h>

HHOOK hHook;
DWORD threadId;

LRESULT WINAPI CBTProc(int nCode, WPARAM wParam, LPARAM lParam) 
{
	if(nCode == HCBT_SYSCOMMAND && wParam == SC_TASKLIST) return 1;
	return CallNextHookEx(nullptr, nCode, wParam, lParam);
}

extern "C"
__declspec(dllexport)
BOOL WINAPI DisableWinKey() 
{
	//http://ru.stackoverflow.com/q/510916/207326
	//Глобальный отваливается!
	threadId = GetWindowThreadProcessId(GetShellWindow(), nullptr);
	hHook = SetWindowsHookEx(WH_CBT, CBTProc, GetModuleHandleA("WinKeyKiller.dll"), threadId);
	return BOOL(hHook);
}

extern "C"
__declspec(dllexport)
BOOL WINAPI EnableWinKey() 
{
	//https://geektimes.ru/post/174565/
	//Всё равно не работает!
	//if(!hHook) { SetLastError(8320); return FALSE; }
	BOOL result = UnhookWindowsHookEx(hHook);
	//DWORD errCode = GetLastError();
	//PostThreadMessage(threadId, WM_NULL, 0, 0);
	//SetLastError(errCode);
	//hHook = nullptr;
	return result;
}