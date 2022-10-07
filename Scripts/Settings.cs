using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private void Start()
    {
        if ( ES3.KeyExists("Texture Quality") )
        {
            QualitySettings.masterTextureLimit = ES3.Load<int>("Texture Quality");
        }

        if ( !ES3.KeyExists("Snap Turn") )
        {
            ES3.Save("Snap Turn", true);
        }

        if ( !ES3.KeyExists("Controls Inverted") )
        {
            ES3.Save("Controls Inverted", false);
        }
    }
    public void SetTextureQuality(int quality)
    {
        QualitySettings.masterTextureLimit = quality;
           
        ES3.Save("Texture Quality", quality);
    }

    public void SetTurnControl(bool b)
    {
        ES3.Save("Snap Turn", b);
    }

    public void ChangeControls(bool inverted)
    {
        ES3.Save("Controls Inverted", inverted);
    }
}
