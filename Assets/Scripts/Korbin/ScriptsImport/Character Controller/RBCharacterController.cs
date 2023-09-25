using UnityEngine;

//Physics controller that works by floating above the ground, with spring forces to keep it towards a desired position.

//https://youtu.be/qdskE8PJy6Q

[RequireComponent(typeof(Rigidbody))]
public class RBCharacterController : MonoBehaviour
{
    
    private Rigidbody _rigidbody;
    [SerializeField] private LayerMask groundLayers;
    
    [Header("Vibes")]
    [SerializeField] private float RideHeight;
    [SerializeField] private float groundCastDistance;
    [SerializeField] private float RideSpringForce;//200
    [SerializeField] private float RideSpringDamper;//10
    private Quaternion _uprightTargetRot = Quaternion.identity;
    [SerializeField] private float UprightJointSpringStrength;//40
    [SerializeField] private float UprightJointSpringDamper;//5

    [Header("Locomotion")] [SerializeField]
    private float maxSpeed;//8

    [SerializeField] private float acceleration;//200
    [SerializeField] private AnimationCurve accelerationFactorFromDot;
    [SerializeField] private float maxAccelerationForce;//150
    [SerializeField] private AnimationCurve maxAccelerationForceFactorFromDot;
    [SerializeField] private Vector3 forceScale = new Vector3(1, 0, 1);
    // [SerializeField] private float gravityScaleDrop;//10

    private Vector2 _input;
    private Vector3 _unitGoal;
    private Vector3 _goalVel;
    private RaycastHit _floatRayHit;

    public int maxJumps = 1;
    /// <summary>
    /// Counts the number of times the player has jumped.
    /// </summary>
    private int _jumps;

    private Rigidbody _platform;
    private Transform _platformParent;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _platformParent = new GameObject().transform;
        _platformParent.gameObject.name = gameObject.name + "Platform Handler";
    }

    private void Update()
    {
        UpdatePlatform();
    }

    public void Move(Vector3 input)
    {
        _input = new Vector2(input.x,input.z);
        //fix for camera angle.
        _unitGoal = new Vector3(_input.x, 0, _input.y);

    }

    public void Jump()
    {
        if (maxJumps > _jumps)
        {
            _rigidbody.AddForce(Vector3.up*10f,ForceMode.Impulse);
            _jumps++;
        }
    }

    void FixedUpdate()
    {
        UpdatePlatform();
        FloatForce();
        UprightForce();
        LocomotionForce();
    }

    private void UpdatePlatform()
    {
        if (_platform != null)
        {
            _platformParent.transform.position = _platform.transform.position;
            _platformParent.transform.rotation = _platform.transform.rotation;
        }
    }


    private void LocomotionForce()
    {
        Vector3 unitVel = _goalVel.normalized;
        float velDot = Vector3.Dot(_unitGoal, unitVel);
        float accel = acceleration * accelerationFactorFromDot.Evaluate(velDot);
        Vector3 realGoalVelocity = _unitGoal * maxSpeed;
        var groundVelocity = Vector3.zero;
        _goalVel = Vector3.MoveTowards(_goalVel, realGoalVelocity + groundVelocity, accel * Time.fixedDeltaTime);

        var neededAccel = (_goalVel - _rigidbody.velocity) / Time.fixedDeltaTime;
        float maxAccel = maxAccelerationForce * maxAccelerationForceFactorFromDot.Evaluate(velDot);
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        
        var force = (neededAccel * _rigidbody.mass);
        force.Scale(forceScale);
        _rigidbody.AddForce(force);

        
        if (_floatRayHit.rigidbody != null)
        {
             // force.Scale(forceScale);
             force = -force*0.9f;
            _floatRayHit.rigidbody.AddForceAtPosition(force,_floatRayHit.point);
        }
    }

    private void FloatForce()
    {
        var rayDir = transform.TransformDirection(Vector3.down); //I can think of reasons you might want to change this. We could pull from gravity settings, for example.
        var rayHit = Physics.Raycast(transform.position, rayDir, out _floatRayHit, groundCastDistance,groundLayers,QueryTriggerInteraction.Ignore);
        if (rayHit)
        {
            Vector3 otherVel = Vector3.zero;

            var vel = _rigidbody.velocity;
            var hitBody = _floatRayHit.rigidbody;
            SetPlatform(_floatRayHit.rigidbody);
            if (hitBody != null)
            {
                otherVel = hitBody.GetPointVelocity(_floatRayHit.point);
            }
            
            //check ray distance for some jump threshold...
            //reset the times jumped counter
            _jumps = 0;
            
            
            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;
            float x = _floatRayHit.distance - RideHeight;

            float springForce = (x * RideSpringForce) - (relVel * RideSpringDamper);
            
            Debug.DrawLine(transform.position,transform.position+(rayDir*springForce)/10f,Color.yellow);
            
            _rigidbody.AddForce(rayDir*springForce);
            if (hitBody != null)
            {
                //push down against the platform
                hitBody.AddForceAtPosition(rayDir * -springForce, _floatRayHit.point);
                
                //now we just need to resolve physics in the orthogonal plane to floating.
                //player has a friction force that resists changes in velocity.
                
                
                //note: with the parenting trick, this kind of breaks everything.
                // var movingVel = hitBody.GetPointVelocity(_floatRayHit.point);
                // if (movingVel.sqrMagnitude > 0.01f)
                // {
                //     float velDot = Vector3.Dot(_rigidbody.velocity, movingVel);
                //
                //     //treat the velocity of the platform as a desired velocity. Maxaccel here basically works like friction.
                //     var neededAccel = (movingVel - _rigidbody.velocity) / Time.fixedDeltaTime;
                //     float maxAccel = maxAccelerationForce*2;//todo set separately.
                //     
                //     neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
                //     var force = (neededAccel * _rigidbody.mass);
                //     force.Scale(forceScale);
                //  //   _rigidbody.AddForce(force);
                //
                //     //todo: add the opposite of our movement force to the platform.
                // }
            }
        }
    }

    private void SetPlatform(Rigidbody rb)
    {
        if (rb == null)
        {
            _platform = null;
            _platformParent.gameObject.SetActive(false);
            transform.SetParent(null);
        }
        else
        {
            _platformParent.gameObject.SetActive(true);
            _platformParent.transform.position = rb.transform.position;
            _platformParent.transform.rotation = rb.transform.rotation;
            transform.SetParent(_platformParent);
        }
    }

    private void UprightForce()
    {
        CalculateTargetRotation();//rotate around y

        var characterCurrent = transform.rotation;
        var toGoal = Utility.ShortestRotation(_uprightTargetRot, characterCurrent);
        toGoal.ToAngleAxis(out var rotDegrees, out var rotAxis);
        rotAxis.Normalize();
        float rotRadians = rotDegrees * Mathf.Deg2Rad;
        _rigidbody.AddTorque((rotAxis * (rotRadians * UprightJointSpringStrength)) - (_rigidbody.angularVelocity * UprightJointSpringDamper));
    }

    /// <summary>
    /// Recalculate the upright target axis to account for facing direction. IE: spin player.
    /// </summary>
    private void CalculateTargetRotation()
    {
        //todo: tilt towards input movement.
        if (_unitGoal.sqrMagnitude > 0.01f)
        {
            _uprightTargetRot = Quaternion.LookRotation(_unitGoal, Vector3.up);
        }
        else
        {
            //check if we have a velocity. If we do, we should turn to face the direction we are, say, being flung
        }
    }

}
