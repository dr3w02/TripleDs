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

    public bool interactable;

    public CinemachineVirtualCamera BookCam;

    public bool Interact(Interactor interactor)
    {

        Debug.Log("OpeningBook");

        if (interactable == false)
        {
            //cameraManager.SwitchCamera(cameraManager.BookCam);

            BookCam.Priority = 11;

            characterMove.TurnOffMovement(); Debug.Log("MovementDisabled");

            interactable = true;
        }
        else
        {
            //cameraManager.SwitchCamera(cameraManager.mainCam);
            BookCam.Priority = 0;
            characterMove.Enabled(); Debug.Log("Movementenabled");

            interactable = false;
        }

        return true;

    }

}

   