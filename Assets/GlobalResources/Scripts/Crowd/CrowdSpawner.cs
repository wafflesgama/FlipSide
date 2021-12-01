using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    public Transform[] waypointsToFollow;
    public bool upDirection;
    public GameObject personTemplate;
    public float spawnRate;
    public LayerMask layerMask;

    void Start()
    {
        StartCoroutine(SpawnPerson());
    }

    IEnumerator SpawnPerson()
    {
        var colliders = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation, layerMask);
        if (colliders.Length == 0)
        {
            var obj = GameObject.Instantiate(personTemplate, transform.position, transform.rotation);
            obj.transform.parent = transform;
            obj.GetComponent<PersonMovement>().Spawn(upDirection,waypointsToFollow,upDirection? 0: waypointsToFollow.Length-1);
            yield return new WaitForSeconds(spawnRate / 2);
        }
        yield return new WaitForSeconds(spawnRate / 2);
        StartCoroutine(SpawnPerson());
    }
}
