using UnityEngine;

namespace Ship
{
	public class EnableChildrenWhenPowered : MonoBehaviour
	{
		[SerializeField] private Station _station;

		
		private void OnEnable()
		{
			_station.OnIsPoweredChange += SetChildren;
		}

		private void OnDisable()
		{
			_station.OnIsPoweredChange -= SetChildren;
		}

		public void SetChildren(bool active)
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(active);
			}
		}
	}
}