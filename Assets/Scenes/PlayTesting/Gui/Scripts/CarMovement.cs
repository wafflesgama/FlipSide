using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarMovement : MonoBehaviour
{
    public LayerMask layerMask;
    public float moveItensity = 25;
    public float minRandomIntensity = 25;
    public float maxRandomIntensity = 25;
    public int frontCastDistance = 5;
    public int sideCastDistance = 5;
    public float breakIntensity = 10;
    public float minBreakDistance = 4;
    public float obsctacleDistance;

    public Transform[] tires;

    public AudioClip[] honkSounds;
    public float pitchVariation;
    public int honkThreshold;
    public float tireSpeed;

    public MeshRenderer meshRenderer;
    public MeshRenderer altMesh;
    public int[] colorMaterialIndexes;
    public int altMatIndex;
    public Color[] colorPalete;
    public bool wheelRotateInY;

    Rigidbody body;
    AudioSource audioSource;
    int honkCounter;

    private void Awake()
    {
        StartCoroutine(StartEngine());


        if (altMesh != null)
            altMesh.materials[altMatIndex].color = colorPalete[Random.Range(0, colorPalete.Length)];

        foreach (var colorMaterialIndex in colorMaterialIndexes)
            meshRenderer.materials[colorMaterialIndex].color = colorPalete[Random.Range(0, colorPalete.Length)];
    }

    IEnumerator StartEngine()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch += Random.Range(-pitchVariation, pitchVariation);

        yield return new WaitForSeconds(Random.Range(0, 1.0f));
        audioSource.Play();
    }

    void Start()
    {
        honkCounter = 0;
        body = GetComponent<Rigidbody>();
        moveItensity += Random.Range(minRandomIntensity, maxRandomIntensity);



    }

    private void Update()
    {
        foreach (var tire in tires)
        {
            tire.localEulerAngles += (wheelRotateInY ? Vector3.down :Vector3.back) * tireSpeed * body.velocity.magnitude * Time.deltaTime * (Vector3.Dot(body.velocity, transform.forward) >= 0 ? 1 : -1);
        }
    }

    private void FixedUpdate()
    {
        CheckObstacles();
        MoveCar();

        if (honkCounter > honkThreshold)
            Honk();
    }

    void Honk()
    {
        honkCounter = 0;
        audioSource.PlayOneShot(honkSounds[Random.Range(0, honkSounds.Length - 1)]);
    }

    void CheckObstacles()
    {
        var isHitFront = Physics.Raycast(transform.position, transform.forward, out RaycastHit hitFront, frontCastDistance, layerMask);

        bool isHitRight = false, isHitLeft = false;
        RaycastHit hitRight = new RaycastHit(), hitLeft = new RaycastHit();

        for (int i = 4; i < 8; i++)
        {
            //Debug.DrawRay(transform.position, (transform.forward * i + transform.up), Color.red);
            isHitRight = Physics.Raycast(transform.position, (transform.forward * i + transform.up), out hitRight, sideCastDistance, layerMask);
            if (isHitRight) break;
        }

        for (int i = 4; i < 8; i++)
        {
            //Debug.DrawRay(transform.position, (transform.forward * i - transform.up), Color.cyan);
            isHitLeft = Physics.Raycast(transform.position, (transform.forward * i - transform.up), out hitLeft, sideCastDistance, layerMask);
            if (isHitLeft) break;
        }


        var hitDistance = isHitFront ? hitFront.distance : float.MaxValue;
        hitDistance = isHitRight ? hitRight.distance < hitDistance ? hitRight.distance : hitDistance : hitDistance;
        hitDistance = isHitLeft ? hitLeft.distance < hitDistance ? hitLeft.distance : hitDistance : hitDistance;
        obsctacleDistance = hitDistance;

        if (isHitFront)
            Debug.DrawLine(transform.position, hitFront.point, Color.blue);

        if (isHitRight)
            Debug.DrawLine(transform.position, hitRight.point, Color.green);

        if (isHitLeft)
            Debug.DrawLine(transform.position, hitLeft.point, Color.green);
    }

    void MoveCar()
    {
        if (obsctacleDistance < minBreakDistance + minBreakDistance && Vector3.Dot(body.velocity, transform.forward) >= 0)
        {
            honkCounter++;

            if (body.velocity.sqrMagnitude > 0.05)
            {
                body.AddForce(-transform.forward * breakIntensity);
            }
            else
                body.velocity = Vector3.zero;
        }
        else
        {
            body.AddForce(transform.forward * moveItensity);
        }

        //body.velocity = Vector3.Scale(body.velocity, transform.forward.normalized);
    }
}
