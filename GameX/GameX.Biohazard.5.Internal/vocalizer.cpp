#include "pch.h"

bool Vocalizer::Enabled = false;
unsigned short Vocalizer::Group1[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group2[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group3[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group4[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group5[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group6[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group7[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group8[6] = { 0, 0, 0, 0, 0, 0 };
unsigned short Vocalizer::Group9[6] = { 0, 0, 0, 0, 0, 0 };
char Vocalizer::Hotkeys[9] = { VK_NUMPAD1, VK_NUMPAD2, VK_NUMPAD3, VK_NUMPAD4, VK_NUMPAD5, VK_NUMPAD6, VK_NUMPAD7, VK_NUMPAD8, VK_NUMPAD9 };

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

void UpdateHotkeys(char values[9]) { std::copy(values, values + 9, Vocalizer::Hotkeys); }