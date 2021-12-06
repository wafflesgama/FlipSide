using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Raycast : MonoBehaviour
{

    RaycastHit hit;
    Ray ray;

    public GameObject raycast;
    public GameObject bolha;

    public GameObject pessoas;
    public GameObject carros;

    public GameObject mike;

    public GameObject fog;

    public GameObject sat;

    public Desaturation desaturation;

    public Animator mike_anim;

    public float speed_cor;

    public AudioSource fundo;

    public SerialController serialController;



    void Start()
    {
        serialController = GetComponent<SerialController>();
        serialController.SendSerialMessage("D");
    }

    // Update is called once per frame
    void Update()
    {


    }
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        pessoas.SetActive(false);
        carros.SetActive(false);
        mike.SetActive(true);
        desaturation.Desaturate();

        mike_anim.SetBool("mike", true);

        serialController.SendSerialMessage("L");

    }
}
