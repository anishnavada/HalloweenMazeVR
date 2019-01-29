using UnityEngine;
using System.Collections;

public class SimpleCharcterController : MonoBehaviour
{
    // All the neat character controls and animations are done here.



    Animator animator; // the Animator paramtercomponent/ state engine

    private float fatigue = 0;
    private bool tired = false;
	private RaycastHit hit;
    public new Transform camera;
	private ChaseCharacter cc;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>(); // assign the animator
		cc = GetComponent<ChaseCharacter>();
    }

    // Update is called once per frame

	public void SimpleMove(Vector3 d){
		transform.position += d; 
		camera.position = transform.position + new Vector3(0, 1.5f, 0.1f);
	}

    void Update()
    {
        int punches = animator.GetInteger("Punch Queue");
        if (animator)
        {
            if (Input.GetMouseButtonDown(0) && punches < 2)
            {
                animator.SetInteger("Punch Queue", punches + 1);
                fatigue += 1f;
            }

            if (fatigue >= 6.0f)
            {
                tired = true;
                animator.SetFloat("Fatigue Multiplier", 0.5f);
            }

            if (Input.GetKey(KeyCode.LeftShift) && !tired)
            {
                animator.SetBool("Run", true);
                fatigue += Time.deltaTime;
            }
            else
            {
                animator.SetBool("Run", false);
                if (fatigue > 0)
                    fatigue -= Time.deltaTime;
                else
                {
                    tired = false;
                    animator.SetFloat("Fatigue Multiplier", 1);
                }
            }

            float v = Input.GetAxis("Vertical");
            animator.SetFloat("YSpeed", v);

            float h = Input.GetAxis("Horizontal") * 0.1f;
            transform.position += transform.right * h;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, camera.eulerAngles.y, transform.eulerAngles.z);
            camera.position = transform.position + new Vector3(0, 1.5f, 0.1f);
			Vector3 forward = transform.TransformDirection (Vector3.forward);
			if (Physics.Raycast (transform.position, forward, out hit, 2)) {
				if (hit.collider.tag == "Enemy") {
					cc.Dissapear ();
				}
			}
            //camera.eulerAngles = new Vector3(camera.eulerAngles.x, transform.eulerAngles.y, camera.eulerAngles.z);
        }
    }



}
