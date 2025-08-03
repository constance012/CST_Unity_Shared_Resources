using System;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// A custom class contains information about Audio Clips.
/// Used by the Audio Manager.
/// </summary>
[Serializable]
public class AudioEntry
{
	public enum AudioCategory
	{
		Sound,
		Music,
		Ambience
	}

	public string entryName;
	public AudioCategory audioCategory;

	[Space] public AudioClip[] clips;

	[Range(0f, 1f), Space] public float volume = 1f;
	[Range(-3f, 3f)] public float pitch = 1f;
	public bool isLooped;
	public bool playOnAwake;

	[HideInInspector] public AudioMixerGroup mixerGroup;
	[HideInInspector] public AudioSource audioSource;

	public AudioClip this[int index] => clips[index];
	public int TotalClips => clips.Length;
}
