using UnityEngine;
using System.Collections.Generic;

public class GenerateWalls : MonoBehaviour {

    // Responsible for carving a maze out of a completed hex grid.
    // Note that wallCellModel is an array of the physical models of the cells,
    // while wallCellGroup holds data pertaining to those walls.
    // Use these arrays or identical ones in pathfinding.

	public int looping = 95;
    [SerializeField]
    Transform relic;
	[SerializeField]
	Transform pumpkin;


	public static GameObject[] wallCellModel;
    public static WallGroup[] wallCellGroup;
	public static int[,] coordinates;
	// Use this for initialization
	void OnEnable() {
		wallCellModel = GameObject.FindGameObjectsWithTag("Cell");
        wallCellGroup = new WallGroup[wallCellModel.Length];
		coordinates = new int[wallCellModel.Length,2];
		for (int i = 0; i < wallCellModel.Length; i++) {
			string[] nums = wallCellModel[i].name.Split(' ');
			coordinates[i,0] = int.Parse (nums[0]);
			coordinates[i,1] = int.Parse (nums[1]);
            wallCellGroup[i] = new WallGroup(coordinates[i, 0], coordinates[i, 1]);
		}
        
            ExploreCell(0, 0);


        GridPlacer placer = new GridPlacer(coordinates);
        Pathfinder finder = new Pathfinder(wallCellGroup);
        for (int i = 0; i < 5; i++)
        {
            placer.PlaceDistanceAway(finder, new int[] { 0, 0 }, relic, 3);
        }

            placer.PlaceDistanceAway(finder, new int[] { 0, 0 }, pumpkin, 5);


       GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScoreTracker>().enabled = true;
       GameObject.Find("HUDCanvas").GetComponent<Animator>().SetTrigger("DisplayScore");
    }



    // Update is called once per frame
    void Update () {

	}


	int FindIndex(int x, int y) {
		for (int i = 0; i < wallCellModel.Length; i++) {
			if(x == coordinates[i,0] && y == coordinates[i,1])
				return i;
		}
		return -1;
	}

	void ClearPath(int x, int y, int side) {
		int pos = FindIndex (x, y);
		int desired = 0;
        int wallBroken = -1;
		GameObject wallPos = null;
		GameObject wallDesired = null;
		if (pos != -1) {
			switch(side) {
			    case 0:
                    desired = FindIndex(x, y + 1);
                    wallPos = wallCellModel[pos].transform.Find("Wall 0").gameObject;
                    wallBroken = (int)WallGroup.Walls.Topright;
				    if(desired >= 0)
					    wallDesired = wallCellModel[desired].transform.Find("Wall 3").gameObject;
				    break;

			    case 1:
				    desired = FindIndex (x+1, y);
				    wallPos = wallCellModel[pos].transform.Find("Wall 1").gameObject;
                    wallBroken = (int)WallGroup.Walls.Right;
                        if (desired >= 0)
					    wallDesired = wallCellModel[desired].transform.Find("Wall 4").gameObject;
				    break;

			    case 2:
			    desired = FindIndex (x+1, y-1);
			    wallPos = wallCellModel[pos].transform.Find("Wall 2").gameObject;
                        wallBroken = (int)WallGroup.Walls.Botright;
                        if (desired >= 0)
				    wallDesired = wallCellModel[desired].transform.Find("Wall 5").gameObject;
			    break;

			    case 3:
				    desired = FindIndex (x, y-1);
				    wallPos = wallCellModel[pos].transform.Find("Wall 3").gameObject;
                    wallBroken = (int)WallGroup.Walls.Botleft;
                    if (desired >= 0)
					    wallDesired = wallCellModel[desired].transform.Find("Wall 0").gameObject;
				    break;

			    case 4:
				    desired = FindIndex (x-1, y);
				    wallPos = wallCellModel[pos].transform.Find("Wall 4").gameObject;
                    wallBroken = (int)WallGroup.Walls.Left;
                    if (desired >= 0)
					    wallDesired = wallCellModel[desired].transform.Find("Wall 1").gameObject;
				    break;

			    case 5:
				    desired = FindIndex (x-1, y+1);
				    wallPos = wallCellModel[pos].transform.Find("Wall 5").gameObject;
                    wallBroken = (int)WallGroup.Walls.Topleft;
                    if (desired >= 0)
					    wallDesired = wallCellModel[desired].transform.Find("Wall 2").gameObject;
				    break;
		}

			if(wallPos != null && wallDesired != null) {
				Destroy(wallPos);
				Destroy(wallDesired);
                wallCellGroup[pos].BreakWall(wallBroken);
			}
		}
	}

	void ExploreCell(int x, int y) {
		List<int> possibilities = new List<int>(new int[] {0,1,2,3,4,5});
		wallCellGroup[FindIndex(x, y)].isExplored = true;
		int counter = 0;
		while (possibilities.Count > 0 && counter < 20) {
			counter++;
			int num = Random.Range (0, possibilities.Count);
			int wall = possibilities[num];
			int newX = x;
			int newY = y;
			possibilities.RemoveAt (num);

			switch(wall) {
			case 0:
				newY++;
				break;
			case 1:
				newX++;
				break;
			case 2:
				newX++;
				newY--;
				break;
			case 3:
				newY--;
				break;
			case 4:
				newX--;
				break;
			case 5:
				newX--;
				newY++;
				break;

			}
			int index  = FindIndex (newX, newY);
			if( index > 0 && !wallCellGroup[index].isExplored) {
				ClearPath(x, y, wall);
				ExploreCell (newX, newY);
			}
			else if(index >= 0 && wallCellGroup[index].isExplored && Random.Range (0,100) > looping)
				ClearPath (newX, newY, wall);
		}
		//Debug.Log (counter + "!");
	}





}
