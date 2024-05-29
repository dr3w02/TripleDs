using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;

public class RoomCameraController : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera virtualCamera;


    [SerializeField] float cameraDollySpeed = 3f;

    CinemachineTrackedDolly trackedDolly;

    public Transform[] tWaypoints;

    private void Update()
    {

        if(trackedDolly == null)

        {
            trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }

        float horizontal = Input.GetAxisRaw("Horizontal");

        float position = trackedDolly.m_PathPosition;

        position += horizontal * cameraDollySpeed * Time.deltaTime;

        position = Mathf.Clamp(position, 0, trackedDolly.m_Path.MaxPos);

        trackedDolly.m_PathPosition = position;



    }
}
