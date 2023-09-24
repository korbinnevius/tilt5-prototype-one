using UnityEngine;
public class PhysicsButton : Interactable
{
    [Header("Button Configuration")]
    private Joint _buttonJoint;
    public float pressDistance = 0.2f;
    private void Awake()
    {
        _buttonJoint = GetComponentInChildren<Joint>();
    }

    private void Start()
    {
    }   

    private void Update()
    {
        var offset = (_buttonJoint.anchor - _buttonJoint.transform.localPosition).magnitude;
        var pressed =  offset >= pressDistance;
        SetInteracting(pressed);
    }
}
