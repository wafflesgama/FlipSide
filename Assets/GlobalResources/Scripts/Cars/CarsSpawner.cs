using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsSpawner : MonoBehaviour
{
    public GameObject[] carTypes;
    public float spawnRate;
    public LayerMask layerMask;
    public Vector3 offSetPos;
    void Start()
    {
        StartCoroutine(SpawnCar());
    }

    IEnumerator SpawnCar()
    {
        var colliders = Physics.OverlapBox(transform.position, transform.localScale,transform.rotation, layerMask);
        Debug.Log($"Num colliders {colliders.Length}");
        if (colliders.Length == 0)
        {
            GameObject.Instantiate(carTypes[Random.Range(0, carTypes.Length)],transform.position, transform.rotation);
            yield return new WaitForSeconds(spawnRate / 2);
        }
        yield return new WaitForSeconds(spawnRate / 2);
        StartCoroutine(SpawnCar()); 
    }
}
