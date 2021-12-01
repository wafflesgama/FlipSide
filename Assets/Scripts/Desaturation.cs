using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Desaturation : MonoBehaviour
{
    public Volume volume;
    ColorAdjustments c;

    public float Cor;

    
    void Start()
    {
        //volume = gameobject.GetComponent<Volume>();
        //volume.profile.TryGet(out c);

    }

    // Update is called once per frame
    void Update()
    {
         if (volume.profile.TryGet<ColorAdjustments>(out c))
                {
                     c.saturation.value = Cor;
                }
       

    }
}
