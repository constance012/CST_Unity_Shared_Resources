using System;
using UnityEngine;

namespace CSTGames.SharedResources
{
	[RequireComponent(typeof(Rigidbody2D))]
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

		private Vector2 _movementDirection;
		private Vector2 _previousDirection;
		private float _maxSpeed;
		private float _currentSpeedSquared;

		private void Start()
		{
			_maxSpeed = stats.GetDynamicStat(StatType.MoveSpeed);
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
				_maxSpeed = stats.GetDynamicStat(StatType.MoveSpeed);
			}
		}

		public void ReadInputValues()
		{
#if ENABLE_INPUT_SYSTEM
			_movementDirection = NewInputManager.Instance.ReadValue<Vector2>(KeybindingAction.Movement);

#else
			_movementDirection.x = LegacyInputManager.Instance.GetAxisRaw("Horizontal");
			_movementDirection.y = LegacyInputManager.Instance.GetAxisRaw("Vertical");
			_movementDirection.Normalize();
#endif
		}

		public void UpdateVelocity()
		{
			if (_movementDirection.sqrMagnitude > .01f)
			{
				Vector2 targetSpeed = _movementDirection * _maxSpeed;
				rb2D.linearVelocity = Vector2.MoveTowards(rb2D.linearVelocity, targetSpeed, acceleration * Time.deltaTime);
			}

			else if (_currentSpeedSquared > 0f)
			{
				rb2D.linearVelocity = Vector2.MoveTowards(rb2D.linearVelocity, Vector2.zero, deceleration * Time.deltaTime);
			}

			OnVelocityChanged?.Invoke(rb2D.linearVelocity);
			_currentSpeedSquared = rb2D.linearVelocity.sqrMagnitude;
		}
	}
}
