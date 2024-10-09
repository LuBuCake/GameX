#include "pch.h"

bool Utils::IsGameWindowFocused()
{
    HWND gameWindowHandle = FindWindow(NULL, L"RESIDENT EVIL 5");

    if (gameWindowHandle == NULL)
        return false;

    return GetForegroundWindow() == gameWindowHandle;
}

bool Utils::IsKeyPressed(int VK)
{
    return (GetAsyncKeyState(VK) & 0x8000) != 0;
}