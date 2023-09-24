using System;
using System.Collections;
//using HDyar.SimpleSOStateMachine;
using UnityEngine;

namespace Timeline
{
	//Timeline fires of events into the scene, can be paused, etc.

	[CreateAssetMenu(fileName = "Game Timeline", menuName = "Ship/Game Timeline", order = 0)]
	public class GameTimeline : ScriptableObject
	{
		//todo: import state machine... subscribe to game state events.
		public Action OnNewBeat;//before waiting for events.
		public Action OnBeat; //before events

		public Action OnTimelineStarted;
		public Action<ShipEvent> OnShipEvent;

		//[SerializeField] private State timelineActiveState;
		public float TimeBetweenBeats => _timeBetweenBeats;
		[SerializeField] private float _timeBetweenBeats;

		[SerializeField] private float degreeOffsetForFirstSector;
		[SerializeField] private Sector[] _sectors; //evenly distributed, in clockwise order.

		[SerializeField] private ShipBeat[] _shipBeats; //this is the actual timeline.

		private ShipBeat[] _activeShipBeats;
		private int _round = 0;

		public bool IsTimelineActive { get; private set; }
		public float CurrentCountdownInBeat { get; private set; }

		public IEnumerator RunTimeline()
		{
			IsTimelineActive = true;
			OnTimelineStarted?.Invoke();
			//beat = [wait... all events] in that order. So on round0 is likely waiting for shipbeat[0].
			for (int i = 0; i < _shipBeats.Length; i++)
			{
				_round = i;
				CurrentCountdownInBeat = _timeBetweenBeats;
				OnNewBeat?.Invoke();
				while (CurrentCountdownInBeat > 0)
				{
					// if (timelineActiveState.IsCurrentState)
					// {
					// 	CurrentCountdownInBeat -= Time.deltaTime;
					// }

					yield return null;
				}

				CurrentCountdownInBeat = 0;

				OnBeat?.Invoke();
				foreach (var sEvent in _shipBeats[i].ShipEvents)
				{
					StartShipEvent(sEvent);
					yield return null; //to wait for animations, ship events could return coroutines.
				}
			}
			//check if we are still alive or not?
			
			//move to game ended state.
			
			IsTimelineActive = false;
		}

		private void StartShipEvent(ShipEvent sEvent)
		{
			//clone the ship event so we don't modify the scriptableObject's timeline.
			OnShipEvent?.Invoke(new ShipEvent(sEvent));
		}

		public bool TryScanForShipEvents(Sector scanSector, out ScanResults scanResults)
		{
			Debug.Log($"Scanning Sector {scanSector.displayName}");
			int maxBeatDistance = 100;

			for (int i = _round; i < _shipBeats.Length; i++)
			{
				for (int j = 0; j < _shipBeats[i].ShipEvents.Length; j++)
				{
					Sector sector = _shipBeats[i].ShipEvents[j].sector;
					if (sector == scanSector && (i - _round < maxBeatDistance))
					{
						scanResults = new ScanResults()
						{
							AnythingScanned = true,
							BeatsUntilEvent = i - _round,
							ScannedEvent = _shipBeats[i].ShipEvents[j]
						};
						return true;
					}
				}
			}

			scanResults = new ScanResults()
			{
				AnythingScanned = false,
				BeatsUntilEvent = 0,
				ScannedEvent = null
			};
			return false;
		}

		public Sector GetSectorInDirection(Vector3 dir)
		{
			float degrees = Quaternion.FromToRotation(Vector3.forward, dir).eulerAngles.y;
			degrees = (degrees - degreeOffsetForFirstSector).RoundAndNormalizeDegrees360();

			int count = _sectors.Length;
			float a = 360f / count;
			int index = Mathf.FloorToInt((degrees) / a);
			return _sectors[index];
		}

		public bool TryGetShipEventInSector(Sector sector, int roundsFromCurrent, out ShipEvent shipEvent)
		{
			int round = _round + roundsFromCurrent;
			if (round < 0 || round >= _shipBeats.Length)
			{
				shipEvent = null;
				return false;
			}

			var beat = _shipBeats[round];
			foreach (var se in beat.ShipEvents)
			{
				if (se.sector == sector)
				{
					shipEvent = se;
					return true;
					//todo: handle case where multiple shipEvents are in the same sector? we might have a "none" sector for generic things.
				}
			}

			shipEvent = null;
			return false;
		}
	}
}