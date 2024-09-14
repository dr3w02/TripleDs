using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class bookZoomIn : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] CameraManager cameraManager;
    [SerializeField] characterMovement characterMove;
    public string InteractionPrompt => _prompt;

    characterMovement characterMovement;

   
    public GameObject character;
    public GameObject BookCam;



    private bool isZoomedIn = false;


    public bool Interact(Interactor interactor)
    {
        Debug.Log("OpeningBook");

        if (!isZoomedIn)
        {
            ZoomInOnBook();
        }
        else
        {
            ZoomOut();
        }

        return true;
    }


    private void ZoomInOnBook()
    {
        BookCam.SetActive(true);

        characterMove.TurnOffMovement();

        isZoomedIn = true;

        StartCoroutine(TurnOffCharacterAfterDelay());
    }

    private void ZoomOut()
    {
        BookCam.SetActive(false);
        characterMove.Enabled();

        StartCoroutine(TurnOnCharacterAfterDelay());
        isZoomedIn = false;
    }


    private IEnumerator TurnOffCharacterAfterDelay()
    {
        yield return new WaitForSeconds(2f); 
        character.SetActive(false);
    }

    private IEnumerator TurnOnCharacterAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);  
        character.SetActive(true);
    }
}

