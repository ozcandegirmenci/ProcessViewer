#include "stdafx.h"
#include "ProcessViewer.Hooks.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
		case DLL_PROCESS_DETACH:
			EndHook();
			break;
	}
	return TRUE;
}