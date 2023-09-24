using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ProceduralWalkHandler : MonoBehaviour
{
    [SerializeField] private float downDistance;
    [SerializeField] private float halfHipWidth;
    [SerializeField] private TwoBoneIKConstraint leftLegIK;
    [SerializeField] private TwoBoneIKConstraint rightLegIK;

    [SerializeField] private Vector3 footOffset;
    [SerializeField] private Quaternion rotationOffset;
    [SerializeField] private LayerMask ikHitTargetMask;
    private RaycastHit _hit = new RaycastHit();
    // Update is called once per frame
    void FixedUpdate()
    {
        
       //if(leftLegIK.data.target.position )
        SetIKTargetToRaycast(leftLegIK,transform.position + -transform.right*halfHipWidth);
        SetIKTargetToRaycast(rightLegIK, transform.position + transform.right * halfHipWidth);
    }

    private void SetIKTargetToRaycast(TwoBoneIKConstraint ik, Vector3 castStart)
    {
        if (Physics.Raycast(castStart, Vector3.down, out _hit, downDistance, ikHitTargetMask,QueryTriggerInteraction.Ignore))
        {
            ik.weight = 1;
            ik.data.target.position = _hit.point+transform.InverseTransformVector(footOffset);
            ik.data.target.rotation = Quaternion.FromToRotation(-transform.forward, _hit.normal)*(transform.rotation*rotationOffset);
        }
        else
        {                   
            ik.weight = 0;
        }
    }
}
