using UnityEngine;

namespace Ship
{
	[RequireComponent(typeof(Interactable))]
	public class StationInteractable : MonoBehaviour
	{
		[SerializeField] private Station _station;
		private Interactable _interactable;
		private void OnEnable()
		{
			_station.OnIsPoweredChange += OnIsPoweredChange;
		}

		private void OnDisable()
		{
			_station.OnIsPoweredChange -= OnIsPoweredChange;
		}

		private void OnIsPoweredChange(bool powered)
		{
			_interactable.SetInteractable(powered);
		}
	}
}