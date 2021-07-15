using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;
    public float m_ScreenEdgeBuffer = 4f;
    public float m_MinSize = 6.5f;

    private Camera m_Camera;
    private float m_ZoomSpeed;
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;

    //Cinemachine.CinemachineVirtualCamera;

    private void Awake()
    {
        m_Camera = Camera.main;
    }

    private void Update()
    {
        
    }

    void LateUpdate()
    {

    }
}
