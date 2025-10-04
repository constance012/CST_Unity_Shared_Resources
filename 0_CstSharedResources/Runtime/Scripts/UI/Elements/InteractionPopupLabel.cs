using TMPro;
using UnityEngine;

namespace CST.Shared.Resources
{
	using static Interactable;

	public class InteractionPopupLabel : MonoBehaviour
	{
		[Header("References"), Space]
		[SerializeField] private TweenableUIMaster tweenable;

		[Space, SerializeField] private TextMeshProUGUI label;
		[SerializeField] private TextMeshProUGUI keyboardCue;
		[SerializeField] private Transform mouseCue;

		[Space, SerializeField] private Transform worldCanvas;

		private void Awake()
		{
			worldCanvas = GameObject.FindWithTag(GlobalDefines.WORLD_CANVAS_TAG).transform;
		}

		public void RestartAnimation()
		{
			tweenable.StartTweening(true);
		}

		public void SetLabelName(string name)
		{
			label.text = name.Trim().ToUpper();
		}

		public void SetLabelName(string name, int quantity, Color textColor, bool isDuplicated = false)
		{
			TextMeshProUGUI chosenLabel = isDuplicated ? Instantiate(label, label.transform.parent) : label;

			chosenLabel.text = quantity > 1 ? $"{name.ToUpper()} x{quantity}" : name.ToUpper();
			chosenLabel.color = textColor;
		}

		public void SetupLabel(Transform interactable, InputSource inputSource)
		{
			SetLabelName("");

#if ENABLE_INPUT_SYSTEM
		keyboardCue.text = NewInputManager.Instance.GetDisplayString(KeybindingActions.Interact);
		
#elif ENABLE_LEGACY_INPUT_MANAGER
			keyboardCue.text = LegacyInputManager.Instance.GetKeyForAction(KeybindingAction.Interact).ToString();
#endif

			switch (inputSource)
			{
				case InputSource.Mouse:
					keyboardCue.gameObject.SetActive(false);
					mouseCue.gameObject.SetActive(true);
					break;

				case InputSource.Keyboard:
					keyboardCue.gameObject.SetActive(true);
					mouseCue.gameObject.SetActive(false);
					break;

				case InputSource.Joystick:
					break;

				case InputSource.None:
					mouseCue.parent.gameObject.SetActive(false);
					break;
			}

			transform.position = interactable.position;
			transform.SetParent(worldCanvas, true);
			transform.SetAsLastSibling();
		}
	}
}
