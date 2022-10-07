using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighscoreSystem : MonoBehaviour
{
    public WaveSystem ws;

    int highscore = 0;

    private void Start()
    {
        highscore = ES3.Load<int>("Highscore");
    }

    private void Update()
    {
        if ( ws.wave > highscore )
        {  
            highscore = ws.wave;

            ES3.Save("Highscore", ws.wave);
        }
    }
}
