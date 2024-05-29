using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour, IInteractable
{
    public Animator animator;

    [SerializeField] private string _prompt;

    int DoorOpenedHash;
    int DoorClosedHash;
    KeyHolder keyHolder;
    public Collider m_ObjectCollider;

    //KeyPickup keyPickup;
    //Inventory inventory;
    public string InteractionPrompt => _prompt;
    public AudioClip soundEffectDoor;

    void Awake()
    {
        DoorOpenedHash = Animator.StringToHash("DoorOpen");
        DoorClosedHash = Animator.StringToHash("DoorClosed");
        keyHolder = FindObjectOfType<KeyHolder>();
    }

    public bool Interact(Interactor interactor)
    {

        bool DoorOpen = animator.GetBool(DoorOpenedHash);
        bool DoorClosed = animator.GetBool(DoorClosedHash);

        //Walking Controls-------------------------------

        var keyPickup = interactor.GetComponent<KeyPickup>();
        m_ObjectCollider = GetComponent<Collider>();

        // if (keyPickup.keyAmount == null)
        //{
        //  return false;

        // Debug.Log("REMOVE KEY");
        //}


        if (keyHolder.keyAmount <= 0)
        {
            Debug.Log("nokeyforyou");
            m_ObjectCollider.isTrigger = false;
        }


         else if (keyHolder.keyAmount >= 1)
        {
            Debug.Log("OpeningDoor");
            animator.SetBool(DoorOpenedHash, true);

            AudioSource.PlayClipAtPoint(soundEffectDoor, transform.position);

            keyHolder.keyAmount --;
            m_ObjectCollider.isTrigger = true;

         }
        ///else if (keyPickup.keyAmount == null)
       // {
           

            Debug.Log("NoKeyFound");

      //  }


        return false;
    }









}




