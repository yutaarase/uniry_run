using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	Rigidbody rb;
	private Vector3 velocity;
	private Animator animator;
	public float walkSpeed = 20f;
	public float dashSpeed = 40f;
	public float creepSpeed = 10f;
	public float jumpSpeed = 500;
	public float Multiplier = 1f;
	public bool run = false;
	public bool push = false;
	public float nextButtonDownTime = 0.3f;
	private float nowTime = 0f;
	public float rotateSpeed = 10f;
	float hori, vert;
	private bool isGround;
	private bool jump;
	private const int IdleID = 0;
	private const int WalkID = 1;
	private const int DashID = 2;
	private const int CreepID = 3;
	private const int JumpID = 4;
	private const int LandID = 5;
	private int PStatus;
	private float idleTime;
	public GameObject child;
	StaminaMeterControl SMC;
	void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		SMC = GetComponent<StaminaMeterControl>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (!push)
			{
				push = true;
				nowTime = 0f;
				PStatus = WalkID;
			}
			else
			{
				if (nowTime <= nextButtonDownTime && !SMC.poop)
				{
					PStatus = DashID;
					run = true;
				}
			}
		}

		if (push)
		{
			nowTime += Time.deltaTime;

			if (nowTime > nextButtonDownTime)
			{
				push = false;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (!jump)
			{
				if (!SMC.CntJump)
				{
					jump = true;
					isGround = false;
					PStatus = JumpID;
					animator.SetBool("Jumping", true);
					animator.SetBool("Landing", false);
					rb.AddRelativeForce(new Vector3(0f, jumpSpeed, 0f));
					SMC.StaminaMeterUpdate(20);
				}
			}
		}

		hori = Input.GetAxis("Horizontal");
		vert = Input.GetAxis("Vertical");


		if (Input.GetKey(KeyCode.W))
		{
            if (Input.GetKey(KeyCode.LeftShift))
            {
				child.transform.rotation = Quaternion.Euler(90f, 0f,0f);
				PStatus = CreepID;
				animator.SetBool("Creeping", true);
				animator.SetBool("Walking", false);
				animator.SetBool("Running", false);
				rb.velocity = Vector3.zero;
				rb.AddRelativeForce(0f, 0f, vert * creepSpeed);
				rb.useGravity = true;
			}
            else
            {
				animator.SetBool("Creeping", false);
				child.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				if (run)
				{
					if (!SMC.poop)
					{
						PStatus = DashID;
						animator.SetBool("Running", true);
						animator.SetBool("Walking", false);
						velocity = new Vector3(0f, 0f, vert * dashSpeed);

					}
					else
					{
						run = false;
						PStatus = WalkID;
						animator.SetBool("Walking", true);
						animator.SetBool("Running", false);
						velocity = new Vector3(0f, 0f, vert * walkSpeed);
					}
				}
				else
				{
					PStatus = WalkID;
					animator.SetBool("Walking", true);
					animator.SetBool("Running", false);
					velocity = new Vector3(0f, 0f, vert * walkSpeed);
				}
			}
		}
		else
		{
			PStatus = IdleID;
			animator.SetBool("Walking", false);
			animator.SetBool("Running", false);
			animator.SetBool("Creeping", false);
			run = false;
			velocity = Vector3.zero;
		}

        if (Input.GetKeyDown(KeyCode.H))
        {
			
			PStatus = IdleID;
			rb.AddRelativeForce(new Vector3(0f,500f,0f));
		}

		if (!isGround)
		{
			animator.SetBool("Jumping", true);
			animator.SetBool("Walking", false);
			animator.SetBool("Running", false);
			animator.SetBool("Landing", false);
		}

		Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed, Input.GetAxis("Mouse Y") * rotateSpeed, 0);
		transform.RotateAround(transform.position, transform.up, angle.x);
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		switch (PStatus)
        {
			case IdleID:
				animator.SetBool("Jumping", false);
				animator.SetBool("Walking", false);
				animator.SetBool("Running", false);
				animator.SetBool("Landing", false);
				animator.SetBool("Climbing", false);
				break;
			case WalkID:
			case DashID:
			case CreepID:
				animator.SetBool("Jumping", false);
				animator.SetBool("Landing", false);
				animator.SetBool("Climbing", false);
				rb.AddRelativeForce(velocity);
				break;
			case JumpID:
				Debug.Log("Jumping");
				break;
			case LandID:
				idleTime += Time.fixedDeltaTime;
				if(idleTime > 0.4)
                {
					animator.SetBool("Landing", false);
					PStatus = IdleID;
					idleTime = 0;
                }
				break;
		}

		switch (PStatus)
        {
			case DashID:
				SMC.StaminaMeterUpdate(1);
				break;
			case JumpID:
				
				break;

			case WalkID:
				SMC.StaminaMeterUpdate(-0.2f);
				break;
			case IdleID:
			case LandID:
				SMC.StaminaMeterUpdate(-0.4f);
				break;
        }
		rb.AddForce((Multiplier - 1f) * Physics.gravity, ForceMode.Acceleration);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Floor" || collision.collider.tag == "Wall" || collision.collider.tag == "Building")
		{
			if(!isGround)
            {
				isGround = true;
				jump = false;
				animator.SetBool("Landing", true);
				PStatus = LandID;
				rb.velocity = Vector3.zero;
			}
        }
		if (collision.collider.tag == "Terrain")
        {
			var trans = GameObject.FindGameObjectWithTag("Point").transform;
			transform.position = trans.position;
			trans.rotation = trans.rotation;

        }
	}
}