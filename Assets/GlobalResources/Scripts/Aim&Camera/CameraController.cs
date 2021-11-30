using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera mainViewCamera;
    public CinemachineVirtualCamera vrViewCamera;

    public enum ViewType
    {
        MainView,
        VrView
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchView(ViewType viewType)
    {
        //Debug.LogWarning("Switching view");
        switch (viewType)
        {
            case ViewType.VrView:
                mainViewCamera.Priority = 10;
                vrViewCamera.Priority = 11;
                break;
            default:
                vrViewCamera.Priority = 10;
                mainViewCamera.Priority = 11;
                break;
        }
    }
}
