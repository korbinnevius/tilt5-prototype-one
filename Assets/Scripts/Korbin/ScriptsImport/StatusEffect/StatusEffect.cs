using Timeline;
using UnityEngine;

namespace Ship
{
	[CreateAssetMenu(fileName = "Status Effect", menuName = "Ship/Status Effect", order = 0)]
	public class StatusEffect : ScriptableObject
	{
		public string messageText;
		
		public Ship Ship => _ship;
		private Ship _ship;

		//config
		public bool expires;
		public int duration;
		
		//prefab spawning
		private int _currentDuration;

		[Header("Configuration")] public GameObject InstantiateDuringLifetimePrefab;
		public GameObject InstantiateOnGainPrefab;

		private GameObject _lifetimeGameObject;
			public bool IsActive => _ship != null;//true on gain, false on lose. Or check ship if we are in their list... or just this, so long as it reset.
		public virtual void OnGain(Ship ship)
		{
			if (_ship != null)
			{
				Debug.Log("Already have status effect!");
				_currentDuration = duration;
			}
			else
			{
				//only subscribe once
				_ship.GameTimeline.OnBeat += Beat;
			}
			
			_ship = ship;
			_currentDuration = duration;
			Debug.Log($"Status Effect {name} Gained");

			if (InstantiateDuringLifetimePrefab != null)
			{
				if (_lifetimeGameObject == null)
				{
					_lifetimeGameObject = Instantiate(InstantiateDuringLifetimePrefab);
				}
			}

			if (InstantiateOnGainPrefab != null)
			{
				Instantiate(InstantiateOnGainPrefab);
			}
		}

		private void Beat()
		{
			if (expires)
			{
				_currentDuration--;
				if (duration <= 0)
				{
					LoseEffect();
				}
			}
		}

		public virtual void Tick()
		{
			
		}

		public virtual void OnLose()
		{
			Debug.Log($"Status Effect {name} Over");
			_ship.GameTimeline.OnBeat -= Beat;
			_ship = null;
			if (_lifetimeGameObject != null)
			{
				Destroy(_lifetimeGameObject);
			}
		}
		
		//convenience wrapper.
		public void LoseEffect()
		{
			_ship.LoseStatusEffect(this);
		}

		public MessageInfo GetMessage()
		{
			return new MessageInfo(messageText);
		}
	}
}