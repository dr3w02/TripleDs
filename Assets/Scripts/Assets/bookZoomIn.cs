using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class bookZoomIn : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] CameraManager cameraManager;
    [SerializeField] characterMovement characterMove;
    public string InteractionPrompt => _prompt;

    characterMovement characterMovement;

    public bool interactable;
    public GameObject character;
    public GameObject BookCam;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("OpeningBook");

        if (interactable == false)
        {
            BookCam.SetActive(true);

            characterMove.TurnOffMovement();

            StartCoroutine(TurnOffCharacterAfterDelay()); 
        }

        else
        {
            BookCam.SetActive(false);

            StartCoroutine(TurnOnCharacterAfterDelay());
            characterMove.Enabled();
            interactable = false;
        }

        return true;
    }

    private IEnumerator TurnOffCharacterAfterDelay()
    {
        yield return new WaitForSeconds(2f); 
        character.SetActive(false);
    }

    private IEnumerator TurnOnCharacterAfterDelay()
    {
        yield return new WaitForSeconds(1f);  
        character.SetActive(true);
    }
}

