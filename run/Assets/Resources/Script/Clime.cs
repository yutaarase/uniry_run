using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clime : MonoBehaviour
{
    public Animator animator;
    public float climbSpeed = 4000;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.transform.parent.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            animator.SetBool("Landing", false);
            animator.SetBool("Climbing", true);
            rb.AddRelativeForce(new Vector3(0f, climbSpeed, 0f));
            rb.angularVelocity = Vector3.zero;
        }
    }
}
