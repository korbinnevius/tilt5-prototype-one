using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interaction
{
	public class TriggerButtonInteractable : Interactable
	{
		[Header("Button Configuration")]
		[SerializeField] private BoxCollider _buttonTriggerArea;
		[SerializeField] private Collider[] _excludeColliders;
		[SerializeField] private LayerMask _pressLayers;
		[SerializeField] private int framesBetweenChecks = 4;
		
		[Header("Pressing Configuration")] [SerializeField]
		private Transform plunger;

		[SerializeField] private Transform upTransform;
		[SerializeField] private Transform downTransform;
		private int counter;

		private void Awake()
		{
			counter = Random.Range(0, framesBetweenChecks);
		}

		private void Update()
		{
			plunger.position = Interacting ? downTransform.position : upTransform.position;
			counter--;
			if (counter <= 0)
			{
				CheckButton();
				counter = framesBetweenChecks;
			}
		}

		void CheckButton()
		{
			var bounds = _buttonTriggerArea.bounds;
			var colliders = Physics.OverlapBox(bounds.center, bounds.extents, _buttonTriggerArea.transform.rotation, _pressLayers,QueryTriggerInteraction.Ignore);
			int pressing = colliders.Where(x => !_excludeColliders.Contains(x)).Count();
			SetInteracting(pressing>0);
		}
		private void OnTriggerEnter(Collider other)
		{
			CheckButton();
		}

		private void OnTriggerExit(Collider other)
		{
			CheckButton();
		}
	}
}