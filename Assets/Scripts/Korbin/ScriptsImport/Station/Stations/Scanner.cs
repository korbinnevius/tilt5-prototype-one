using Timeline;
using UnityEngine;

namespace Ship
{
	public class Scanner : Station
	{
		public GameTimeline Timeline;
		[SerializeField] private ShipShellRotation _shipShellRotation;

		[SerializeField] private bool canScanWhileRotating = false;
		protected override bool CanDoStationAction()
		{
		
			if (!canScanWhileRotating && _shipShellRotation.IsRotating)
			{
				return false;
			}
			return base.CanDoStationAction();
		}

		[ContextMenu("Scan")]

		protected override void DoStationAction()
		{
			//todo: need to add that you can't scan while moving,and i would like to do that in Try, not Do.
			base.DoStationAction();
			//go from orientation 

			Sector sector = Timeline.GetSectorInDirection(transform.forward);
			var scan = Timeline.TryScanForShipEvents(sector, out var results);
			if (scan)
			{
				Debug.Log($"Scan successful! In {results.BeatsUntilEvent} turns there will be a {results.ScannedEvent.displayName} at {results.ScannedEvent.sector.displayName}.");
			}
			else
			{
				Debug.Log($"Scan unsuccessful!");
			}
		}
	}
}