using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaseCharacter : MonoBehaviour {

    GameObject playerObject = null;

    public int speed = 1;

    Vector3 me;
    Vector3 player;

    Pathfinder finder = new Pathfinder(GenerateWalls.wallCellGroup);
    List<WallGroup> shortestPath;

    WallGroup currentTarget = new WallGroup(0, 0);
    int[] previousPlayerPos = new int[] { 1, 1 };

    public bool attacking = false, killer = false;
    [SerializeField]
    Transform smoke = null;

    int randomTime = 1;
    float timer = 0;

    

    // Use this for initialization
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform.position;
        me = transform.position;
        shortestPath = finder.GetShortestPath(ConvertVector3(player), ConvertVector3(me));
    }

    // Update is called once per frame
    void Update() {
        if (timer > randomTime)
        {
            GetComponents<AudioSource>()[1].Play();
            timer = 0;
            randomTime = UnityEngine.Random.Range(15, 30);
        }
        else
            timer += Time.deltaTime;
        player = playerObject.transform.position;
        me = transform.position;
        if (!(previousPlayerPos[0] == ConvertVector3(player)[0] && previousPlayerPos[1] == ConvertVector3(player)[1]))
        {
            UpdatePath();
        }
        if (shortestPath != null && shortestPath.Count > 1)
        {
            FollowCharacter();
        }
        else
        {
            attack();
        }
    }

    public void attack()
    {
        if (!attacking)
        {
            attacking = true;
            GetComponents<AudioSource>()[0].Play();
        }
        transform.LookAt(new Vector3(player.x, me.y, player.z));
        transform.position += transform.forward * speed * Time.deltaTime * 3f;
        if (Vector3.Distance(player, me) < 2 && !killer)
        {
            playerObject.GetComponent<Animator>().SetTrigger("Death");
            GetComponents<AudioSource>()[0].pitch = 0.6f;
            GetComponents<AudioSource>()[0].PlayDelayed(2);
            killer = true;
            StartCoroutine(ResetGame());
        }

    }

    public void FollowCharacter()
    {
        attacking = false;
        currentTarget = shortestPath[0];
        if (GetDistFromWallGroup(me, currentTarget) < 2)
            UpdatePath();
        else
        {
            Vector3 targetVector = ConvertIntArr(currentTarget);
            transform.LookAt(new Vector3(targetVector.x, me.y, targetVector.z));
            transform.position += transform.forward * speed * Time.deltaTime * (1 + shortestPath.Count * 0.1f);
        }
    }

    int[] ConvertVector3(Vector3 pose)
    {
        int[] coords = new int[2];
        coords[1] = (int)((pose.z / Mathf.Cos(Mathf.PI / 6) + HexMetrics.innerRadius) / (2 * HexMetrics.innerRadius));
        coords[0] = (int)((pose.x + (coords[1] % 2 == 0 ? HexMetrics.innerRadius : 0)) / (2 * HexMetrics.innerRadius));
        coords[0] -= (coords[1] / 2);
        return coords;
    }
    Vector3 ConvertIntArr(WallGroup group)
    {
        int[] arr = new int[] { group.x, group.y };
        Vector3 end = new Vector3();
        end.x = (arr[0] + arr[1] / 2) * 2 * HexMetrics.innerRadius + (arr[1] % 2 == 0 ? 0 : HexMetrics.innerRadius);
        end.z = arr[1] * 1.5f * HexMetrics.outerRadius;
        return end;
    }

    public void Dissapear()
    {
        if (killer)
            return;
        gameObject.name = "pumpkin";
        GridPlacer placer = new GridPlacer(GenerateWalls.coordinates);
        placer.PlaceDistanceAway(finder, ConvertVector3(player), transform, 5);
        GameObject.Instantiate(smoke, transform.position, transform.rotation);
        GameObject.Destroy(gameObject);
    }

    float GetDistFromWallGroup(Vector3 one, WallGroup two)
    {
        Vector3 pos2 = ConvertIntArr(two);
        return Vector3.Distance(one, pos2);
    }

    void UpdatePath()
    {
        bool potentialError = false;
        try
        {
            shortestPath = finder.GetShortestPath(ConvertVector3(player), ConvertVector3(me));
        }
        catch (IndexOutOfRangeException)
        {
            potentialError = true;
        }
        previousPlayerPos = ConvertVector3(player);
        Debug.Log(ConvertVector3(me)[0] + ", " + ConvertVector3(me)[1]);

        if (shortestPath == null || shortestPath.Count <= 1|| potentialError)
        {
            List<WallGroup> temp = finder.GetShortestPath(ConvertVector3(me), ConvertVector3(player));
            // Inexplicable flaw in algorithm; only works in 1 direction.
            if (temp != null && temp.Count > 2)
            {
                temp.Reverse();
                shortestPath = temp;
            }
        }
        if(shortestPath[0].x == ConvertVector3(me)[0] && shortestPath[0].y == ConvertVector3(me)[1])
            shortestPath.RemoveAt(0);
    }


    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene(0);
    }
}

