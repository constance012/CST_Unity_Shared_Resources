using UnityEngine;

/// <summary>
/// Base class for all interactable objects.
/// </summary>
namespace CST.Shared.Resources
{
	public abstract class Interactable : MonoBehaviour
	{
		public enum InputSource { Mouse, Keyboard, Joystick, None }

		[Header("Type"), Space]
		public InputSource inputSource;

		[Header("Reference"), Space]
		[SerializeField] protected SpriteRenderer spriteRenderer;
		[SerializeField] protected GameObject popupLabelPrefab;

		[Header("Interaction Radius"), Space]
		[SerializeField, Tooltip("The distance required for the player to interact with this object.")]
		protected float interactDistance;

		// Protected fields.
		protected static Transform _playerTransform;
		protected bool _isInteracted;
		protected Transform _worldCanvas;
		protected Material _mat;
		protected InteractionPopupLabel _popupLabel;

		protected virtual void Awake()
		{
			if (_playerTransform == null)
			{
				_playerTransform = GameObject.FindWithTag(GlobalDefines.PLAYER_TAG).transform;
			}

			_worldCanvas = GameObject.FindWithTag(GlobalDefines.WORLD_CANVAS_TAG).transform;
			_mat = spriteRenderer.material;
		}

		protected void Update()
		{
			if (CheckForPlayer(out float mouseDistance, out float playerDistance))
			{
				CheckForInteraction(mouseDistance, playerDistance);
			}
		}

		public abstract void Interact();

		protected virtual void CheckForInteraction(float mouseDistance, float playerDistance)
		{
			if (playerDistance <= interactDistance)
			{
				VisualizeInteraction(playerDistance);
			}
			else
			{
				CancelInteraction(playerDistance);
			}
		}

		protected virtual void VisualizeInteraction(float playerDistance)
		{
			if (_popupLabel == null)
				CreatePopupLabel();
			else
				_popupLabel.transform.position = transform.position;

			_mat.SetFloat("_Thickness", .4f);

			if (LegacyInputManager.Instance.GetKeyDown(KeybindingActions.Interact))
				Interact();

			// TODO - derived classes implement their own way to visualize interaction.
		}

		protected virtual void CancelInteraction(float playerDistance)
		{
			if (_popupLabel != null)
				Destroy(_popupLabel.gameObject);

			_mat.SetFloat("_Thickness", 0f);
			// TODO - derived classes implement their own way to cancel interaction.
		}

		protected virtual void CreatePopupLabel()
		{
			GameObject label = Instantiate(popupLabelPrefab);
			label.name = popupLabelPrefab.name;

			_popupLabel = label.GetComponent<InteractionPopupLabel>();

			_popupLabel.SetupLabel(transform, (InputSource)inputSource);
		}

		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, interactDistance);
		}

		private bool CheckForPlayer(out float mouseDistance, out float playerDistance)
		{
			mouseDistance = int.MaxValue;
			playerDistance = int.MaxValue;

			if (_playerTransform != null)
			{
				Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				mouseDistance = Vector2.Distance(worldMousePos, transform.position);
				playerDistance = Vector2.Distance(_playerTransform.position, transform.position);

				return true;
			}

			return false;
		}
	}
}
