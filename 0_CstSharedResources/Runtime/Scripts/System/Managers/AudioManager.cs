using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;
using UnityRandom = UnityEngine.Random;

namespace CSTGames.SharedResources
{
	[AddComponentMenu("Singletons/Audio Manager")]
	public sealed class AudioManager : PersistentSingleton<AudioManager>
	{
		[Header("Audio Mixer Groups"), Space]
		[SerializeField] private SerializedDictionary<AudioEntry.AudioCategory, AudioMixerGroup> mixerGroupMaps;

		[Header("A list of Audio Entries"), Space]
		[SerializeField] private List<AudioEntry> audioEntries;

		protected override void Awake()
		{
			base.Awake();
			InitializeAudioEntries();
		}

		/// <summary>
		/// Plays an audio entry with a random clip and default pitch.
		/// </summary>
		/// <param name="audioName"></param>
		public void Play(string audioName)
		{
			if (!TryGetAudio(audioName, out AudioEntry chosenAudio))
			{
				Debug.LogWarning($"Audio Clip: {audioName} could not be found!!");
				return;
			}

			chosenAudio.audioSource.clip = GetRandomClip(chosenAudio);

			chosenAudio.audioSource.Play();
		}

		/// <summary>
		/// Plays an audio entry at the clip index with a specified pitch.
		/// </summary>
		/// <param name="audioName"></param>
		/// <param name="clipIndex"> The index of the clip to play. </param>
		/// <param name="pitch"> The specified pitch value. </param>
		public void Play(string audioName, int clipIndex, float pitch)
		{
			if (!TryGetAudio(audioName, out AudioEntry chosenAudio))
			{
				Debug.LogWarning($"Audio Clip: {audioName} could not be found!!");
				return;
			}

			chosenAudio.audioSource.clip = chosenAudio[clipIndex];
			chosenAudio.audioSource.pitch = pitch;

			chosenAudio.audioSource.Play();
		}

		/// <summary>
		/// Plays an audio entry with a random clip in a specified pitch range.
		/// </summary>
		/// <param name="audioName"></param>
		/// <param name="min"> The minimum pitch. </param>
		/// <param name="max"> The maximum pitch. </param>
		public void PlayWithRandomPitch(string audioName, float min, float max)
		{
			if (!TryGetAudio(audioName, out AudioEntry chosenAudio))
			{
				Debug.LogWarning($"Audio Clip: {audioName} could not be found!!");
				return;
			}

			chosenAudio.audioSource.clip = GetRandomClip(chosenAudio);
			chosenAudio.audioSource.pitch = UnityRandom.Range(min, max);

			chosenAudio.audioSource.Play();
		}

		/// <summary>
		/// Stops an audio entry.
		/// </summary>
		/// <param name="audioName"></param>
		public void Stop(string audioName)
		{
			if (!TryGetAudio(audioName, out AudioEntry chosenAudio))
			{
				Debug.LogWarning($"Audio Clip: {audioName} could not be found!!");
				return;
			}

			chosenAudio.audioSource.Stop();
		}

		/// <summary>
		/// Sets a new volume for a specified audio entry.
		/// </summary>
		/// <param name="audioName"></param>
		/// <param name="newVolume"> The new volume value. </param>
		/// <param name="resetToDefault"> If this is true, resets the entry's volume to default. </param>
		public void SetVolume(string audioName, float newVolume, bool resetToDefault = false)
		{
			if (TryGetAudio(audioName, out AudioEntry chosenAudio))
			{
				chosenAudio.audioSource.volume = resetToDefault ? chosenAudio.volume : newVolume;
			}
		}

		/// <summary>
		/// Tries gettings an audio entry with a specified name.
		/// </summary>
		/// <param name="audioName"></param>
		/// <param name="chosenAudio"> The retrieved entry, null if it was not found. </param>
		/// <returns></returns>
		public bool TryGetAudio(string audioName, out AudioEntry chosenAudio)
		{
			chosenAudio = GetAudio(audioName);
			return chosenAudio != null;
		}

		private AudioEntry GetAudio(string audioName)
		{
			audioName = audioName.ToLower().Trim();
			return audioEntries.Find(entry => entry.entryName.ToLower().Equals(audioName));
		}

		private void InitializeAudioEntries()
		{
			foreach (var entry in audioEntries)
			{
				string audioTypeName = entry.audioCategory.ToString();
				var parent = transform.Find(audioTypeName);

				if (parent == null)
				{
					parent = new GameObject(audioTypeName).transform;
				}

				GameObject audioSourceHolder = new(entry.entryName);
				audioSourceHolder.transform.SetParent(parent);

				entry.audioSource = audioSourceHolder.AddComponent<AudioSource>();
				entry.audioSource.outputAudioMixerGroup = mixerGroupMaps[entry.audioCategory];
				entry.audioSource.volume = entry.volume;
				entry.audioSource.pitch = entry.pitch;
				entry.audioSource.loop = entry.isLooped;
				entry.audioSource.playOnAwake = entry.playOnAwake;
			}
		}

		private AudioClip GetRandomClip(AudioEntry target)
		{
			int index = UnityRandom.Range(0, target.clips.Length);
			return target[index];
		}
	}
}