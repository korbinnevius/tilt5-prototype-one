using Interaction;
using UnityEngine;

public class Grabbable : Interactable
{
	public Rigidbody Rigidbody => _rigidbody;
	private Rigidbody _rigidbody;

	public GrabHandler GrabHandler => _grabHandler;
	private GrabHandler _grabHandler;

	private MeshRenderer _meshRenderer;
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_meshRenderer = GetComponentInChildren<MeshRenderer>();
	}
	
	public void Grabbed(GrabHandler grabHandler)
	{
		_grabHandler = grabHandler;
		SetInteracting(true);
	}

	public void Released()
	{
		_grabHandler = null;
		SetInteracting(false);
	}

	protected override void OnDestroy()
	{
		if (_grabHandler != null)
		{
			_grabHandler.ForceRelease();
		}
		base.OnDestroy();
	}

	private void OnJointBreak(float breakForce)
	{
		if (_grabHandler != null)
		{
			_grabHandler.ForceRelease();
		}
	}

	public override Vector3 GetWorldUIPosition()
	{
		return _meshRenderer.bounds.center + new Vector3(0,_meshRenderer.bounds.extents.y+0.2f,0);
	}

	public Vector3 GetAnchorPosition(Vector3 grabPosition)
	{
		var c = GetComponent<Collider>();
		return transform.InverseTransformPoint(c.ClosestPoint(grabPosition));
	}
}
