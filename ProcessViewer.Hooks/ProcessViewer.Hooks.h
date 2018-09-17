#ifndef PROCESSVIEWERHOOKS_API 
#define PROCESSVIEWERHOOKS_API __declspec(dllexport)
#endif

#ifndef PROCESSVIEWERHOOKS_SHARED
#define PROCESSVIEWERHOOKS_SHARED extern __declspec(dllimport)
#endif

typedef struct hookMSG
{
	int msg;
	WPARAM wParam;
	LPARAM lParam;
} HOOK_MSG;

typedef struct hookRESULT
{
	UINT msg;
	WPARAM wParam;
	LPARAM lParam;
} HOOK_RESULT;


PROCESSVIEWERHOOKS_SHARED HWND notifyWindow;
PROCESSVIEWERHOOKS_SHARED HWND hookWindow;
static HHOOK hhk;
static LONG originalWindowProcedure;
static HMODULE moduleHandle;

WINUSERAPI int WINAPI StartHook(HWND hWnd, HWND notifyParent);
WINUSERAPI void WINAPI EndHook();
LRESULT CALLBACK OnMessage(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK MessageHookProcedure(int nCode, WPARAM wParam, LPARAM lParam);
