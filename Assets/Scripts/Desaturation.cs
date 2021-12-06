using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Desaturation : MonoBehaviour
{
    public float saturatedValue=50;
    public float deStaturatedValue=-70;
    public float lerpSpeed=2;
    Volume volume;
    ColorAdjustments c;
    public int action=0;

    
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet<ColorAdjustments>(out c);
        Saturate();
    }

    private void Update()
    {
        if (action != 0)
        {
            c.saturation.value = Mathf.Lerp(c.saturation.value, (action == 1 ? saturatedValue : deStaturatedValue),Time.deltaTime* lerpSpeed);
            
            if (action==1 && c.saturation.value >= saturatedValue-1 || action == -1 && c.saturation.value <= deStaturatedValue+1)
                action = 0;
        }
    }

    public void Desaturate()
    {
        action = -1;
    }
    public void Saturate() => action = 1;
    
}
