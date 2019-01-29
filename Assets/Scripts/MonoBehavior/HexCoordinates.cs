using UnityEngine;

[System.Serializable]
public struct HexCoordinates {
	// A struct used in the creation of an offset coordinate system.

	public int X;
	
	public int Z;
	
	public HexCoordinates (int x, int z) {
		X = x;
		Z = z;
	}
	public static HexCoordinates FromOffsetCoordinates (int x, int z) {
		return new HexCoordinates(x - z / 2, z);
	}

	public override string ToString () {
		return X.ToString() + " " + Z.ToString();
	}
	
	public string ToStringOnSeparateLines () {
		return X.ToString() + "\n" + Z.ToString();
	}

}