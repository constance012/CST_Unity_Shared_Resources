using AYellowpaper.SerializedCollections;
using UnityEngine;

/// <summary>
/// A scriptable object for creating a set of keys use in keybinding.
/// </summary>
namespace CST.Shared.Resources
{
	[CreateAssetMenu(fileName = "New Keyset", menuName = "Keybinding/Keyset")]
	public class Keyset : ScriptableObject
	{
		[Header("List of keys"), Space]
		public SerializedDictionary<KeybindingActions, KeyCode> keyTable = new SerializedDictionary<KeybindingActions, KeyCode>();

		public int TotalKeys => keyTable.Count;
		public int LastIndex => TotalKeys - 1;

		public KeyCode this[KeybindingActions action]
		{
			get { return keyTable[action]; }
			set { keyTable[action] = value; }
		}
	}

	public enum KeybindingActions
	{
		Attack = 0,
		ToggleAimMode = 1,
		Aiming = 2,
		MoveLeft = 3,
		MoveRight = 4,
		MoveUp = 5,
		MoveDown = 6,
		Movement = 7,
		Jump = 13,
		ContinueDialogue = 8,
		Interact = 9,
		Reload = 10,
		BackToMenu = 11,
		SkipPlayable = 12
	}

	public enum MouseButtonType
	{
		Left,
		Right,
		Middle,
		Forward,
		Backward
	}
}
