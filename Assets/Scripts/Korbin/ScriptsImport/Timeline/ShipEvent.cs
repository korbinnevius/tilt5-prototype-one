using System;
using Ship;

namespace Timeline
{
	[Serializable]
	public class ShipEvent
	{
		//A ship event is a single thing that happens to a ship. It could be an enemy attack, storm, etc.
		//todo naming
		public string displayName;
		public Sector sector;
		public int damage;
		public ShipDamageType DamageType;
		/// <summary>
		/// Status Effect to apply. Can be null.
		/// </summary>
		public StatusEffect StatusEffect;

		public ShipEvent(ShipEvent clone)
		{
			displayName = clone.displayName;
			sector = clone.sector;
			damage = clone.damage;
			DamageType = clone.DamageType;
		}
		//Asteroid Impact


		public MessageInfo GetMessage()
		{
			string text = "Event: " + displayName;
			if (damage > 0)
			{
				text += $". Ship took {damage} {DamageType.name} Damage";
			}
			else
			{
				text += ". Ship took no damage.";
			}

			return new MessageInfo(text);
		}
	}
}