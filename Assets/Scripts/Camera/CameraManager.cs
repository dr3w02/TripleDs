using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager cameraInstance;

    public List <CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera> ();
    public CinemachineVirtualCamera ActiveCamera = null;

    private void Awake()
    {
        if(cameraInstance == null)
        {
            cameraInstance = this;
        }
    }

    public bool IsActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == ActiveCamera;

    }

    public void SwitchCamera(CinemachineVirtualCamera camera)
    {
        foreach (CinemachineVirtualCamera c in cameras)
        {
           c.Priority = 0;
        }

        camera.Priority = 10;
    }

    public void ResetCamera(CinemachineVirtualCamera camera)
    {
        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c == camera)
            {

                c.Priority = 0;

            }
      
        }
   

    }

        public void Register(CinemachineVirtualCamera camera)
        {

        cameras.Add(camera);
        Debug.Log("Camera Registered:" + camera);


        }

   
}




