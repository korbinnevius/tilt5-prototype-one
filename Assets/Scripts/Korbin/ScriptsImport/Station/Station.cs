using System;
using UnityEngine;

namespace Ship
{
	public class Station : MonoBehaviour
	{
		public Action<bool> OnIsPoweredChange;
		[SerializeField] private bool requiresEnergyToInteract = true;
		[SerializeField]
		protected ResourceAreaMonitor energyBank;
		public int minimumResourcesToInteract;
		public int energyToBurnOnInteract;
		public Interactable InteractWithStationInteractable;//ie: a button to FIRE ZE MISSILEZ
		public bool IsPowered => isPowered;
		protected bool isPowered;

		[SerializeField] private bool powerSwitchStartState = true;
		private bool powerSwitchedOn = true;//for manual override.

		[SerializeField] private StatusEffect[] _cantDoActionWithEffects;
		private void Start()
		{
			//set initial powered state
			powerSwitchedOn = powerSwitchStartState;
			//force call to update initial state.
			if (!requiresEnergyToInteract)
			{
				isPowered = powerSwitchedOn;
			}
			else
			{
				CheckIfPowered();
			}
			
			OnIsPoweredChange?.Invoke(isPowered);
		}

		private void OnEnable()
		{
			energyBank.OnResourcesChange += CheckIfPowered;
			InteractWithStationInteractable.OnInteractStart += TryStationAction;
			
		}

		private void OnDisable()
		{
			energyBank.OnResourcesChange -= CheckIfPowered;
			InteractWithStationInteractable.OnInteractStart -= TryStationAction;
		}

		private void CheckIfPowered()
		{
			var newIsPowered = powerSwitchedOn;
			
			if (powerSwitchedOn && requiresEnergyToInteract)
			{
				newIsPowered = energyBank.ResourceCount >= minimumResourcesToInteract;
			}

			if (newIsPowered != isPowered)
			{
				isPowered = newIsPowered;
				OnIsPoweredChange?.Invoke(isPowered);
			}
		}

		public void TogglePowerSwitch()
		{
			SetPowerSwitch(!powerSwitchedOn);
		}
		public void SetPowerSwitch(bool power)
		{
			powerSwitchedOn = power;
			CheckIfPowered();//broadcast event
		}

		//todo pass player object along
		protected void TryStationAction()
		{
			if(CanDoStationAction())
			{
				DoStationAction();
			}
		}
		
		protected virtual bool CanDoStationAction()
		{
			//check status effects
			if (_cantDoActionWithEffects.Length > 0)
			{
				foreach (var effect in _cantDoActionWithEffects)
				{
					if (effect.IsActive)
					{
						//todo: broadcast message to player
						return false;
					}
				}
			}

			//check if we have energy
			if (requiresEnergyToInteract)
			{
				if(minimumResourcesToInteract > energyBank.ResourceCount)
				{
					//not enough resources!
					return false;
				}
			}
			
			//check if we can burn energy
			if (energyToBurnOnInteract > 0)
			{
				if (!energyBank.TryBurnResources(energyToBurnOnInteract))
				{
					//failed to burn enough! We wasted resources!
					return false;
				}
			}
			return true;
		}


		protected virtual void DoStationAction()
		{
			Debug.Log($"Station Action! {gameObject.name}");
		}
	}
}