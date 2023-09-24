using Timeline;
using UnityEngine;

namespace Ship
{
	public class DirectionalDamageShield : MonoBehaviour
	{
		public GameTimeline Timeline;

		//like DamageTypeDefenseStation, but not powered.
		//todo Should refactor to combine these, and this can optionally be connected to a station and powered, or not[
		public ShipDamageType DamageType;

		[SerializeField] private int maxDamageToReduce = 3;

		[SerializeField] private Transform forwardTransform;

		private void OnValidate()
		{
			if (forwardTransform == null)
			{
				forwardTransform = transform;
			}
		}

		public void ProcessShipEvent(ref ShipEvent shipEvent)
		{
			Sector sector = Timeline.GetSectorInDirection(forwardTransform.forward);
			if (shipEvent.sector == sector && shipEvent.DamageType == DamageType)
			{
				//todo move reduce to function on shipevent.
				shipEvent.damage -= maxDamageToReduce;
				if (shipEvent.damage < 0)
				{
					shipEvent.damage = 0;
				}
			}
		}
	}
}