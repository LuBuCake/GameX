// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

volatile bool ShouldRun = true;

DWORD WINAPI MainThread(LPVOID param)
{
    Vocalizer::Initialize();

    while (ShouldRun)
    {
        Vocalizer::Update();
    }

    return 0;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    static HANDLE hThread = NULL;

    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
            hThread = CreateThread(0, 0, MainThread, hModule, 0, 0);
            break;
        case DLL_PROCESS_DETACH:
            ShouldRun = false;
            if (hThread)
            {
                WaitForSingleObject(hThread, INFINITE);
                CloseHandle(hThread);
            }
            break;
    }

    return TRUE;
}

extern "C" __declspec(dllexport) void Finish()
{
    ShouldRun = false;
}
