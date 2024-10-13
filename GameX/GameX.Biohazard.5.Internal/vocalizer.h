#pragma once

#ifdef DLL_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif

class Vocalizer
{
public:
	// Props
	static bool Enabled;
	static bool PerformingActionResponse;
	static char Hotkeys[9];
	static unsigned short Group1[6];
	static unsigned short Group2[6];
	static unsigned short Group3[6];
	static unsigned short Group4[6];
	static unsigned short Group5[6];
	static unsigned short Group6[6];
	static unsigned short Group7[6];
	static unsigned short Group8[6];
	static unsigned short Group9[6];
	static unsigned short InternalGroup1[3];
	static unsigned short InternalGroup2[4];

	// Methods
	static void CycleGroup(unsigned short* Group);
	static void Initialize();
	static void Update();
};

extern "C" {
	DLL_API void Enable(bool Enable);
	DLL_API void UpdateGroup1(unsigned short values[5]);
	DLL_API void UpdateGroup2(unsigned short values[5]);
	DLL_API void UpdateGroup3(unsigned short values[5]);
	DLL_API void UpdateGroup4(unsigned short values[5]);
	DLL_API void UpdateGroup5(unsigned short values[5]);
	DLL_API void UpdateGroup6(unsigned short values[5]);
	DLL_API void UpdateGroup7(unsigned short values[5]);
	DLL_API void UpdateGroup8(unsigned short values[5]);
	DLL_API void UpdateGroup9(unsigned short values[5]);
	DLL_API void UpdateInternalGroup1(unsigned short values[3]);
	DLL_API void UpdateInternalGroup2(unsigned short values[4]);
	DLL_API void UpdateHotkeys(char values[9]);
}