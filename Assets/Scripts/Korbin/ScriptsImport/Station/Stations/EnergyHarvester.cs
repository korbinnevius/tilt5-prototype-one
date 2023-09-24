using Timeline;
using UnityEngine;

namespace Ship
{
	public class EnergyHarvester : Station
	{
		public GameTimeline _Timeline;
		
		//it might NOT be a station?

		//it can be turned on/off. 

		//It's a converter. It has an input resource, an output resource, a number of times per beat to generate, and a distribution curve.
		//every frame we check if the current time left in the beat is, on the distribution curve, larger than the next threshold.

		//what powers the energy harvester?
		//It also creates WASTE, which need to be sent in the trash chute.

		//are there other converters?
		
		[SerializeField] private GameObject _energyPrefab;
		[SerializeField] private Transform _energySpawnPoint;


		private float _harvestTimer;
		[SerializeField] private float _harvestRatePerBeat = 6f;

		private float TimePerHarvest => _harvestRatePerBeat / _Timeline.TimeBetweenBeats;

		[SerializeField] private ResourceAreaMonitor _mineralMonitor;
		
		private void Awake()
		{
			_harvestTimer = 0;
		}

		private void Update()
		{
			if (isPowered)
			{
				HarvestTick();
			}
		}

		private void HarvestTick()
		{
			if (_mineralMonitor.AnyResources)
			{
				_harvestTimer += Time.deltaTime;
				if(_harvestTimer >= TimePerHarvest)
				{
					_harvestTimer = 0;
					Harvest();
				}
			}

		}

		private void Harvest()
		{
			_mineralMonitor.TryBurnResources(1);
			Instantiate(_energyPrefab, _energySpawnPoint.position, _energyPrefab.transform.rotation);
		}
		
	}
}