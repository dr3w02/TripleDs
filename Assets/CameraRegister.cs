using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraRegister : MonoBehaviour
{
    private void OnEnabled()
    {
        CameraManager.Register(GetComponent<CinemachineVirtualCamera>());

    }

    private void OnDisabled()
    {
        CameraManager.UnRegister(GetComponent<CinemachineVirtualCamera>());
    }
}
