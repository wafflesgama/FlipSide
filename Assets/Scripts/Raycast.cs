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

    private Component col;
    public GameObject pessoas;
    public GameObject carros;

    public GameObject mike;

    public GameObject fog;

    public GameObject sat;

    public Volume volume;
    ColorAdjustments c;

    public Animator mike_anim;

    public float speed_cor;

    public AudioSource fundo;

    public SerialController serialController;



    // Start is called before the first frame update
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnTriggerEnter(Collider other)
    {

        pessoas.SetActive(false);
        carros.SetActive(false);
        mike.SetActive(true);

        fundo.volume = Mathf.Lerp(fundo.volume, 0f, speed_cor);

        mike_anim.SetBool("mike", true);

        serialController.SendSerialMessage("A");

        if (other.CompareTag("Player"))
        {
            raycast.GetComponent<Raycast>().enabled = true;

        }

        if (volume.profile.TryGet<ColorAdjustments>(out c))
        {
            //c.saturation.value = Cor;
            c.saturation.value = Mathf.Lerp(c.saturation.value, -100, speed_cor);
        }
    }
}
