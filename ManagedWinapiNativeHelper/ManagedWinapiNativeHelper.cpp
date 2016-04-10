#define WIN32_LEAN_AND_MEAN
#include <windows.h>

LRESULT InternalCallback(int type, int code, WPARAM wParam, LPARAM lParam);
LRESULT WINAPI wrapper00(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 0, code, wparam, lparam);}
LRESULT WINAPI wrapper01(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 1, code, wparam, lparam);}
LRESULT WINAPI wrapper02(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 2, code, wparam, lparam);}
LRESULT WINAPI wrapper03(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 3, code, wparam, lparam);}
LRESULT WINAPI wrapper04(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 4, code, wparam, lparam);}
LRESULT WINAPI wrapper05(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 5, code, wparam, lparam);}
LRESULT WINAPI wrapper06(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 6, code, wparam, lparam);}
LRESULT WINAPI wrapper07(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 7, code, wparam, lparam);}
LRESULT WINAPI wrapper08(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 8, code, wparam, lparam);}
LRESULT WINAPI wrapper09(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback( 9, code, wparam, lparam);}
LRESULT WINAPI wrapper10(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback(10, code, wparam, lparam);}
LRESULT WINAPI wrapper11(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback(11, code, wparam, lparam);}
LRESULT WINAPI wrapper12(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback(12, code, wparam, lparam);}
LRESULT WINAPI wrapper13(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback(13, code, wparam, lparam);}
LRESULT WINAPI wrapper14(int code, WPARAM wparam, LPARAM lparam) { return InternalCallback(14, code, wparam, lparam);}

const int cntHookType = 15;
const HOOKPROC delegt[cntHookType] = {
	wrapper00, wrapper01, wrapper02, wrapper03,
	wrapper04, wrapper05, wrapper06, wrapper07,
	wrapper08, wrapper09, wrapper10, wrapper11,
	wrapper12, wrapper13, wrapper14
};
HMODULE hModule;
#pragma data_seg(".SHARDATA") //Должен быть глобальным
static HHOOK hHook[cntHookType] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
static HWND hWindow[cntHookType]= {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
#pragma data_seg()
#pragma comment(linker, "/SECTION:.SHARDATA,RWS") // Read Write Shared

//      [DllImport("ManagedWinapiNativeHelper.dll")]
//      private static extern bool AllocHookWrapper(int type, IntPtr hWnd);
//
//      [DllImport("ManagedWinapiNativeHelper.dll")]
//      private static extern bool FreeHookWrapper(int type);

extern "C"
__declspec(dllexport)
BOOL WINAPI AllocHookWrapper(__int32 type, HWND hWnd) 
{
	if(type < 0 || type >= cntHookType || hHook[type]) return FALSE;
	hWindow[type] = hWnd;
	hHook[type] = SetWindowsHookEx(type, delegt[type], hModule, 0);
	return int(hHook[type]);
}

extern "C"
__declspec(dllexport)
BOOL WINAPI FreeHookWrapper(__int32 type) 
{
	if(type < 0 || type >= cntHookType || hHook[type] == nullptr) return FALSE;
	BOOL result = UnhookWindowsHookEx(hHook[type]);
	hHook[type] = nullptr;
	return result;
}

LRESULT InternalCallback(int type, int code, WPARAM wParam, LPARAM lParam)
{
	if (code >= 0)
	{
		if(IsWindow(hWindow[type]))
		{
			MSG msg = {hWindow[type], code, wParam, lParam, 0, 0, 0};
			COPYDATASTRUCT cd = {type, sizeof(msg), &msg};
			BOOL handled = 0;
			if(SendMessageTimeout(hWindow[type], WM_COPYDATA, WPARAM(hModule), LPARAM(&cd),
				SMTO_NORMAL, 50, PDWORD_PTR(&handled)) && handled) return 1;
		}
		else FreeHookWrapper(type);
	}
	return CallNextHookEx(hHook[type], code, wParam, lParam);
}

#pragma unmanaged
BOOL APIENTRY DllMain(HMODULE hMod, DWORD  fdwReason, LPVOID lpReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH) 
	{
		hModule = hMod;
		//if (lpReserved) printf("DLL загружена с неявной компоновкой \n");
	}
	return TRUE;
}


