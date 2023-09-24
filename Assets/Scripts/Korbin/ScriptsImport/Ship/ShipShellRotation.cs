using System.Collections;
using UnityEngine;

namespace Ship
{
	public class ShipShellRotation : MonoBehaviour
	{
		[SerializeField] private Interactable ClockwiseInput;
		[SerializeField] private Interactable CounterClockwiseInput;

		public Quaternion currentTarget;

		[SerializeField] private float rotationSpeed;

		//breaks if >360.
		[SerializeField] private float degreeSnap;

		private bool _isRotating;
		public bool IsRotating => _isRotating;

		void Start()
		{
			_isRotating = false;
			currentTarget = transform.rotation;
		}

		public float GetCurrentRotationAsDegrees()
		{
			var degrees = transform.eulerAngles.y;
			degrees = degrees.RoundAndNormalizeDegrees360();
			return degrees;
		}

		private IEnumerator RotateToTarget(Quaternion target)
		{
			_isRotating = true;
			var start = transform.rotation;
			var end = target;
			float t = 0f;
			var timeToRotate = rotationSpeed / degreeSnap;
			while (t < 1)
			{
				t += Time.deltaTime / timeToRotate;
				transform.rotation = Quaternion.Lerp(start, end, t);
				yield return null;
			}

			transform.rotation = end;
			_isRotating = false;
		}

		public void Update()
		{
			if (!_isRotating)
			{
				//check buttons
				int cw = ClockwiseInput.Interacting ? 1 : 0;
				int ccw = CounterClockwiseInput.Interacting ? -1 : 0;
				int direction = cw + ccw; //0 when neither or both, 1 or -1 when just one.
				if (direction != 0)
				{
					currentTarget = Quaternion.Euler(0, (GetCurrentRotationAsDegrees() + degreeSnap * direction).RoundAndNormalizeDegrees360(), 0);
					StartCoroutine(RotateToTarget(currentTarget));
				}
			}
		}

		
	}
}