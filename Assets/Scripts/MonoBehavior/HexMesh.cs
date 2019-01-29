using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {
	// Responsible for both creating the hexagonal floor of the maze
    // and the walls and posts of each individual cell.
    // Works in tandem with HexGenerator.


	Mesh hexMesh;
	public Transform wallPrefab, groupPrefab, postPrefab, parent;
	List<Vector3> vertices;
	List<int> triangles;
	

	void Awake () {
		GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
		hexMesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();

	}

	public void Triangulate (HexCell[] cells) {
		hexMesh.Clear();
		vertices.Clear();
		triangles.Clear();
		for (int i = 0; i < cells.Length; i++) {
				Triangulate(cells[i]);
		}
		hexMesh.vertices = vertices.ToArray();

		hexMesh.triangles = triangles.ToArray();
		hexMesh.RecalculateNormals();
	}

	void Triangulate (HexCell cell) {
		Vector3 center = cell.transform.localPosition;
		for (int i = 0; i < 6; i++) {

			AddTriangle (
				center,
				center + HexMetrics.corners [i],
				center + HexMetrics.corners [1+i]
			);
		}
	}

	void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);

	}


	public void AddCells(int width, int height) {
		Transform group = null;
		for (int i = 0, j = 0; i < vertices.Count; i+=3, j ++) {
			Vector3 position = ((vertices [i + 1] + vertices [i + 2]) / 2);
			Quaternion rotation = Quaternion.identity;

			switch(j%6) {
			case (1):
			case (4):
				rotation = Quaternion.Euler(0, 90f, 0);
				break;
			case (2):
			case (5):
				rotation = Quaternion.Euler(0, -30f, 0);
				break;
			
			case (6):
			case (0):
				group = (Transform)Instantiate (groupPrefab, vertices[i] , Quaternion.identity);
                group.name = "" + (j / 6);
                group.SetParent(parent);
				goto case (3);
			case (3):
				rotation = Quaternion.Euler(0, 30f, 0);
				break;
			}
			Transform wall = (Transform)Instantiate (wallPrefab, position + new Vector3( 0, wallPrefab.transform.localScale.y/2, 0), rotation);
			Transform post = (Transform)Instantiate (postPrefab, vertices[i+1] + new Vector3( 0, 0, 0), rotation);

			wall.name = "Wall " + (j%6==6?0:j%6);
			wall.SetParent (group);
			post.name = "Post " + (j%6==6?0:j%6);
			post.SetParent (group);
		}
	}
}