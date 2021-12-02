using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Teleporter : MonoBehaviour
{
    public VrMovementController vrMovementController;
    public Transform groundRef;
    public float groundThreshold = .5f;
    MeshRenderer meshRenderer;
    bool canTeleport;
    bool teleportTimeFlag;
    // Start is called before the first frame update
    void Start()
    {
        canTeleport = false;
        teleportTimeFlag = true;
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public async void TryTeleport()
    {
        if (canTeleport && teleportTimeFlag)
        {
            teleportTimeFlag = false;
            vrMovementController.Teleport(transform.position);
        }
        await Task.Delay(200);
        teleportTimeFlag = true;
    }

    void Update()
    {
        if (Mathf.Abs(groundRef.position.y - transform.position.y) < groundThreshold)
        {
            meshRenderer.enabled = true;
            canTeleport = true;
        }
        else
        {
            meshRenderer.enabled = false;
            canTeleport = false;
        }
    }
}
