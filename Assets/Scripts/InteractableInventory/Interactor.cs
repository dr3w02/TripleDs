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

    

    private readonly Collider[] _colliders = new Collider[3];

    [SerializeField] private int _numFound;
    List<Collider> interactableItems = new List<Collider>();

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders,_interactableMask);

        if (_numFound > 0)
        {
            Debug.Log(_colliders[0].name + " is the collider ");
            var interactable = _colliders[0].gameObject.GetComponent<IInteractable>();

            if(interactable != null && Keyboard.current.eKey.wasPressedThisFrame) // how its done with new input system
            {
                Debug.Log("Interacting");
                interactable.Interact(this);

            }



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


