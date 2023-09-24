using System;
using UnityEngine;

namespace Ship
{
	[CreateAssetMenu(fileName = "Ship Info", menuName = "Ship/Ship Info", order = 0)]
	public class ShipInfo : ScriptableObject
	{
		/// <summary>
		/// Passes along damage taken and new current health
		/// </summary>
		public Action<int,int> OnTakeDamage;

		private int _health;
		public int Health => _health;
		
		public void SetHealth(int health)
		{
			if (health < 0)
			{
				health = 0;
			}
			_health = health;
			OnTakeDamage?.Invoke(0,health);
		}

		public void TakeDamage(int damage)
		{
			if (damage <= 0)
			{
				return;
			}

			_health = _health - damage;
			if (_health < 0)
			{
				_health = 0;
			}

			OnTakeDamage?.Invoke(damage,_health);
		}
		
	}
}