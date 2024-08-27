using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraRegister : MonoBehaviour
{
    private void Start()
    {
        CameraManager.cameraInstance.Register(GetComponent<CinemachineVirtualCamera>());
    }

}
