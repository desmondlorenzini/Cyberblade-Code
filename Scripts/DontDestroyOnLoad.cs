using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    public string id;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        foreach ( DontDestroyOnLoad item in FindObjectsOfType(typeof(DontDestroyOnLoad)) )
        {
            if ( item.id == this.id && item != this )
            {
                Destroy(item.gameObject);
            }
        }
    }

}
