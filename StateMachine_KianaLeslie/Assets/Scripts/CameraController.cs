using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Camera fireAICamera;
    public Camera waterAICamera;
    public Camera birdsEyeCamera;
    bool fireVsWaterView = false;

    Keyboard kb;

    void Start()
    {
        kb = Keyboard.current;
        fireAICamera.enabled = false;
        waterAICamera.enabled = false;
        birdsEyeCamera.enabled = true;
    }

    void Update()
    {
        if (kb.pKey.wasPressedThisFrame)
        {
            ToggleCameras();
        }
        if (kb.oKey.wasPressedThisFrame)
        {
            birdsEyeCamera.enabled = true;
        }
    }

    void ToggleCameras()
    {
        fireVsWaterView = !fireVsWaterView;

        if (fireVsWaterView)
        {
            fireAICamera.enabled = true;
            waterAICamera.enabled = false;
            birdsEyeCamera.enabled = false;
        }
        else
        {
            fireAICamera.enabled = false;
            waterAICamera.enabled = true;
            birdsEyeCamera.enabled = false;
        }
    }
}