using System;
using UnityEngine;

namespace CST.Shared.Resources
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlatformerPlayerController : MonoBehaviour, IPlayerController
	{
		[Header("Platformer Stats"), Space]
		[SerializeField] private PlayerPlatformerStats stats;

		[Header("Physic"), Space]
		[SerializeField] private Rigidbody2D rb2D;
		[SerializeField] private CapsuleCollider2D col;
		[SerializeField] private float gravityScaleOnDisabled = 1f;

		[Header("Graphic"), Space]
		[SerializeField] private Transform playerGraphic;

		public static Vector2 Position { get; private set; }
		public static event Action<Vector2> OnVelocityChanged;

		private FrameInputData _frameInput;
		private Vector2 _frameVelocity;
		private bool _raycastStartsInColliders;
		private float _time;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (stats == null)
			{
				Debug.LogWarning("Please assign a ScriptableStats asset to the controller's Stats slot", this);
			}
		}
#endif

		private void Awake()
		{
			_frameInput = new FrameInputData();

			_raycastStartsInColliders = Physics2D.queriesStartInColliders;

			_pressedJumpTime = float.MinValue;
			_leftGroundTime = float.MinValue;

			Position = rb2D.position;
		}

		private void Update()
		{
			_time += Time.deltaTime;

			ReadInputValues();
			CheckFlip();
		}

		private void FixedUpdate()
		{
			UpdateVelocity();
		}

		public void UpdateVelocity()
		{
			CheckForGroundAndCeiling();

			HandleJumping();
			HandleGroundMovement();
			HandleGravity();

			ApplyFrameMovement();
		}

		public void SetEnable(bool state)
		{
			_frameInput.Reset();
			_frameVelocity = Vector2.zero;

			rb2D.gravityScale = state ? 0f : gravityScaleOnDisabled;

			this.enabled = state;
		}

		public void ReadInputValues()
		{
#if ENABLE_INPUT_SYSTEM
			_frameInput.jumpPressedDown = NewInputManager.Instance.WasPressedThisFrame(KeybindingActions.Jump);
			_frameInput.jumpHeld = NewInputManager.Instance.IsPressed(KeybindingActions.Jump);
			_frameInput.direction = NewInputManager.Instance.ReadValue<Vector2>(KeybindingActions.Movement);

#elif ENABLE_LEGACY_INPUT_MANAGER
			_frameInput.jumpPressedDown = LegacyInputManager.Instance.GetKeyDown(KeybindingActions.Jump);
			_frameInput.jumpHeld = LegacyInputManager.Instance.GetKey(KeybindingActions.Jump);

			_frameInput.direction.x = LegacyInputManager.Instance.GetAxisRaw("Horizontal");
			_frameInput.direction.y = LegacyInputManager.Instance.GetAxisRaw("Vertical");
			_frameInput.direction.Normalize();
#endif

			if (_frameInput.jumpPressedDown)
			{
				_needToJump = true;
				_pressedJumpTime = _time;
			}
		}


		#region Collision
		private bool _isGrounded;
		private float _leftGroundTime;

		private void CheckForGroundAndCeiling()
		{
			Physics2D.queriesStartInColliders = false;

			bool groundHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0f, Vector2.down, stats.surfaceCastDistance, ~stats.playerLayer);
			bool ceilingHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0f, Vector2.up, stats.surfaceCastDistance, ~stats.playerLayer);

			if (ceilingHit)
			{
				_frameVelocity.y = Mathf.Min(0f, _frameVelocity.y);
			}

			if (groundHit && !_isGrounded)
			{
				// Landing on the ground.
				_isGrounded = true;
				_isBufferedJumpAvailable = true;
				_isJumpEndedEarly = false;
				_coyoteAvailable = true;
			}
			else if (!groundHit && _isGrounded)
			{
				// Left the ground.
				_isGrounded = false;
				_leftGroundTime = _time;
			}

			Physics2D.queriesStartInColliders = _raycastStartsInColliders;
		}
		#endregion


		#region Jumping
		private bool _needToJump;
		private bool _isBufferedJumpAvailable;
		private bool _isJumpEndedEarly;
		private bool _coyoteAvailable;
		private float _pressedJumpTime;

		private bool HasBufferedJump => _isBufferedJumpAvailable && _pressedJumpTime + stats.jumpBufferTime > _time;
		private bool CanPerformCoyote => _coyoteAvailable && !_isGrounded && _leftGroundTime + stats.coyoteTime > _time;

		private void HandleJumping()
		{
			if (!_isJumpEndedEarly && !_isGrounded && !_frameInput.jumpHeld && rb2D.linearVelocity.y > 0)
			{
				_isJumpEndedEarly = true;
			}

			if (!_needToJump && !HasBufferedJump)
			{
				return;
			}

			if (_isGrounded || CanPerformCoyote)
			{
				PerformJump();
			}

			_needToJump = false;
		}

		private void PerformJump()
		{
			_frameVelocity.y = stats.jumpForce;

			_isBufferedJumpAvailable = false;
			_isJumpEndedEarly = false;
			_coyoteAvailable = false;
			_pressedJumpTime = 0f;
			_leftGroundTime = 0f;
		}
		#endregion


		#region Ground Movement
		private bool _facingRight = true;

		private void HandleGroundMovement()
		{
			if (_frameInput.direction.x == 0f)
			{
				float deceleration = _isGrounded ? stats.groundDeceleration : stats.airDeceleration;
				_frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0f, deceleration * Time.deltaTime);
			}
			else
			{
				float targetSpeed = _frameInput.direction.x * stats.maxGroundSpeed;
				_frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, targetSpeed, stats.groundAcceleration * Time.deltaTime);
			}
		}

		private void CheckFlip()
		{
			bool mustFlip = (_facingRight && _frameInput.direction.x < 0f) || (!_facingRight && _frameInput.direction.x > 0f);

			if (mustFlip)
			{
				playerGraphic.Rotate(0f, -180f, 0f);
				_facingRight = !_facingRight;
			}
		}
		#endregion


		#region Gravity
		private void HandleGravity()
		{
			if (_isGrounded && _frameVelocity.y <= 0f)
			{
				_frameVelocity.y = -stats.groundingForce;
			}
			else
			{
				float gravity = stats.fallGravity;

				if (_isJumpEndedEarly && _frameVelocity.y > 0)
				{
					gravity *= stats.gravityMultiplierWhenEndJumpEarly;
				}

				_frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -stats.maxFallSpeed, gravity * Time.deltaTime);
			}
		}
		#endregion

		private void ApplyFrameMovement()
		{
			rb2D.linearVelocity = _frameVelocity;
			Position = rb2D.position;

			OnVelocityChanged?.Invoke(new Vector2(Mathf.Abs(_frameVelocity.x), _frameVelocity.y));
		}
	}

	public struct FrameInputData
	{
		public bool jumpPressedDown;
		public bool jumpHeld;
		public Vector2 direction;

		public void Reset()
		{
			jumpPressedDown = false;
			jumpHeld = false;
			direction = Vector2.zero;
		}
	}
}