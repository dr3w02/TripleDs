using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyPickup : MonoBehaviour, IInteractable
{
    Inventory inventory;

    [SerializeField] private string _prompt;

    public GameObject Key;

    public bool hasKey;

    //public int keyAmount;
    public KeyHolder keyHolder;
    public string InteractionPrompt => _prompt;

    public GameObject InteractionImagePrompt => null;

    [SerializeField] public AudioClip keyPickupSound;

    public bool Interact(Interactor interactor)
    {

        hasKey = true;


        if (hasKey == true)
        {
            keyHolder.keyAmount ++;

            Key.SetActive(false);

            AudioManager.instance.PlayAudioSFX(keyPickupSound, transform.position);
            Debug.Log("HasKey");
        }

        else
        {
            hasKey = false;

            Debug.Log("NoKey");
        }

        return true;
    

    }
}

