using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HexGenerator : MonoBehaviour {
    // Most important class, creates a grid of hexagons, then calls other classes
    // to form a maze from said grid.


	public HexCell cellPrefab;

	public static int width = 8;
	public static int height = 8;

	HexCell[] cells;
	HexMesh hexMesh;
    List<string> coordinateList = new List<string>();

	public Text labelPrefab;
	Canvas gridCanvas;
	
	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();;
		cells = new HexCell[width*height];
		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void Start() {
		hexMesh.Triangulate(cells);
		hexMesh.AddCells (width, height);
        LabelWallGroups();
		GetComponent<GenerateWalls>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);


		//Make the cells
		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);



		// Label the cells
		Text label = Instantiate<Text>(labelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
        coordinateList.Add(cell.coordinates.ToString());
	}

    void LabelWallGroups()
    {
        GameObject[] wallGroups = GameObject.FindGameObjectsWithTag("Cell");
        for (int i = 0; i < wallGroups.Length; i++)
        {
            wallGroups[i].name = coordinateList[i];
        }
    }



}
