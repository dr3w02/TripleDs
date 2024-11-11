using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour, IInteractable
{
    public Animator animator;

    [SerializeField] private string _prompt;

    public int DoorOpenedHash;
    public int DoorClosedHash;
    KeyHolder keyHolder;
    public Collider m_ObjectCollider;
    public bool doorisopen;

    //KeyPickup keyPickup;
    //Inventory inventory;
    public string InteractionPrompt => _prompt;
    public GameObject InteractionImagePrompt => null;

    public AudioClip soundEffectDoor;


    public void Awake()
    {
        DoorOpenedHash = Animator.StringToHash("DoorOpen");
        DoorClosedHash = Animator.StringToHash("DoorClosed");
        keyHolder = FindObjectOfType<KeyHolder>();

    }

    public bool Interact(Interactor interactor, GameObject player)
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

            if (!doorisopen)
            {
               
                m_ObjectCollider.isTrigger = false;
            }
            else
            {
                m_ObjectCollider.isTrigger = true;
            }
        
          
        }


         else if (keyHolder.keyAmount >= 1)
        {
            Debug.Log("OpeningDoor");
            animator.SetBool(DoorOpenedHash, true);
            doorisopen = true;
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

    public void Update()
    {

        if (!doorisopen)
        {
            m_ObjectCollider.isTrigger = false;
            animator.SetBool(DoorOpenedHash, false);
        }
        else
        {
            animator.SetBool(DoorOpenedHash, true);
            m_ObjectCollider.isTrigger = true;
        }
        
    }







}




