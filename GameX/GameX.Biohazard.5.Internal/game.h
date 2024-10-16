#pragma once

class Game
{
public:
    static int GetBaseAddress();
    static int GetBasePlayerAddress(int Index);

    static int GetPlayerQuantity();
    static int GetLocalPlayerIndex();

    static bool IsPlayerSpeaking(int PlayerBaseAddress);
    static int GetRealTimeInventoryItemBaseAddress(int PlayerBaseAddress, int Slot);

    static void PlaySpeech(int PlayerBaseAddress, unsigned short Speech);
};