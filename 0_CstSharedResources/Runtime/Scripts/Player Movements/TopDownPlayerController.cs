using System;
using UnityEngine;

namespace CST.Shared.Resources
{
	public class TopDownPlayerController : MonoBehaviour, IPlayerController, IUpgradeApplicationReceiver
	{
		[Header("References"), Space]
		[SerializeField] private Rigidbody2D rb2D;
		[SerializeField] private Stats stats;

		[Header("Movement Settings"), Space]
		[SerializeField] private float acceleration;
		[SerializeField] private float deceleration;

		public static Vector2 Position { get; private set; }
		public static event Action<Vector2> OnVelocityChanged;

		// Private fields.
		private Vector2 _movementDirection;
		private Vector2 _previousDirection;
		private float _maxSpeed;
		private float _currentSpeed;

		private void Start()
		{
			_maxSpeed = stats.GetDynamicStat(Stat.MoveSpeed);
		}

		private void Update()
		{
			ReadInputValues();

			if (_movementDirection.sqrMagnitude > .01f)
				_previousDirection = _movementDirection;
		}

		private void FixedUpdate()
		{
			UpdateVelocity();

			Position = rb2D.position;
		}

		public void OnUpgradeApplied(Type type, UpgradeBase upgrade)
		{
			if (type == typeof(StatsUpgrade))
			{
				_maxSpeed = stats.GetDynamicStat(Stat.MoveSpeed);
			}
		}

		public void ReadInputValues()
		{
#if ENABLE_INPUT_SYSTEM
		_movementDirection = NewInputManager.Instance.ReadValue<Vector2>(KeybindingActions.Movement);

#elif ENABLE_LEGACY_INPUT_MANAGER
			_movementDirection.x = LegacyInputManager.Instance.GetAxisRaw("Horizontal");
			_movementDirection.y = LegacyInputManager.Instance.GetAxisRaw("Vertical");
			_movementDirection.Normalize();
#endif
		}

		public void UpdateVelocity()
		{
			OnVelocityChanged?.Invoke(rb2D.linearVelocity);

			if (_movementDirection.sqrMagnitude > .01f)
			{
				_currentSpeed += acceleration * Time.deltaTime;
				_currentSpeed = Mathf.Min(_maxSpeed, _currentSpeed);

				rb2D.linearVelocity = _movementDirection * _currentSpeed;
			}

			else if (_currentSpeed > 0f)
			{
				_currentSpeed -= deceleration * Time.deltaTime;
				_currentSpeed = Mathf.Max(0f, _currentSpeed);

				rb2D.linearVelocity = _previousDirection * _currentSpeed;
			}
		}
	}
}
