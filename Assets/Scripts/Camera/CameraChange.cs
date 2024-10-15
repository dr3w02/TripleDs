using Cinemachine;
using Platformer;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows.WebCam;


public class CameraChange : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    public BoxCollider box;

    public bool invertPlayerControls;

    public float enterPlayer = 1f;
    public float invertDelay = 0.2f;

    public float exitPlayer = 1f;
    public float returnInvert = 0.2f;

    //what room are we in?
    public string room;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RBController playerRb = other.GetComponent<RBController>();
            if (playerRb.rb != null)
            {
                if (CameraManager.cameraInstance.ActiveCamera != cam)
                {
                    //Debug.Log("SwitchCamera" + cam);
                    CameraManager.cameraInstance.SwitchCamera(cam);
                }

                HandleLightChange();

                if (invertPlayerControls)
                    StartCoroutine(EnterPlayer(playerRb));
                else
                    StartCoroutine(ExitPlayer(playerRb));
            }

        }
    }

    void HandleLightChange()
    {
        CameraManager c = CameraManager.cameraInstance;
        c.TurnAllTheLightsOff();

        switch (room)
        {
            case "attic":

               c.atticLights.SetActive(true);
                //if there are other rooms you want to leave lights on add them here

                break;


            case "bathroom":

                c.bathroomLights.SetActive(true);

                break;

            case "hallways":

                c.hallwaysLights.SetActive(true);

                break;

            case "nurse":

                c.nurseLights.SetActive(true);

                break;

            case "classroom":

                c.classroomLights.SetActive(true);

                break;

            case "library":

                c.libraryLights.SetActive(true);

                break;

            case "vent":

                c.ventLights.SetActive(true);

                break;

            case "kitchen":

                c.kitchenLights.SetActive(true);

                break;

                //add cases
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RBController playerRb = other.GetComponent<RBController>();
            if (playerRb.rb != null)
            {
                if (CameraManager.cameraInstance.ActiveCamera != cam)
                {
                    CameraManager.cameraInstance.SwitchCamera(cam);
                }

                if (invertPlayerControls)
                    StartCoroutine(EnterPlayer(playerRb));
                else
                    StartCoroutine(ExitPlayer(playerRb));
            }
        }
    }
    private IEnumerator EnterPlayer(RBController playerRb)
    {
        yield return new WaitForSeconds(enterPlayer);
        StartCoroutine(InvertControls(playerRb));

    }

    private IEnumerator InvertControls(RBController playerRb)
    {
        yield return new WaitForSeconds(invertDelay);
        playerRb.invertInputs = true;

    }

    private IEnumerator ExitPlayer(RBController playerRb)
    {
        yield return new WaitForSeconds(exitPlayer);
        StartCoroutine(ReturnInvert(playerRb));
    }

    private IEnumerator ReturnInvert(RBController playerRb)
    {
        yield return new WaitForSeconds(returnInvert);
        playerRb.invertInputs = false;
    }
}