using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ProcAnim
{
	public class NearTargetIKHandler : MonoBehaviour
	{
		private RaycastHit _hit; 
		[SerializeField]
		float castDistance;
		[SerializeField]
		private TwoBoneIKConstraint _constraint;

		[SerializeField] private float moveSpeed;
		private float _targetWeight;
		private void Update()
		{
			SetIKTargetToRaycast(_constraint,transform.position);
			_constraint.weight = Mathf.MoveTowards(_constraint.weight, _targetWeight, Time.deltaTime* moveSpeed);
		}

		private void SetIKTargetToRaycast(TwoBoneIKConstraint ik, Vector3 castStart)
		{
			
			if (Physics.Raycast(castStart, transform.forward, out _hit, castDistance))
			{
				_targetWeight = 1;
				ik.data.target.position = _hit.point;
				//ik.data.target.rotation = Quaternion.FromToRotation(-transform.forward, _hit.normal) * (transform.rotation * rotationOffset);
			}
			else
			{
				_targetWeight = 0;
			}
		}
	}
}