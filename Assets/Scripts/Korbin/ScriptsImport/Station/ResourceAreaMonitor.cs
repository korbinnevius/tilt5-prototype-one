using System;
using System.Collections.Generic;
using System.Linq;
using Resources;
using UnityEngine;

namespace Ship
{
	public class ResourceAreaMonitor : MonoBehaviour
	{
		public Action OnResourcesChange;
		
		//Tracks the number of Resources in a station.
		//for example, used by power cell area to track power cells.
		public int ResourceCount => GetResourceCount();
		public bool AnyResources => AnyResourcesInArea();

		

		[SerializeField] private ShipResource _resourceToMonitor;
		private List<ResourceElement> _resourcesInArea;
		private Collider _collider;

		private void Awake()
		{
			_resourcesInArea = new List<ResourceElement>();
			_collider = GetComponent<Collider>();
		}

		private int GetResourceCount()
		{
			//probably a way to do this with linq aggregate
			int count = 0;
			foreach (var res in _resourcesInArea)
			{
				count += res.Count;
			}

			return count;
		}

		private bool AnyResourcesInArea()
		{
			return _resourcesInArea.Count > 0;
		}
		private void OnTriggerEnter(Collider other)
		{
			var rese = other.GetComponent<ResourceElement>();
			if (rese != null && rese.Resource == _resourceToMonitor)
			{
				if (!_resourcesInArea.Contains(rese))
				{
					_resourcesInArea.Add(rese);
					OnResourcesChange?.Invoke();
				}
			}
		}

		public bool TryBurnResources(int resourceToBurn)
		{
			var count = ResourceCount;
			if (resourceToBurn <= 0)
			{
				return true;
			}
			if (ResourceCount < resourceToBurn)
			{
				return false;
			}

			//...I wrote this really poorly. This is a mess! There's def a clever way to burn the biggest available resources first.
			
			//rewrite, make an array of all resource sizes, then go in the order "size of burn left and smaller, descending. if needed, get smallest that is larger")
			
			
			_resourcesInArea.OrderByDescending(x => x.Count);

			while (resourceToBurn > 0 && _resourcesInArea.Count > 0)
			{
				var smallerThanBurn = _resourcesInArea.Where(x => x.Count <= resourceToBurn);
				var resourceElements = smallerThanBurn as ResourceElement[] ?? smallerThanBurn.ToArray();
				if (resourceElements.Sum(x => x.Count) > resourceToBurn)
				{
					foreach (var b in resourceElements)
					{
						resourceToBurn -= b.Count;
						b.Burn();
						_resourcesInArea.Remove(b);
						if (resourceToBurn <= 0)
						{
							OnResourcesChange?.Invoke();
							return true;
						}
					}
				}

				resourceToBurn -= _resourcesInArea[0].Count;
				_resourcesInArea[0].Burn();
				_resourcesInArea.RemoveAt(0);
				if (resourceToBurn <= 0)
				{
					OnResourcesChange?.Invoke();
					return true;
				}
			}

			//just call once now, not 
			if (ResourceCount != count)
			{
				OnResourcesChange?.Invoke();
			}
			return resourceToBurn <= 0;
		}

		private void OnTriggerExit(Collider other)
		{
			var rese = other.GetComponent<ResourceElement>();
			if (rese != null)
			{
				if (_resourcesInArea.Contains(rese))//no need to check if the element matches, it probably wont be slower than checking it its in the list or not.
				{
					_resourcesInArea.Remove(rese);
					OnResourcesChange?.Invoke();
				}
			}
		}
	}
}