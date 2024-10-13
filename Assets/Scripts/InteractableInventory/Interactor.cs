using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/*
interface IInteractable
{
    public void Interact();

}
*/

public class Interactor : MonoBehaviour
{
    
    //public Transform InteractorSource;
    //public float InteractRange;
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;

    public IInteractable currentInteractable;

    private readonly Collider[] _colliders = new Collider[3];

    [SerializeField] private int _numFound;
    List<Collider> interactableItems = new List<Collider>();

    bool currentlyInteracting = false;

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders,_interactableMask);
        Debug.Log(currentInteractable);

        if (_numFound > 0)
        {
            //Debug.Log(_colliders[0].name + " is the collider ");
            var interactable = _colliders[0].gameObject.GetComponent<IInteractable>();

            if(interactable != null) // how its done with new input system
            {
                currentInteractable = interactable;

                if(!currentlyInteracting && currentInteractable.InteractionImagePrompt != null)
                    currentInteractable.InteractionImagePrompt.SetActive(true);

                if (Keyboard.current.eKey.wasPressedThisFrame)
                {

                   
                    Debug.Log("Interacting");

                    currentlyInteracting = true;
                    
                    if(currentInteractable.InteractionImagePrompt != null)
                        currentInteractable.InteractionImagePrompt.SetActive(false);

                    currentInteractable.Interact(this);

                }
            }
            else
            {
         
                if (currentInteractable != null && currentInteractable.InteractionImagePrompt != null)
                    currentInteractable.InteractionImagePrompt.SetActive(false);

                currentlyInteracting = false;
                currentInteractable = null;
            }
        }
        else
        {
            if (currentInteractable != null && currentInteractable.InteractionImagePrompt != null)
                currentInteractable.InteractionImagePrompt.SetActive(false);

            currentlyInteracting = false;
            currentInteractable = null;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position,_interactionPointRadius);

    }


    /*


    void Update()
    {
       if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

            if(Physics.Raycast( r, out RaycastHit hitInfo,InteractRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
    */
}


