using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Arduino : MonoBehaviour
{


   SerialPort sp = new SerialPort("COM4", 9600);
    float next_time; int ii = 0;
    // Use this for initialization
    void Start () {
        next_time = Time.time;
        

        if (!sp.IsOpen)
        {
            print("Opening " + ", baud 9600");
            sp.Open();
            sp.ReadTimeout = 100;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen) { print("Open"); }
        }
    }
    // Update is called once per frame
    void Update() {
        if (Time.time > next_time) { 
            if (!sp.IsOpen)
            {
                sp.Open();
                print("opened sp");
            }
            if (sp.IsOpen)
            {
                print("Writing " + ii);
                sp.Write((ii.ToString()));
            }
            next_time = Time.time + 5;
            if (++ii > 1) {
                ii = 0;
                };
        }
    }
}
