using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour {

    public static int remainingScore = 0;


    // Use this for initialization
    void Start () {
        remainingScore = GameObject.FindGameObjectsWithTag("Relic").Length - 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DecreaseScore()
    {
        remainingScore-=1;
        GameObject.Find("ScoreText").GetComponent<Text>().text = remainingScore + "";
        GetComponentInChildren<Animator>().SetTrigger("DisplayScore");
        if (remainingScore <= 0)
            Debug.Log("Winner!");
    }
}
