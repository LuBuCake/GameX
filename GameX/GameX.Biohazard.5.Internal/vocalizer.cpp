#include "pch.h"

bool Vocalizer::Enabled = false;
bool Vocalizer::PerformingActionResponse = false;
char Vocalizer::Hotkeys[9] = { VK_NUMPAD1, VK_NUMPAD2, VK_NUMPAD3, VK_NUMPAD4, VK_NUMPAD5, VK_NUMPAD6, VK_NUMPAD7, VK_NUMPAD8, VK_NUMPAD9 };
unsigned short Vocalizer::Group1[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group2[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group3[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group4[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group5[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group6[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group7[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group8[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group9[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::InternalGroup1[3] = { 32, 33, 0 };
unsigned short Vocalizer::InternalGroup2[4] = { 52, 53, 54, 0 };

void Vocalizer::CycleGroup(unsigned short* Group)
{
	Group[5]++;
	Group[5] = Group[5] == 5 ? 0 : Group[5];
}

void Vocalizer::Initialize()
{

}

void Vocalizer::Update()
{
	if (!Enabled || !Utils::IsGameWindowFocused() || Game::GetPlayerQuantity() == 0)
		return;

	int LocalPlayerIndex = Game::GetLocalPlayerIndex();
	int LocalPlayer = Game::GetBasePlayerAddress(LocalPlayerIndex);

	if (LocalPlayer == 0 || Game::IsPlayerSpeaking(LocalPlayer))
		return;

	// Player's vocalizer

	if (Utils::IsKeyPressed(Hotkeys[0]))
	{
		Game::PlaySpeech(LocalPlayer, Group1[Group1[5]]);
		CycleGroup(Group1);
	}
	else if (Utils::IsKeyPressed(Hotkeys[1]))
	{
		Game::PlaySpeech(LocalPlayer, Group2[Group2[5]]);
		CycleGroup(Group2);
	}
	else if (Utils::IsKeyPressed(Hotkeys[2]))
	{
		Game::PlaySpeech(LocalPlayer, Group3[Group3[5]]);
		CycleGroup(Group3);
	}
	else if (Utils::IsKeyPressed(Hotkeys[3]))
	{
		Game::PlaySpeech(LocalPlayer, Group4[Group4[5]]);
		CycleGroup(Group4);
	}
	else if (Utils::IsKeyPressed(Hotkeys[4]))
	{
		Game::PlaySpeech(LocalPlayer, Group5[Group5[5]]);
		CycleGroup(Group5);
	}
	else if (Utils::IsKeyPressed(Hotkeys[5]))
	{
		Game::PlaySpeech(LocalPlayer, Group6[Group6[5]]);
		CycleGroup(Group6);
	}
	else if (Utils::IsKeyPressed(Hotkeys[6]))
	{
		Game::PlaySpeech(LocalPlayer, Group7[Group7[5]]);
		CycleGroup(Group7);
	}
	else if (Utils::IsKeyPressed(Hotkeys[7]))
	{
		Game::PlaySpeech(LocalPlayer, Group8[Group8[5]]);
		CycleGroup(Group8);
	}
	else if (Utils::IsKeyPressed(Hotkeys[8]))
	{
		Game::PlaySpeech(LocalPlayer, Group9[Group9[5]]);
		CycleGroup(Group9);
	}

	// Action responses

	int Animation = *(int*)(LocalPlayer + 0x10E0);

	if (Animation != 112 && Animation != 100 && Animation != 98 && Animation != 31)
		PerformingActionResponse = false;
	else
	{
		if (!PerformingActionResponse)
		{
			PerformingActionResponse = true;

			switch (Animation)
			{
				case 100:
					// Giving item to partner
					Game::PlaySpeech(LocalPlayer, InternalGroup1[InternalGroup1[2]]);
					InternalGroup1[2]++;
					InternalGroup1[2] = InternalGroup1[2] > 1 ? 0 : InternalGroup1[2];
					break;
				case 112:
				case 98:
					// Healing partner
					Game::PlaySpeech(LocalPlayer, InternalGroup2[InternalGroup2[3]]);
					InternalGroup2[3]++;
					InternalGroup2[3] = InternalGroup2[3] > 2 ? 0 : InternalGroup2[3];
					break;
				case 31:
				{
					// Reloading
					for (int i = 0; i < 9; i++)
					{
						int ItemBaseAddress = Game::GetRealTimeInventoryItemBaseAddress(LocalPlayer, i);
						bool Equipped = *(int*)(ItemBaseAddress + 0x18) == 1;

						if (!Equipped)
							continue;

						int Ammount = *(int*)(ItemBaseAddress + 4);
						int MaxAmmount = *(int*)(ItemBaseAddress + 8);

						if (Ammount < (MaxAmmount * 0.5))
							Game::PlaySpeech(LocalPlayer, 66);
					}

					break;
				}
				default:
					break;
			}
		}
	}
}

void Enable(bool Enable) { Vocalizer::Enabled = Enable; }
void UpdateGroup1(unsigned short values[5]) { std::copy(values, values + 5, Vocalizer::Group1); }
void UpdateGroup2(unsigned short values[5]) { std::copy(values, values + 5, Vocalizer::Group2); }
void UpdateGroup3(unsigned short values[5]) { std::copy(values, values + 5, Vocalizer::Group3); }
void UpdateGroup4(unsigned short values[5]) { std::copy(values, values + 5, Vocalizer::Group4); }
void UpdateGroup5(unsigned short values[5]) { std::copy(values, values + 5, Vocalizer::Group5); }
void UpdateGroup6(unsigned short values[5]) { std::copy(values, values + 5, Vocalizer::Group6); }
void UpdateGroup7(unsigned short values[5]) { std::copy(values, values + 5, Vocalizer::Group7); }
void UpdateGroup8(unsigned short values[5]) { std::copy(values, values + 5, Vocalizer::Group8); }
void UpdateGroup9(unsigned short values[5]) { std::copy(values, values + 5, Vocalizer::Group9); }
void UpdateInternalGroup1(unsigned short values[3]) { std::copy(values, values + 3, Vocalizer::InternalGroup1); }
void UpdateInternalGroup2(unsigned short values[4]) { std::copy(values, values + 4, Vocalizer::InternalGroup2); }
void UpdateHotkeys(char values[9]) { std::copy(values, values + 9, Vocalizer::Hotkeys); }