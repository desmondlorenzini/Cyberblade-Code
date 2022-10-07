using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaveSelect : MonoBehaviour
{
    public List<int> waves = new List<int> {0, 4, 9, 14};
    public Text waveTxt;

    private int currentIndex = 0;
    private List<int> wavesPassed = new List<int> {0};

    private void Start()
    {
        ES3.Save("Spawn Wave", 0);

        if ( ES3.KeyExists("Waves Passed") )
        {
            wavesPassed = ES3.Load<List<int>>("Waves Passed");
        }
    }

    public void Select(int add)
    {
        int nextIndex;

        if ( waves[currentIndex] == (wavesPassed.Max() - 1) )
        {
            GoToStart();

            return;
        }

        if ( currentIndex == 0 && add < 0 )
        {
            GoToEnd();
            
            return;
        }

        nextIndex = currentIndex + add;

        int spawnWave = waves[nextIndex];

        if ( !wavesPassed.Contains(spawnWave) )
        {
            GoToStart();

            return;
        }

        ES3.Save("Spawn Wave", spawnWave);

        waveTxt.text = (spawnWave + 1).ToString();

        currentIndex = nextIndex;
    }

    void GoToStart()
    {
        currentIndex = 0;

        ES3.Save("Spawn Wave", waves[0]);

        waveTxt.text = (waves[0] + 1).ToString();
    }

    void GoToEnd()
    {
        int spawnWave = 0;

        for ( int i = waves.Count - 1; i > 0; i-- )
        {
            int wave = waves[i];

            if ( wavesPassed.Contains(wave) )
            {
                spawnWave = wave;

                currentIndex = i;

                break;
            }
        }

        ES3.Save("Spawn Wave", spawnWave);

        waveTxt.text = (spawnWave + 1).ToString();
    }
}
