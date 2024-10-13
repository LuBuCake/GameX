#include "pch.h"

int Game::GetBaseAddress()
{
    typedef int DAT_0043B5B0(void);
    DAT_0043B5B0* _caller = (DAT_0043B5B0*)reinterpret_cast<void*>(0x43B5B0);
    return _caller();
}

int Game::GetBasePlayerAddress(int Index)
{
    return *(int*)(*(int*)(GetBaseAddress() + 0x103C8) + 0x24 + (Index * 4));
}

int Game::GetPlayerQuantity()
{
    return *(int*)(*(int*)(GetBaseAddress() + 0x103C8) + 0x34);
}

int Game::GetLocalPlayerIndex()
{
    int GameObj = GetBaseAddress();
    int Index = 0;
    unsigned int AND = 1;

    do
    {
        if ((AND & *(unsigned int*)(*(int*)(GameObj + 0x1042C) + 0x47C)) != 0)
            return Index;

        Index += 1;
        AND *= 2;
    } while (Index < 4);

    return Index;
}

bool Game::IsPlayerSpeaking(int PlayerBaseAddress)
{
    return *(int*)(PlayerBaseAddress + 0x2674) != 0;
}

int Game::GetRealTimeInventoryItemBaseAddress(int Player, int Slot)
{
    return Player + 0x21A8 + (Slot * 0x30);
}

void Game::PlaySpeech(int PlayerBaseAddress, unsigned short Speech)
{
    if (Speech == 0)
        return;

    typedef void(__thiscall* FUN_00b7a090)(PVOID, unsigned short, short, unsigned short);
    typedef void(__thiscall* FUN_007972d0)(PVOID, int, unsigned short, unsigned short, unsigned short);

    FUN_00b7a090 _caller_FUN_00b7a090 = (FUN_00b7a090)(0xB7A090);
    FUN_007972d0 _caller_FUN_007972d0 = (FUN_007972d0)(0x7972D0);

    int DAT_0123AD50 = *(int*)(0x123AD50);

    _caller_FUN_00b7a090((PVOID)PlayerBaseAddress, Speech, 0, 1);
    _caller_FUN_007972d0((PVOID)DAT_0123AD50, PlayerBaseAddress, Speech, 0, 1);
}