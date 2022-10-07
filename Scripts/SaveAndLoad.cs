using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]  
public class SaveAndLoad : MonoBehaviour
{
    public Text highscoreTxt;

    private void Start()
    {
        if ( ES3.KeyExists("Highscore") )
        {
            highscoreTxt.text = "Highscore \n" + ES3.Load<int>("Highscore");
        }
    }
}
