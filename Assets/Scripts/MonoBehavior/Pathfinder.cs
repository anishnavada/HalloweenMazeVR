using System.Collections.Generic;
using UnityEngine;

public class Pathfinder {

	// Every cell on the map.
	static WallGroup[] wallCellGroup;

	// Every Cell already considered.
	List<WallGroup> evaluated;
	// Every Cell that can be reached.
	List<WallGroup> discovered;

	// 2d Map of f values and g values.
	int[,] fmap, gmap;
	// Map of cells' closest neighbors.
	WallGroup[,] camefrom;

	public Pathfinder(WallGroup[] grid)
	{
        // Just generating and filling fields. Be sure to pass the array of cells!
        ResetAll(grid);

	}

    private void ResetAll(WallGroup[] grid)
    {
        wallCellGroup = grid;
        evaluated = new List<WallGroup>();
        discovered = new List<WallGroup>();
        fmap = new int[HexGenerator.width, HexGenerator.height];
        gmap = new int[HexGenerator.width, HexGenerator.height];
        camefrom = new WallGroup[HexGenerator.width, HexGenerator.height];
        for (int i = 0; i < fmap.GetLength(0); i++)
            for (int j = 0; j < fmap.GetLength(1); j++)
            {
                fmap[i, j] = 99999;
                gmap[i, j] = 99999;
            }

    }

    public List<WallGroup> GetShortestPath(int[] startI, int[] finishI)
	{
        // ***IMORTANT NOTE: ***D
        // Since the arrays are Hexagons and the Maps are rectangular,
        // it is of UPMOST IMPORTANCE that you account for the offset when
        // translating form hex coords to rect coords. (x + y/2, y) -> (x,y)
        ResetAll(wallCellGroup);
		int debugCounter = 0;
		//Starting and ending cells.
		WallGroup start = wallCellGroup[FindIndex(startI[0], startI[1])];
		WallGroup finish = wallCellGroup[FindIndex(finishI[0], finishI[1])];
		WallGroup neighbor = null ;
		// So far, only start should be open for evaulation.
		discovered.Add(start);
		// No cost to get from start to start.
		gmap[start.x + start.y / 2, start.y] = 0;
		// Heuristic distance from start to end.
		fmap[start.x + start.y / 2, start.y] = getEstimate(start, finish);
		WallGroup currentNode;
		while (discovered.Count > 0 && debugCounter < 100)
		{
			// For loop finds next node with lowest heuristic values.
			int minXF = discovered[0].x + discovered[0].y/2, minYF = discovered[0].y;
			for (int i = 0; i < discovered.Count; i++) { 
				if (fmap[minXF, minYF] > fmap[discovered[i].x + discovered[i].y/2, discovered[i].y])
				{
					minXF = discovered[i].x + discovered[i].y / 2;
					minYF = discovered[i].y;
				}
			}
			// The node with the lowest fMap value.
			currentNode = wallCellGroup[FindIndex(minXF - minYF/2, minYF)];
			// At the goal? Then you're finished!!!
			if (currentNode.x == finish.x && currentNode.y == finish.y)
			{
				return ReconstructPath(currentNode);
			}
			// Remove current from discovered, add it to evaluated.
			discovered.RemoveAt(findIndex2(discovered, currentNode));
			evaluated.Add(currentNode);
			// For every potential neighbor...
			for (int i = 0; i < 6; i++) {
                // Find the potential neighbor for the corresponding side.
                int[] adjCoords = currentNode.GetAdjacentCoords(i);
					// Make sure it exists on the map!
				if (FindIndex(adjCoords[0], adjCoords[1]) >= 0)
					neighbor = wallCellGroup[FindIndex(adjCoords[0], adjCoords[1])];
				else
					continue;
				// Make sure it was not already evaluated.
				if (Contains(evaluated, neighbor))
					continue;
				// If it hasn't been discovered, discover it.
				if (!Contains(discovered, neighbor))
					discovered.Add(neighbor);

				int gScore;
				// Calculate A* score.
				gScore = gmap[currentNode.x + currentNode.y/2,currentNode.y] + getEstimate(currentNode, neighbor);
                // It is very hard to walk through walls...
                // Ignore the path if it isn't better.
                if (!currentNode.IsBroken(i))
                    gScore *= 2000;
                if (gScore > gmap[neighbor.x + neighbor.y / 2, neighbor.y])
					continue;
				// Mark it as the new best path, and update the costs for the better path.
				camefrom[neighbor.x + neighbor.y / 2, neighbor.y] = currentNode;
				gmap[neighbor.x + neighbor.y / 2, neighbor.y] = gScore;
				fmap[neighbor.x + neighbor.y / 2, neighbor.y] = gScore + getEstimate(start, neighbor);
			}
			// Infinite loops are bad.
			debugCounter++;
		}
		return null;
	}

	private List<WallGroup> ReconstructPath(WallGroup currentNode)
	{
        // This method works backwards to find the best path.
        List<WallGroup> path = new List<WallGroup> {currentNode};
        while (camefrom[currentNode.x + currentNode.y/2, currentNode.y] != null)
		{
			currentNode = camefrom[currentNode.x + currentNode.y / 2, currentNode.y];
			path.Add(currentNode);
		}
		return path;

	}



	private int FindIndex(int x, int y)
	{
		// Same as in GenerateWalls, get an index in wallCellGroup based on its coords.
		for (int i = 0; i < wallCellGroup.Length; i++)
		{
			if (x == wallCellGroup[i].x && y == wallCellGroup[i].y)
				return i;
		}
		return -1;
	}
	private int findIndex2(List<WallGroup> list, WallGroup group)
	{
		// Get a WallGroup from a list of WallGroups.
		for (int i = 0; i < list.Count; i++)
		{
			if (group.x == list[i].x && group.y == list[i].y)
				return i;
		}
		return -1;
	}



	private bool Contains(List<WallGroup> list, WallGroup group)
	{
		// Checks if a WallGroup is inside of a list of them (based on coords).
		for (int i = 0; i < list.Count; i++)
			if (list[i].x == group.x && list[i].y == group.y)
				return true;
		return false;
	}

    int getEstimate(WallGroup A, WallGroup B)
    {
        return (Mathf.Abs(A.x - B.x) + Mathf.Abs(A.x + A.y - B.x - B.y) + Mathf.Abs(A.y - B.y)) / 2;
    }

    int[] AxialToCube(WallGroup hex)
    {
        int x = hex.x;
        int z = hex.y;
        int y = -x - z;
        return new int[] { x, y, z };
    }


}