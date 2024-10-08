using Cinemachine;
using Platformer;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows.WebCam;


public class CameraChange : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    public BoxCollider box;

    public bool invertPlayerControls;

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

                if (invertPlayerControls)
                    playerRb.invertInputs = true;
                else
                    playerRb.invertInputs = false;
            }

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
                    playerRb.invertInputs = true;
                else
                    playerRb.invertInputs = false;
            }

        }
    }



}
