using AYellowpaper.SerializedCollections;
using UnityEngine;

/// <summary>
/// A scriptable object for creating a set of keys use in keybinding.
/// </summary>
namespace CSTGames.SharedResources
{
	[CreateAssetMenu(fileName = "New Keyset", menuName = "Keybinding/Keyset")]
	public class Keyset : ScriptableObject
	{
		[Header("List of keys"), Space]
		public SerializedDictionary<KeybindingAction, KeyCode> keyTable = new SerializedDictionary<KeybindingAction, KeyCode>();

		public int TotalKeys => keyTable.Count;
		public int LastIndex => TotalKeys - 1;

		public KeyCode this[KeybindingAction action]
		{
			get { return keyTable[action]; }
			set { keyTable[action] = value; }
		}
	}
}
