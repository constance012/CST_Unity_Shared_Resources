using UnityEngine;
using DG.Tweening;

public sealed class PickableItem : Interactable, IPoolable
{
	[Header("Overriden Item (Useful for debugging or direct assignment)"), Space]
	[SerializeField] private Item currentItem;
	[SerializeField] private int overrideQuantity;

	[Header("References"), Space]
	[SerializeField] private Rigidbody2D rb2D;

	[Header("Life Time"), Space]
	[Tooltip("The life time of this item after it's dropped, enter -1 to for infinite life time.")]
	[SerializeField] private float lifeTime;

	[Header("Fly Settings"), Space]
	[SerializeField] private float flyDistance;
	[SerializeField] private float flySpeed;
	[SerializeField] private float pickUpMinDistance;
	[SerializeField] private float pickUpFailDelay;

	// Private fields.
	private int _overrideQuantity = -1;
	private float _lifeTime;
	private float _delay;

	private void Start()
	{
		if (currentItem != null)
		{
			Initialize(currentItem, overrideQuantity);
		}
	}

	public void Initialize(Item itemSO, int overrideQuantity)
	{
		_lifeTime = lifeTime;
		_isInteracted = false;
		_overrideQuantity = overrideQuantity;
		_delay = pickUpFailDelay;

		currentItem = Instantiate(itemSO);
		currentItem.name = itemSO.name;

		if (_overrideQuantity != -1)
			currentItem.quantity = Mathf.Clamp(_overrideQuantity, 0, currentItem.maxPerStack);

		spriteRenderer.sprite = currentItem.icon;
	}

	public void Allocate()
	{
		gameObject.SetActive(true);
		transform.localScale = Vector3.one;
	}

	public void Deallocate()
	{
		if (_popupLabel != null)
			Destroy(_popupLabel.gameObject);

		currentItem = null;
		_lifeTime = -1f;
		gameObject.SetActive(false);
	}

	protected override void CheckForInteraction(float mouseDistance, float playerDistance)
	{
		CheckForLifeTime();		

		if (playerDistance <= flyDistance)
			FlyTowardsPlayer();

		base.CheckForInteraction(mouseDistance, playerDistance);
	}

	protected override void CreatePopupLabel()
	{
		Transform foundLabel = _worldCanvas.transform.Find("Popup Label");

		string itemName = currentItem.displayName;
		int quantity = currentItem.quantity;
		Color textColor = currentItem.rarity.color;

		// Create a clone if not already exists.
		if (foundLabel == null)
		{
			base.CreatePopupLabel();
			_popupLabel.SetLabelName(itemName, quantity, textColor);
		}

		// Otherwise, append to the existing one.
		else
		{
			_popupLabel = foundLabel.GetComponent<InteractionPopupLabel>();

			_popupLabel.SetLabelName(itemName, quantity, textColor, true);
			_popupLabel.RestartAnimation();
		}
	}

	public override void Interact()
	{
		if (!_isInteracted)
		{
			Debug.Log($"You're picking up a(n) {currentItem.displayName}");
			
			if (currentItem.autoUse && currentItem.Use(forced: _delay > 0f))
			{
				_isInteracted = true;
				Disable();
			}
			else
			{
				_delay = pickUpFailDelay;
			}
		}
	}

	private void CheckForLifeTime()
	{
		if (_lifeTime != -1f)
		{
			_lifeTime -= Time.deltaTime;
			if (_lifeTime <= 0f)
				Disable();
		}
	}

    private void FlyTowardsPlayer()
	{
		_delay -= Time.deltaTime;

		if (_delay <= 0f)
		{
			Vector2 flyDirection = _playerTransform.position - transform.position;
			rb2D.linearVelocity = flyDirection.normalized * flySpeed;

			if (flyDirection.sqrMagnitude <= Mathf.Pow(pickUpMinDistance, 2))
				Interact();
		}
	}

	private void Disable()
	{
		transform.DOScale(0f, .2f).OnComplete(() => Deallocate());
	}

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, flyDistance);
    }
}