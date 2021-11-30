using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ChangeView();

public class AimController : MonoBehaviour
{

    public event ChangeView OnVrEnabled;
    public event ChangeView OnVrDisabled;

    public InputManager inputManager;
    public ObjectLockController lockController;
    public CameraController cameraController;
    public Transform aimTarget;
    public float xSensitivity = 1;
    public float ySensitivity = 1;


    public bool isInVr;

    void Start()
    {
        //cameraController.SwitchView(CameraController.ViewType.VrView);
        //isInVr = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.lockState== CursorLockMode.Locked? CursorLockMode.None : CursorLockMode.Locked;
        }

        if (isInVr)
        {
            aimTarget.forward= Camera.main.transform.forward;
        }
        else
        {

            // var verticalRotation=Mathf.Lerp(lastAimValue,-inputManager.input_look.value.y,Time.deltaTime*.5f);
            // Debug.LogWarning("Input val  X:"  + inputManager.input_look.value.x + " Y:"+ inputManager.input_look.value.y);
            var newRotation = Quaternion.Euler(aimTarget.eulerAngles.x + (inputManager.input_look.value.y * ySensitivity * -.08f),
                                                aimTarget.eulerAngles.y + (inputManager.input_look.value.x * xSensitivity * .08f),
                                                aimTarget.eulerAngles.z);

            // Debug.Log("Rotation is X:" + newRotation.eulerAngles.x +
            //                                             " Y:" + newRotation.eulerAngles.y +
            //                                               " Z:" + newRotation.eulerAngles.z);

            aimTarget.rotation = newRotation;
        }
        // aimTarget.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * .5f);

        // lastAimValue= inputManager.input_look.value.y;
    }
}
