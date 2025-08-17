using System;
using UnityEngine;

namespace CST.Shared.Resources
{
	public static class PlayerPrefsRuntimeHelper
	{
		public static T GetValue<T>(PlayerPrefsKey key, T defaultValue)
		{
			return GetValueInternal(key.ToString(), defaultValue);
		}

		public static T GetValue<T>(string key, T defaultValue)
		{
			return GetValueInternal(key, defaultValue);
		}

		public static void SetValue<T>(PlayerPrefsKey key, T newValue)
		{
			SetValueInternal(key.ToString(), newValue);
		}

		public static void SetValue<T>(string key, T newValue)
		{
			SetValueInternal(key, newValue);
		}

		private static T GetValueInternal<T>(string key, T defaultValue)
		{
			if (typeof(T) == typeof(float))
			{
				object value = PlayerPrefs.GetFloat(key.ToString(), (float)(object)defaultValue);
				return (T)value;
			}
			else if (typeof(T) == typeof(string))
			{
				object value = PlayerPrefs.GetString(key.ToString(), (string)(object)defaultValue);
				return (T)value;
			}
			else if (typeof(T) == typeof(int))
			{
				object value = PlayerPrefs.GetInt(key.ToString(), (int)(object)defaultValue);
				return (T)value;
			}
			else
			{
				throw new NotSupportedException($"Type {typeof(T)} is not supported as a PlayerPrefs value!");
			}
		}

		private static void SetValueInternal<T>(string key, T newValue)
		{
			if (typeof(T) == typeof(float))
			{
				PlayerPrefs.SetFloat(key, (float)(object)newValue);
			}
			else if (typeof(T) == typeof(string))
			{
				PlayerPrefs.SetString(key, (string)(object)newValue);
			}
			else if (typeof(T) == typeof(int))
			{
				PlayerPrefs.SetInt(key, (int)(object)newValue);
			}
			else
			{
				throw new NotSupportedException($"Type {typeof(T)} is not supported as a PlayerPrefs value!");
			}
		}
	}

	public enum PlayerPrefsKey
	{
		MasterVolume,
		MusicVolume,
		SoundVolume,
		AmbienceVolume,

		QualityLevel,
		TargetFramerate,
		UseVsync
	}
}