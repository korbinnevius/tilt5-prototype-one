using Timeline;

namespace Ship
{
	//An asteroid defense array can spend energy to stop asteroids, and a laser shield will prevent laser attacks.
	
	public class DamageTypeDefenseStation : Station
	{
		public ShipDamageType DamageType;
		public int energyBurnNeededToPreventOneDamageType=1;

		public void ProcessShipEvent(ref ShipEvent shipEvent)
		{
			bool tryReduce = true;
			while (tryReduce)
			{
				//keep trying until we cannot.
				tryReduce = TryReduceOneDamage(ref shipEvent);
			}
		}

		private bool TryReduceOneDamage(ref ShipEvent shipEvent)
		{
			if (isPowered && shipEvent.damage > 0 && shipEvent.DamageType == DamageType)
			{
				if (energyBurnNeededToPreventOneDamageType > 0)
				{
					if (energyBank.TryBurnResources(energyBurnNeededToPreventOneDamageType))
					{
						shipEvent.damage--;
						return true;
					}
				}
				else
				{
					shipEvent.damage--;
					return true;
					//reduce no matter what.
				}
			}

			return false;
		}
		
	}
}