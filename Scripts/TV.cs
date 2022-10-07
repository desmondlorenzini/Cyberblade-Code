using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TV : MonoBehaviour
{
    public Text waveTxt;
    public WaveSystem ws;

    void Update()
    {
        waveTxt.text = "Wave \n " + ws.wave.ToString();
    }
}
