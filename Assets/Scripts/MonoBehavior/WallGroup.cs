using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallGroup {
    // An abstract version of a hex cell in the maze.
    // Vital for pathfinding and maze creation.

	public enum Walls {Topright, Right, Botright, Botleft, Left,Topleft};
    public int x, y;
    public List<int> wallsBroken = new List<int>();
    public bool isExplored = false;

    public WallGroup(int hor, int ver)
    {
        x = hor;
        y = ver;
    }

    public void BreakWall(int wall)
    {
        if(wall >= 0)
            wallsBroken.Add(wall);
    }

    public bool IsBroken(int wall) {
        return wallsBroken.Contains(wall);
       }

    override
    public string ToString()
    {
        string walls = "";
        foreach(int i in wallsBroken)
            walls += i + " ";
        return "Walls broken at: " + walls;
    }

    public int[] GetAdjacentCoords(int i)
    {
        int newX = x;
        int newY = y;
        switch(i) {
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
        return new int[] { newX, newY };
    }


}
