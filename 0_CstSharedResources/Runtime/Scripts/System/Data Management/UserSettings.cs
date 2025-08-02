using UnityEngine;

/// <summary>
/// A static wrapper class for easily manipulating PlayerPref keys.
/// </summary>
public static class UserSettings
{
	#region Audio Settings
	public static float MasterVolume
	{
		get { return PlayerPrefsRuntimeHelper.GetValue(PlayerPrefsKey.MasterVolume, 1f); }
		set { PlayerPrefsRuntimeHelper.SetValue(PlayerPrefsKey.MasterVolume, value); }
	}

	public static float MusicVolume
	{
		get { return PlayerPrefsRuntimeHelper.GetValue(PlayerPrefsKey.MusicVolume, 1f); }
		set { PlayerPrefsRuntimeHelper.SetValue(PlayerPrefsKey.MusicVolume, value); }
	}

	public static float SoundVolume
	{
		get { return PlayerPrefsRuntimeHelper.GetValue(PlayerPrefsKey.SoundVolume, 1f); }
		set { PlayerPrefsRuntimeHelper.SetValue(PlayerPrefsKey.SoundVolume, value); }
	}

	public static float AmbienceVolume
	{
		get { return PlayerPrefsRuntimeHelper.GetValue(PlayerPrefsKey.AmbienceVolume, 1f); }
		set { PlayerPrefsRuntimeHelper.SetValue(PlayerPrefsKey.AmbienceVolume, value); }
	}

	public static float ToMixerDecibel(float amount) => Mathf.Log10(amount) * 20f;
	#endregion


	#region Graphics Settings
	public static int QualityLevel
	{
		get { return PlayerPrefsRuntimeHelper.GetValue(PlayerPrefsKey.QualityLevel, 1); }
		set { PlayerPrefsRuntimeHelper.SetValue(PlayerPrefsKey.QualityLevel, value); }
	}

	public static int TargetFramerate
	{
		get { return PlayerPrefsRuntimeHelper.GetValue(PlayerPrefsKey.TargetFramerate, 60); }
		set { PlayerPrefsRuntimeHelper.SetValue(PlayerPrefsKey.TargetFramerate, value); }
	}

	public static int UseVsync
	{
		get { return PlayerPrefsRuntimeHelper.GetValue(PlayerPrefsKey.UseVsync, 0); }
		set { PlayerPrefsRuntimeHelper.SetValue(PlayerPrefsKey.UseVsync, value); }
	}
	#endregion

	#region Gameplay Settings
	public static float AimSpeed
	{
		get { return PlayerPrefsRuntimeHelper.GetValue("AimSpeed", 3f); }
		set { PlayerPrefsRuntimeHelper.SetValue("AimSpeed", value); }
	}

	public static int DialogueSpeed
	{
		get { return PlayerPrefsRuntimeHelper.GetValue("DialogueSpeed", 50); }
		set { PlayerPrefsRuntimeHelper.SetValue("DialogueSpeed", value); }
	}
	#endregion

	/// <summary>
	/// Resets all the settings in the specified section to their default value.
	/// </summary>
	/// <param name="section"></param>
	public static void ResetToDefault(SettingSection section)
	{
		switch (section)
		{
			case SettingSection.Audio:
				MasterVolume = 1f;
				MusicVolume = 1f;
				SoundVolume = 1f;
				AmbienceVolume = 1f;
				break;

			case SettingSection.Graphics:
				QualityLevel = 1;
				TargetFramerate = 60;
				UseVsync = 0;
				break;

			case SettingSection.Gameplay:
				AimSpeed = 3f;
				DialogueSpeed = 50;
				break;

			case SettingSection.All:
				MasterVolume = 1f;
				MusicVolume = 1f;
				SoundVolume = 1f;
				AmbienceVolume = 1f;

				QualityLevel = 3;
				TargetFramerate = 60;
				UseVsync = 0;

				AimSpeed = 3f;
				DialogueSpeed = 50;
				break;
		}
	}

	/// <summary>
	/// Deletes the specified key by name, or you can optionally specified whether to delete all keys at once.
	/// <para />
	/// <c>WARNING: This method causes irreversible changes, proceed with your own risk.</c>
	/// </summary>
	/// <param name="keyName">The name of the key to be deleted.</param>
	/// <param name="deleteAll">Optional, set this to true in order to delete all keys at once.</param>
	public static void DeleteKey(string keyName, bool deleteAll = false)
	{
		if (deleteAll)
			PlayerPrefs.DeleteAll();
		else
			PlayerPrefs.DeleteKey(keyName);
	}

	public enum SettingSection
	{
		Audio,
		Graphics,
		Gameplay,
		All
	}
}
