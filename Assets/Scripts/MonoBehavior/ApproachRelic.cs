using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApproachRelic : MonoBehaviour {

    [SerializeField]
    new Transform camera = null;

    public float distortionDistance = 10;
    private float distance = 20;
    private bool relicActive = true;
    private bool lookingAt = false;

    public static int relicCounter = 0;
    public Text counter;

	// Use this for initialization
	void Start () {
        relicCounter = GameObject.FindGameObjectsWithTag("Relic").Length - 1;
        

    }
	
	// Update is called once per frame
	void Update () {
        if (relicActive && lookingAt)
            ZoomIn();
       // if (!relicActive && camera.GetComponent<Camera>().fieldOfView < 59)
       //     camera.GetComponent<Camera>().fieldOfView += 1;
    }

    void WinGame()
    {
        GameObject pumpkin = GameObject.FindGameObjectWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        pumpkin.transform.position = transform.position + Vector3.up;
        pumpkin.GetComponentInChildren<ChaseCharacter>().speed = 0;
        pumpkin.transform.GetChild(0).gameObject.SetActive(true);
        pumpkin.transform.GetChild(1).gameObject.SetActive(true);
        pumpkin.transform.LookAt(player.transform);
        pumpkin.GetComponent<ChaseCharacter>().killer = true;
        camera.GetComponent<AudioSource>().PlayDelayed(3);
        pumpkin.GetComponent<AudioSource>().PlayDelayed(3);

        camera.GetComponentInChildren<Animator>().SetTrigger("Win");
        StartCoroutine(ResetGame());
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(0);
    }

    public void SetLookAt(bool val)
    {
        lookingAt = val;
        if(!val)
            camera.GetComponent<Camera>().fieldOfView = 60;
    }

    public void ZoomIn()
    {
        Debug.Log(Vector3.Distance(transform.position, camera.position));
        bool inRange = distortionDistance > Vector3.Distance(transform.position, camera.position);
        if (inRange)
        {
            camera.GetComponent<Camera>().fieldOfView-= 0.75f;
            Debug.Log("zoom!");
        }
        if (inRange && camera.GetComponent<Camera>().fieldOfView < 5)
            ActivateRelic();
    }


    public void ActivateRelic()
    {
        relicActive = false;
        relicCounter += 1;
        transform.Find("Spotlight").GetComponent<Light>().enabled = false;
#pragma warning disable CS0618 // Type or member is obsolete
        transform.Find("Idol").Find("Eye_L").GetComponent<ParticleSystem>().enableEmission = false;
        transform.Find("Idol").Find("Eye_R").GetComponent<ParticleSystem>().enableEmission = false;
#pragma warning restore CS0618 // Type or member is obsolete
        transform.GetComponents<AudioSource>()[0].enabled = false;
        transform.GetComponents<AudioSource>()[1].Play();
        camera.GetComponent<ScoreTracker>().DecreaseScore();
        Destroy(transform.Find("Idol").gameObject);
        SetLookAt(false);
        if (ScoreTracker.remainingScore <= 0)
            WinGame();
    }

}
