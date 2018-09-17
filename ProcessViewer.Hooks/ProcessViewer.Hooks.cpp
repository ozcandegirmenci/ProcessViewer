#include "stdafx.h"
#include "ProcessViewer.Hooks.h"

// create shared data segment
// and put window and notify window variables there
// these variables must be same for all the instances of this dll
#pragma data_seg(".shared")
HWND notifyWindow = NULL;
HWND hookWindow = NULL;
#pragma data_seg()
#pragma comment(linker, "/SECTION:.shared,RWS") 


PROCESSVIEWERHOOKS_API int WINAPI StartHook(HWND hWnd, HWND notifyParent)
{
	originalWindowProcedure = 0;

	// Load our libary and get its handle
	HINSTANCE phookDll = LoadLibrary((LPCTSTR) _T("ProcessViewer.Hooks.dll"));

	EndHook();

	hookWindow = hWnd;
	notifyWindow = notifyParent;

	// get Thread Id of the Window, which we will hook
	DWORD threadId = GetWindowThreadProcessId(hWnd, NULL);
	// get address of our Hook method in this dll
	HOOKPROC proc = (HOOKPROC)GetProcAddress(phookDll, "MessageHookProcedure");
	// set Hook (This will injects our dll in to all process which are created by the given thread)
	hhk = SetWindowsHookEx(WH_CALLWNDPROC,
							proc,
							phookDll,
							threadId);
 
	return (DWORD)hhk;
}

// new window procedure for the selected window
LRESULT CALLBACK OnMessage(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	// prepare message information, for sending it to our ProcessViewer program
	HOOK_MSG message;
	message.msg = msg;
	message.wParam = wParam;
	message.lParam = lParam;

	// this is for the WM_COPYDATA message,
	// WM_COPYDATA transfers data between 2 process by using this structure
	COPYDATASTRUCT cdata;
	cdata.dwData = 0;
	cdata.cbData = sizeof(message);
	cdata.lpData = &message;


	// send a WM_COPYDATA to our ProcessViewer which will give information about the message
	// to our program
	LRESULT result = SendMessage(notifyWindow, WM_COPYDATA, (WPARAM)hWnd, (LPARAM)&cdata);
	// return values, 1: ignore message, 0 do nothing, 
	// different from 0 and 1 : memory address of data which contains the changed wParam and lParam
	if (result == 1)
		return 0;
	if (result != 0)
	{
		// if result is not 0 or 1
		// this means we are going to change the message, wParam and lParam
		// which we write new values of these variables to the
		// address (result indicates this address) in the hooked program's memory
		HOOK_RESULT* hr = (HOOK_RESULT*)result;
		msg = hr->msg;
		wParam = hr->wParam;
		lParam = hr->lParam;
	}
	// send message to original window procedure
	LRESULT ret = CallWindowProc((WNDPROC)originalWindowProcedure, hWnd, msg, wParam, lParam);
	// send message to the ProcessViewer application which 
	// give information about the result of the message
	SendMessage(notifyWindow, HM_MESSAGE_RESULT, msg, ret);
	return ret;
}	

// this is our WH_CALLWNDPROC hook method
LRESULT CALLBACK MessageHookProcedure(int nCode, WPARAM wParam, LPARAM lParam)
{
	if (originalWindowProcedure == 0)
	{
		// the first time this method is called
		// for the our hooked window, replace its window procedure
		// to our procedure method
		// do this just for the control which we wnat to listen its messages
		CWPSTRUCT* str = (CWPSTRUCT*)lParam;
		if (str->hwnd == hookWindow)
		{
			// take original windows procedure of this control
			originalWindowProcedure = GetWindowLong(hookWindow, GWL_WNDPROC);
			// set new windows procedure
			SetWindowLong(hookWindow, GWL_WNDPROC, (LONG)OnMessage);
		}
	}
	return CallNextHookEx(hhk, nCode, wParam, lParam);
}

// ends hook operation
PROCESSVIEWERHOOKS_API void WINAPI EndHook()
{
	// if we have been taken procedure of the window,
	// set its procedure to the its original
	if (originalWindowProcedure)
		SetWindowLong(hookWindow, GWL_WNDPROC, originalWindowProcedure);
	if (hhk != NULL)
		UnhookWindowsHookEx(hhk);
	originalWindowProcedure = 0;
	hhk = NULL;
}
