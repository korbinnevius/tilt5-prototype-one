using Ship;
using UnityEngine;

namespace Interaction
{
	public class PlayerInteractionHandler : MonoBehaviour
	{
		[SerializeField] private InteractionZone _zone;
		[SerializeField] private GrabHandler _grabHandler;
		[SerializeField] private float throwForce=0.5f;

		public Player Player => _player;
		private Player _player;
		public Interaction CurrentInteraction => _currentInteraction;
		private Interaction _currentInteraction = new Interaction();
		
		public void SetPlayer(Player player)
		{
			_player = player;
		}
		private void Awake()
		{
			_zone.SetPlayer(this);
		}

		public void Interact()
		{
			if (_currentInteraction != null && _currentInteraction.Interact != null)
			{
				_currentInteraction.Interact.Invoke();
				_currentInteraction = new Interaction();
			}
		}

		private void GetCurrentInteraction()
		{
			//First, we check if we are holding something. If so, we call interact on the grabber, which throws.
			if (_grabHandler.IsHolding)
			{
				//todo: Make set function for Interact
				_currentInteraction.Verb = "Throw";
				_currentInteraction.Interest = _grabHandler.HoldingGrabbable;
				_currentInteraction.Interact = delegate
				{
					var dir = _player.Input.WorldInputDirection;
					var force = (dir.normalized + _player.transform.forward * 0.1f) * throwForce;
					_grabHandler.Throw(force);
				};
				return;
			}

			//Then, we look for things to pick up, which we prioritize over the closest thing, and interact with it.
			if (_zone.TryGetClosestInteractableOfType<Grabbable>(out var grabbable))
			{
				if (_grabHandler.CanGrab(grabbable))
				{
					_currentInteraction.Verb = "Grab";
					_currentInteraction.Interest = grabbable;
					_currentInteraction.Interact = delegate { _grabHandler.TryGrab(grabbable); };
					return;
				}
				
			}

			//then, we interact with anything else.
			if (_zone.TryGetClosestInteractable(out var interactable))
			{
				if (interactable.CanInteract)
				{
					_currentInteraction.Verb = interactable.Verb;
					_currentInteraction.Interest = interactable;
					_currentInteraction.Interact = interactable.DirectInteract;
					return;
				}
			}

			_currentInteraction.Clear();
		}
		private void Update()
		{
			GetCurrentInteraction();
		}
	}
}