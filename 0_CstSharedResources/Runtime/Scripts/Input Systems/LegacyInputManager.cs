#if ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine;

namespace CST.Shared.Resources
{
	/// <summary>
	/// Manages all the keyboard input for the game, using Unity's legacy input system.
	/// </summary>
	[AddComponentMenu("Singletons/Legacy Input Manager")]
	public class LegacyInputManager : Singleton<LegacyInputManager>
	{
		[Header("Keyset Reference"), Space]
		[SerializeField] private Keyset keySet;

		public KeyCode GetKeyForAction(KeybindingAction action)
		{
			return keySet[action];
		}

		/// <summary>
		/// Returns true while the user holds down the key for the specified action.
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public bool GetKey(KeybindingAction action)
		{
			KeyCode keyCode = GetKeyForAction(action);
			bool result = Input.GetKey(keyCode);

			return result;
		}

		/// <summary>
		/// Returns true during the frame the user starts pressing down the key for the specified action.
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public bool GetKeyDown(KeybindingAction action)
		{
			KeyCode keyCode = GetKeyForAction(action);
			//Debug.Log(keyCode);
			bool result = Input.GetKeyDown(keyCode);

			return result;
		}

		/// <summary>
		/// Returns true during the frame the user releases the key for the specified action.
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public bool GetKeyUp(KeybindingAction action)
		{
			KeyCode keyCode = GetKeyForAction(action);
			bool result = Input.GetKeyUp(keyCode);

			return result;
		}

		/// <summary>
		/// Returns the value of the axis based on which key is being held.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public float GetAxisRaw(string axis)
		{
			axis = axis.ToLower().Trim();

			switch (axis)
			{
				case "horizontal":
					if (GetKey(KeybindingAction.MoveRight))
						return 1f;

					else if (GetKey(KeybindingAction.MoveLeft))
						return -1f;

					else
						return 0f;

				case "vertical":
					if (GetKey(KeybindingAction.MoveUp))
						return 1f;

					else if (GetKey(KeybindingAction.MoveDown))
						return -1f;

					else
						return 0f;
			}

			return 0f;
		}
	}
}
#endif