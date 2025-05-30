﻿using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour, IPoolable
{
	[Header("References"), Space]
	[SerializeField] protected TrailRenderer trail;
	[SerializeField] protected Rigidbody2D rb2D;
	[SerializeField, ReadOnly, Tooltip("The target to track if this projectile is homing.")]
	protected Transform targetToTrack;

	[Header("General Properties"), Space]
	[SerializeField] protected float maxLifeTime;
	[SerializeField, ReadOnly] protected bool isHoming;
	
	[Header("Movement Properties"), Space]
	[SerializeField] protected float flySpeed;
	[Tooltip("How sharp does the projectile turn to reach its target? Measures in deg/s.")]
	public float trackingRigidity;

	// Protected fields.
	protected float _aliveTime;
	protected Transform _shooter;
	protected Stats _shooterStats;

	private void FixedUpdate()
	{
		TravelForwards();
		TrackingTarget();
	}

	private void LateUpdate()
	{
		_aliveTime -= Time.deltaTime;

		if (_aliveTime <= 0f)
			Deallocate();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		ProcessCollision(other.collider);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		ProcessCollision(other);
	}

	public void Allocate()
	{
		gameObject.SetActive(true);
	}

	public void Deallocate()
	{
		flySpeed = 0f;
		trail.emitting = false;
		gameObject.SetActive(false);
	}

	public void SetTarget(Transform target)
	{
		targetToTrack = target;
		isHoming = targetToTrack != null;
	}
	
	public virtual void Initialize(Transform shooter, Stats shooterStats, Transform trackTarget)
	{
		Allocate();
		_shooter = shooter;
		_shooterStats = shooterStats;
		
		SetTarget(trackTarget);

		flySpeed = _shooterStats.GetStaticStat(Stat.ProjectileSpeed);
		trackingRigidity = _shooterStats.GetStaticStat(Stat.ProjectileTrackingRigidity);
		maxLifeTime = _shooterStats.GetStaticStat(Stat.ProjectileLifeTime);

		_aliveTime = maxLifeTime;
	}

	/// <summary>
	/// Determines what happens if this projectile collides with other objects.
	/// </summary>
	/// <param name="other"></param>
	public abstract void ProcessCollision(Collider2D other);

	protected void TravelForwards()
	{
		if (!trail.emitting)
			trail.emitting = true;
			
		rb2D.velocity = transform.right * flySpeed;
	}

	protected void TrackingTarget()
	{
		if (isHoming && targetToTrack != null)
		{
			Vector3 trackDirection = targetToTrack.position - transform.position;
			float angle = Mathf.Atan2(trackDirection.y, trackDirection.x) * Mathf.Rad2Deg;

			rb2D.rotation = Mathf.MoveTowardsAngle(rb2D.rotation, angle, trackingRigidity * Time.deltaTime);
		}
	}

	protected void DamageTarget(Collider2D other, float damageScale)
	{
		IDamageable target = other.GetComponentInParent<IDamageable>();

		if (_shooter != null)
		{
			target?.TakeDamage(_shooterStats, _shooter.position, scaleFactor: damageScale);
		}
	}
}
