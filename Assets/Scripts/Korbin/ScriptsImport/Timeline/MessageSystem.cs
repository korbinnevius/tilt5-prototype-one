using System;
using System.Collections.Generic;
using Ship;
using UnityEngine;

namespace Timeline
{
	public class MessageSystem : MonoBehaviour
	{
		public static Action<MessageInfo> OnNewMessage;
		private GameTimeline _gameTimeline => _ship.GameTimeline;
		private Ship.Ship _ship;
		private readonly List<MessageInfo> _allMessages = new List<MessageInfo>();

		private void Awake()
		{
			_ship = GetComponent<Ship.Ship>();
		}

		private void OnEnable()
		{
			_gameTimeline.OnTimelineStarted += Init;
			_gameTimeline.OnShipEvent += ProcessShipEvent;
			_ship.OnStatusEffectGained += OnGainStatusEffect;
			_ship.ShipInfo.OnTakeDamage += OnTakeDamage;
		}
		
		private void OnDisable()
		{
			_gameTimeline.OnTimelineStarted -= Init;
			_gameTimeline.OnShipEvent -= ProcessShipEvent;
			_ship.OnStatusEffectGained -= OnGainStatusEffect;
			_ship.ShipInfo.OnTakeDamage -= OnTakeDamage;
		}

		private void OnTakeDamage(int damage, int newHealth)
		{
			// if (damage > 0)
			// {
			// 	BroadcastMessage(new MessageInfo($"Ship Took {damage} damage!"));
			// }
		}

		private void OnGainStatusEffect(StatusEffect effect)
		{
			BroadcastMessage(effect.GetMessage());
		}

		private void ProcessShipEvent(ShipEvent shipEvent)
		{
			BroadcastMessage(shipEvent.GetMessage());
		}

		private void BroadcastMessage(MessageInfo message)
		{
			message.MessageSystem = this;
			Debug.Log("Info: "+message.Message);
			_allMessages.Add(message);
			OnNewMessage?.Invoke(message);
		}

		private void Init()
		{
			_allMessages.Clear();
		}

		public MessageInfo[] GetRecentMessages(int count)
		{
			List<MessageInfo> recentMessages = new List<MessageInfo>();
			//reverse loop, end to front.
			for (int i = _allMessages.Count-1; i >=(Mathf.Max(0,_allMessages.Count-1-count)); i--)
			{
				recentMessages.Add(_allMessages[i]);
			}
			recentMessages.Reverse();
			return recentMessages.ToArray();
		}
	}
}