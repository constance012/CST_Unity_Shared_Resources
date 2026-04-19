#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityDebug = UnityEngine.Debug;

namespace CSTGames.SharedResources
{
	/// <summary>
	/// Manages inputs from various devices and sources, using the NEW input system.
	/// </summary>
	[AddComponentMenu("Singletons/New Input Manager")]
	public sealed class NewInputManager : Singleton<NewInputManager>
	{
		public event EventHandler OnAttackAction;
		public event EventHandler<InputActionPhase> OnAimModeToggleAction;
		public event EventHandler OnInteractAction;
		public event EventHandler OnReloadAction;
		public event EventHandler OnJumpAction;
		public event EventHandler OnOpenInventoryAction;
		public event EventHandler OnContinueDialogueAction;
		public event EventHandler OnBackToMenuAction;
		public event EventHandler OnSkipPlayableAction;

		// Private fields.
		private PlayerInputActions _playerInputActions;
		private Dictionary<KeybindingAction, InputAction> _inputActions;

		protected override void Awake()
		{
			base.Awake();
			Initialize();
		}

		private void OnDestroy()
		{
			Dispose();
		}

		#region Event methods.
		private void Attack_performed(InputAction.CallbackContext context)
		{
			OnAttackAction?.Invoke(this, EventArgs.Empty);
		}

		private void AimMode_toggled(InputAction.CallbackContext context)
		{
			OnAimModeToggleAction?.Invoke(this, context.phase);
		}

		private void Jump_performed(InputAction.CallbackContext context)
		{
			OnJumpAction?.Invoke(this, EventArgs.Empty);
		}

		private void Interact_performed(InputAction.CallbackContext context)
		{
			OnInteractAction?.Invoke(this, EventArgs.Empty);
		}
		
		private void Reload_performed(InputAction.CallbackContext context)
		{
			OnReloadAction?.Invoke(this, EventArgs.Empty);
		}
		
		private void OpenInventory_performed(InputAction.CallbackContext context)
		{
			OnOpenInventoryAction?.Invoke(this, EventArgs.Empty);
		}
		
		private void ContinueDialogue_performed(InputAction.CallbackContext context)
		{
			OnContinueDialogueAction?.Invoke(this, EventArgs.Empty);
		}

		private void BackToMenu_performed(InputAction.CallbackContext context)
		{
			OnBackToMenuAction?.Invoke(this, EventArgs.Empty);
		}

		private void SkipPlayable_performed(InputAction.CallbackContext context)
		{
			OnSkipPlayableAction?.Invoke(this, EventArgs.Empty);
		}
		#endregion

		#region Get data and value methods.
		public TValue ReadValue<TValue>(KeybindingAction action) where TValue : struct
		{
			return _inputActions[action].ReadValue<TValue>();
		}

		public InputAction GetInputAction(KeybindingAction action)
		{
			return _inputActions[action];
		}

		public string GetDisplayString(KeybindingAction action, int index = 0)
		{
			ReadOnlyArray<InputBinding> bindings = _inputActions[action].bindings;
			index = Mathf.Clamp(index, 0, bindings.Count - 1);

			return bindings[index].ToDisplayString();
		}
		#endregion

		#region Get keys and mouse buttons methods.
		public Vector2 ScrollDelta => Mouse.current.scroll.ReadValue().normalized;
		public Vector2 MousePosition => Mouse.current.position.ReadValue();

		public void WarpCursor(Vector2 position, bool isLocal = false)
		{
			if (!isLocal)
				Mouse.current.WarpCursorPosition(position);
			else
				Mouse.current.WarpCursorPosition(MousePosition + position);
		}

		public bool GetMouseButtonDown(MouseButtonType button)
		{
			return GetMouseButtonControl(button).wasPressedThisFrame;
		}

		public bool GetMouseButtonHeld(MouseButtonType button)
		{
			return GetMouseButtonControl(button).isPressed;
		}

		public bool GetMouseButtonUp(MouseButtonType button)
		{
			return GetMouseButtonControl(button).wasReleasedThisFrame;
		}

		public bool GetKeyDown(Key key)
		{
			return Keyboard.current[key].wasPressedThisFrame;
		}

		public bool GetKeyHeld(Key key)
		{
			return Keyboard.current[key].isPressed;
		}

		public bool GetKeyUp(Key key)
		{
			return Keyboard.current[key].wasReleasedThisFrame;
		}

		public bool WasPressedThisFrame(KeybindingAction action)
		{
			return _inputActions[action].WasPressedThisFrame();
		}

		public bool IsPressed(KeybindingAction action)
		{
			return _inputActions[action].IsPressed();
		}

		public bool WasReleasedThisFrame(KeybindingAction action)
		{
			return _inputActions[action].WasReleasedThisFrame();
		}
		#endregion

		#region Initialization and Clean up.
		private void Initialize()
		{
			SetupInputActions();

			SubscribeCallbacks();
		}

		private void Dispose()
		{
			UnsubscribeCallbacks();
			_playerInputActions.Dispose();
		}

		private void SetupInputActions()
		{
			_playerInputActions = new PlayerInputActions();

			_playerInputActions.Player.Enable();

			_inputActions ??= new Dictionary<KeybindingAction, InputAction>()
			{
				[KeybindingAction.Attack] = _playerInputActions.Player.Attack,
				[KeybindingAction.ToggleAimMode] = _playerInputActions.Player.ToggleAimMode,
				[KeybindingAction.Aiming] = _playerInputActions.Player.Aiming,

				[KeybindingAction.Movement] = _playerInputActions.Player.Movement,
				[KeybindingAction.Jump] = _playerInputActions.Player.Jump,

				[KeybindingAction.Interact] = _playerInputActions.Player.Interact,
				[KeybindingAction.Reload] = _playerInputActions.Player.Reload,
				[KeybindingAction.OpenInventory] = _playerInputActions.Player.OpenInventory,

				[KeybindingAction.ContinueDialogue] = _playerInputActions.Player.ContinueDialogue,
				[KeybindingAction.BackToMenu] = _playerInputActions.Player.BackToMenu,

				[KeybindingAction.SkipPlayable] = _playerInputActions.Player.SkipPlayable,
			};
		}
		#endregion

		#region Callbacks subscription management.
		private void SubscribeCallbacks()
		{
			Subscribe(KeybindingAction.Attack, ActionEventType.Performed, Attack_performed);

			Subscribe(KeybindingAction.ToggleAimMode, ActionEventType.Started, AimMode_toggled);
			Subscribe(KeybindingAction.ToggleAimMode, ActionEventType.Canceled, AimMode_toggled);

			Subscribe(KeybindingAction.Jump, ActionEventType.Performed, Jump_performed);
			Subscribe(KeybindingAction.Interact, ActionEventType.Performed, Interact_performed);
			Subscribe(KeybindingAction.Reload, ActionEventType.Performed, Reload_performed);
			Subscribe(KeybindingAction.OpenInventory, ActionEventType.Performed, OpenInventory_performed);
			Subscribe(KeybindingAction.ContinueDialogue, ActionEventType.Performed, ContinueDialogue_performed);
			
			Subscribe(KeybindingAction.BackToMenu, ActionEventType.Performed, BackToMenu_performed);
			Subscribe(KeybindingAction.SkipPlayable, ActionEventType.Performed, SkipPlayable_performed);
		}

		private void UnsubscribeCallbacks()
		{
			Unsubscribe(KeybindingAction.Attack, ActionEventType.Performed, Attack_performed);

			Unsubscribe(KeybindingAction.ToggleAimMode, ActionEventType.Started, AimMode_toggled);
			Unsubscribe(KeybindingAction.ToggleAimMode, ActionEventType.Canceled, AimMode_toggled);

			Unsubscribe(KeybindingAction.Jump, ActionEventType.Performed, Jump_performed);
			Unsubscribe(KeybindingAction.Interact, ActionEventType.Performed, Interact_performed);
			Unsubscribe(KeybindingAction.Reload, ActionEventType.Performed, Reload_performed);
			Unsubscribe(KeybindingAction.OpenInventory, ActionEventType.Performed, OpenInventory_performed);
			Unsubscribe(KeybindingAction.ContinueDialogue, ActionEventType.Performed, ContinueDialogue_performed);

			Unsubscribe(KeybindingAction.BackToMenu, ActionEventType.Performed, BackToMenu_performed);
			Unsubscribe(KeybindingAction.SkipPlayable, ActionEventType.Performed, SkipPlayable_performed);
		}

		private void Subscribe(KeybindingAction action, ActionEventType eventType, Action<InputAction.CallbackContext> method)
		{
			switch (eventType)
			{
				case ActionEventType.Started:
					_inputActions[action].started += method;
					break;
				case ActionEventType.Performed:
					_inputActions[action].performed += method;
					break;
				case ActionEventType.Canceled:
					_inputActions[action].canceled += method;
					break;
			}
		}

		private void Unsubscribe(KeybindingAction action, ActionEventType eventType, Action<InputAction.CallbackContext> method)
		{
			switch (eventType)
			{
				case ActionEventType.Started:
					_inputActions[action].started -= method;
					break;
				case ActionEventType.Performed:
					_inputActions[action].performed -= method;
					break;
				case ActionEventType.Canceled:
					_inputActions[action].canceled -= method;
					break;
			}
		}
		#endregion

		private ButtonControl GetMouseButtonControl(MouseButtonType button)
		{
			return button switch
			{
				MouseButtonType.Left => Mouse.current.leftButton,
				MouseButtonType.Right => Mouse.current.rightButton,
				MouseButtonType.Middle => Mouse.current.middleButton,
				MouseButtonType.Forward => Mouse.current.forwardButton,
				MouseButtonType.Backward => Mouse.current.backButton,
				_ => Mouse.current.leftButton,  // Default will be the left mouse button.
			};
		}

		enum ActionEventType
		{
			Started,
			Performed,
			Canceled
		}
	}
}
#endif