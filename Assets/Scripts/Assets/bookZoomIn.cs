using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class bookZoomIn : MonoBehaviour, IInteractable
{
    [SerializeField] characterMovement characterMove;
    public string InteractionPrompt => "LookAtBook";

    public GameObject InteractionImagePrompt => BookPromptCanvas;

    //public GameObject character;
    public GameObject BookCam;
    public GameObject BookPromptCanvas;

    public bool isZoomedIn;

    private void OnValidate()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Interactable"))
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            gameObject.AddComponent<BoxCollider>();
        }

    }

    private void Start()
    {
        BookCam.SetActive(false);
    }

    public bool Interact(Interactor interactor)
    {
        isZoomedIn = !isZoomedIn;

        if (!isZoomedIn)
        {
            ZoomInOnBook();
            return true;
        }
        else
        {
            ZoomOut();
            return true;
        }
    }


    private void ZoomInOnBook()
    {
        //isZoomedIn = true;
        BookCam.SetActive(true);

        characterMove.TurnOffMovement();
        //change this to can move is false

    }

    private void ZoomOut()
    {
        BookCam.SetActive(false);

        characterMove.Enabled();
    }
}

