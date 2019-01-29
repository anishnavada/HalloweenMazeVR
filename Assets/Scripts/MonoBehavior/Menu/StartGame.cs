using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	public void LoadByIndex(int sIndex){
        GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[0].enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[1].Play();
        StartCoroutine(loadScene(sIndex));
	}


    public IEnumerator loadScene(int sIndex)
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sIndex);
    }



}

