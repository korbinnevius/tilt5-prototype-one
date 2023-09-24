using System;
using System.Collections.Generic;
using Timeline;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Ship
{
	public class Ship : MonoBehaviour
	{
		public GameTimeline GameTimeline => _gameTimeline;
		[SerializeField] private GameTimeline _gameTimeline;
		public Action<StatusEffect> OnStatusEffectGained;

		public ShipInfo ShipInfo => _shipInfo;
		[SerializeField] private ShipInfo _shipInfo;
		
		[Header("Configuration")]
		[SerializeField] private int startingHealth = 3;

		[SerializeField] private StatusEffect[] _startingStatusEffects;
		private readonly List<StatusEffect> _statusEffects = new List<StatusEffect>();

		//Station References
		[Header("Station References")]
		public DamageTypeDefenseStation[] DefenseStations;
		public DirectionalDamageShield[] Shields;

		void Start()
		{
			_shipInfo.SetHealth(startingHealth);
			foreach(var effect in _startingStatusEffects)
			{
				GainStatusEffect(effect);
			}
		}

		private void GainStatusEffect(StatusEffect effect)
		{
			_statusEffects.Add(effect);
			effect.OnGain(this);
			OnStatusEffectGained?.Invoke(effect);
		}

		public void LoseStatusEffect(StatusEffect effect)
		{
			_statusEffects.Remove(effect);
			effect.OnLose();
		}


		private void OnEnable()
		{
			_gameTimeline.OnShipEvent += ProcessShipEvent;
		}

		private void OnDisable()
		{
			_gameTimeline.OnShipEvent -= ProcessShipEvent;
		}

		private void Update()
		{
			foreach (var effect in _statusEffects)
			{
				effect.Tick();
			}
		}

		public void ProcessShipEvent(ShipEvent shipEvent)
		{
			//reduce any incoming damage.
			foreach (var shield in Shields)
			{
				shield.ProcessShipEvent(ref shipEvent);
			}
			foreach (var defenseStation in DefenseStations)
			{
				defenseStation.ProcessShipEvent( ref shipEvent);
			}

			//todo: status before or after damage?
			TakeDamage(ref shipEvent);

			//apply status effect
			if (shipEvent.StatusEffect != null)
			{
				GainStatusEffect(shipEvent.StatusEffect);
			}
			
		}

		private void OnDestroy()
		{
			//status's are scriptableobjects, so we should clear out any data they have when we exit play mode, switch scenes, etc
			foreach (var status in _statusEffects)
			{
				//remove from list, but why bother
				status.OnLose();
			}
		}

		private void TakeDamage(ref ShipEvent shipEvent)
		{
			_shipInfo.TakeDamage(shipEvent.damage);
			Debug.Log("Ship Took " + shipEvent.damage + " damage!");
		}
	}
}