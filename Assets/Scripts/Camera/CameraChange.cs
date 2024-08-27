using Cinemachine;
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
    [SerializeField] public CameraManager cameraManager;

   
    public BoxCollider box;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();

        box.isTrigger = true;
        
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            // playerRb.isKinematic = true;
           
            if (playerRb != null)
            {
                if (CameraManager.cameraInstance.ActiveCamera != cam)
                {
                    Debug.Log("SwitchCamera" + cam);
                    CameraManager.cameraInstance.SwitchCamera(cam);

                }
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        CameraManager.cameraInstance.ResetCamera(cam);
    }



}
