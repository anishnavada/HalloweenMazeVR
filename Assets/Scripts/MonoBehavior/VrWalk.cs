using UnityEngine;
using System.Collections;

public class VrWalk : MonoBehaviour {
	public Transform vrCamera;
	public float toggleAngle = -10.0f;//when you look on X axis at this angle you will walk was 30.0f
	public float speed=0.03f;//speed at which you walk
	public bool moveForward;
	public SimpleCharcterController cc;

	// Use this for initialization
	void Start () {
		cc = GetComponent<SimpleCharcterController> ();

	}

	// Update is called once per frame
	void Update () {
		if (vrCamera.localEulerAngles.y>= toggleAngle && vrCamera.localEulerAngles.y < 10.0f) {

			moveForward = true;
		} else {
			moveForward = false;
		}
		if (moveForward) {
			Vector3 forward = vrCamera.TransformDirection (Vector3.forward);
			cc.SimpleMove (forward * speed);
		}

	}
}