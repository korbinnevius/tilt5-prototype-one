using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public Action<Interactable> OnDestroyed;
    //Tracks players interacting with it.
    public Action OnInteractStart;
    public Action OnInteractStop;

    public bool CanBeDirectInteracted => _canBeDirectInteracted;
    
    
    [Header("Interaction Configuration")] 
    [SerializeField] private bool _canBeDirectInteracted;//can you interact with this by pressing a button?
    public UnityEvent OnInteractionStartEvent;
    public UnityEvent OnInteractionStopEvent;

    public bool Interacting => _interacting;
    private bool _interacting;

    public bool CanInteract => _canInteract;
    public string Verb;

    private bool _canInteract = true;

    [SerializeField] private Transform overrideUIPosition;
    
    public void SetInteracting(bool interacting)
    {
        if (!_canInteract)
        {
            Debug.LogWarning($"Tried to interact with {gameObject.name} but not interactable");
        }
        if (interacting != _interacting)
        {
            _interacting = interacting;
            if (_interacting)
            {
                StartInteraction();
            }
            else
            {
                StartInteraction();
            }
        }
    }

    private void StartInteraction()
    {
        OnInteractStart?.Invoke();
        OnInteractionStartEvent.Invoke();
    }

    private void StopInteraction()
    {
        OnInteractStop?.Invoke();
        OnInteractionStopEvent.Invoke();
    }

    //player pressed the button.
    public void DirectInteract()
    {
        if (_canInteract && _canBeDirectInteracted)
        {
            StartInteraction();
            StopInteraction();
        }
    }
    
    public void SetInteractable(bool interactable)
    {
        _canInteract = interactable;
    }

    protected virtual void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public virtual Vector3 GetWorldUIPosition()
    {
        return overrideUIPosition == null ? transform.position : overrideUIPosition.position;
    }
}
