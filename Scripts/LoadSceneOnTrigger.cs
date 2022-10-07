using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
    public string scene;
    public MeshRenderer whiteScreen;

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        whiteScreen.material.DOFade(255, 5);

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(scene);
    }
}
