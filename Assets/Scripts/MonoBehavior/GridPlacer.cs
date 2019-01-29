using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacer
{
    public const float distRatio = 2 * HexMetrics.innerRadius;
    public static int width = HexGenerator.width;
    public static int height = HexGenerator.height;
    public List<int[]> availableCoordinates = new List<int[]>();

    public GridPlacer()
    {
        availableCoordinates = null;
    }
    public GridPlacer(int[,] potentialCoordinates)
    {
        for (int i = 0; i < potentialCoordinates.GetLength(0); i++)
            availableCoordinates.Add(new int[] { potentialCoordinates[i, 0], potentialCoordinates[i, 1] });
    }


    public void PlacePrefabAt(int x, int z, Transform prefab)
    {
        placePrefabAt(x, z, 0, prefab);
    }

    public void placePrefabAt(int x, int z, float height, Transform prefab)
    {
        float offSet = z * distRatio;
        float realX = x * distRatio + offSet * 0.5f;
        float realZ = offSet * Mathf.Sin(Mathf.PI / 3);
        GameObject.Instantiate(prefab, new Vector3(realX, height, realZ), prefab.rotation);
    }

    public void PlaceRandomPrefab(Transform prefab)
    {
        if (availableCoordinates != null)
        {
            int random = Random.Range(0, availableCoordinates.Count);
            int[] coords = availableCoordinates[random];
            PlacePrefabAt(coords[0], coords[1], prefab);
            availableCoordinates.RemoveAt(random);
        }
        else
            Debug.LogWarning("Warning: Available Coordinates not Initialized");

    }


    public void PlaceDistanceAway(Pathfinder finder, int[] playerCoords, Transform prefab, int minSteps, int height)
    {
        int random = Random.Range(0, availableCoordinates.Count);
        int[] coords = availableCoordinates[random];
        List<WallGroup> potentialPath1 = finder.GetShortestPath(playerCoords, coords);
        List<WallGroup> potentialPath2 = finder.GetShortestPath(coords, playerCoords);
        if (potentialPath1.Count >= minSteps || potentialPath2.Count >= minSteps)
            placePrefabAt(coords[0], coords[1], height, prefab);
        else
            PlaceDistanceAway(finder, playerCoords, prefab, minSteps, height);
    }

    public void PlaceDistanceAway(Pathfinder finder, int[] playerCoords, Transform prefab, int minSteps)
    {
        PlaceDistanceAway(finder, playerCoords, prefab, minSteps, 0);
    }
}
