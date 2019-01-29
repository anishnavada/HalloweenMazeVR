using UnityEngine;
using System.Collections;

public static class HexMetrics {
    // Stores the size and vector positions of each hexagon cell.
    // Feel free to change the outer radius if desired, but ONLY the outer radius.


	public const float outerRadius = 10f;
	public const float innerRadius = outerRadius * 0.866025404f;
	
	public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(0f, 0f, outerRadius)

	};
}