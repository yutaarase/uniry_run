using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player�̓����𐧌䂷��N���X
/// </summary>
public class PlayerControl : MonoBehaviour
{
	Rigidbody rb; //Rigidbody
	private Vector3 velocity; //�ړ��x�N�g��
	private Animator animator; //�A�j���[�^�[
	public float walkSpeed = 20f; //�����X�s�[�h
	public float dashSpeed = 40f; //�_�b�V���X�s�[�h
	public float creepSpeed = 10f; //�����O�i�X�s�[�h
	public float jumpSpeed = 500; //�W�����v�X�s�[�h
	public float Multiplier = 1f; //�d�͂̊���
	public bool run = false; //�����Ă邩�ǂ���
	public bool push = false; //���������ǂ���
	public float nextButtonDownTime = 0.3f; //�_�u���^�b�v���o��������
	private float nowTime = 0f; //�o�ߎ���
	public float rotateSpeed = 10f; //��]�X�s�[�h
	private float hori, vert; //���������A���s�������͕ϐ�
	private bool isGround; //���n�������ǂ���
	private bool jump; //�W�����v���Ă��邩�ǂ���
	private const int IdleID = 0; //�ʏ���ID
	private const int WalkID = 1; //�������ID
	private const int DashID = 2; // ������ID
	private const int CreepID = 3; //�����O�i���ID
    private const int JumpID = 4; //�W�����v���ID
    private const int LandID = 5; //���n���ID
    private int PStatus; //���݂�Player�̏��
	private float idleTime; //�ʏ��Ԃɖ߂������p�ϐ�
	public GameObject child; //���g�̎q�̃I�u�W�F�N�g
	StaminaMeterControl SMC; //�X�^�~�i���[�^�[
	void Start()
	{
		//�C���X�^���X������
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		SMC = GetComponent<StaminaMeterControl>();
	}

	private void Update()
	{
		//Wkey�����ꂽ�Ƃ��̏���
		//�������菈��
		if (Input.GetKeyDown(KeyCode.W))
		{
			//�_�u���^�b�v����
			if (!push)
			{
				//������ĂȂ�������
				//�����ꂽ���Ƃɂ���
				//�ϐ�������
				//������Ԃɂ���
				push = true;
				nowTime = 0f;
				PStatus = WalkID;
			}
			else
			{
				//������Ă�����
				//�o�ߎ��Ԃ����o�������Ԉȉ��ŃX�^�~�i���[�^�[��
				if (nowTime <= nextButtonDownTime && !SMC.tired)
				{
					//�����Ԃɂ���
					PStatus = DashID;
					run = true;
				}
			}
		}

		///�����ꂽ��
		if (push)
		{
			//�o�ߎ��ԉ��Z
			nowTime += Time.deltaTime;

			//�o�ߎ��Ԃ��������Ԃ��傫��������
			if (nowTime > nextButtonDownTime)
			{
				//������Ă��Ȃ���Ԃɂ���
				push = false;
			}
		}

		//Space�������ꂽ���̏���
		//�W�����v����
		if (Input.GetKeyDown(KeyCode.Space))
		{
			//�W�����v��Ԃ���Ȃ�������
			if (!jump)
			{
				//�X�^�~�i���[�^�[�̕ϐ��ŃW�����v���o����Ȃ�
				if (!SMC.CntJump)
				{
					//�W�����v��Ԃɂ���
					//�n�ʂɐG��ĂȂ��悤�ɂ���
					//�W�����v�A�j���[�V�������Đ�
					//y��������̗͂�������
					jump = true;
					isGround = false;
					PStatus = JumpID;
					animator.SetBool("Jumping", true);
					animator.SetBool("Landing", false);
					rb.AddRelativeForce(new Vector3(0f, jumpSpeed, 0f));
					
					//�X�^�~�i���[�^�[�̒l���X�V
					SMC.StaminaMeterUpdate(20);
				}
			}
		}

		//���������A���s�����̓��͎擾
		hori = Input.GetAxis("Horizontal");
		vert = Input.GetAxis("Vertical");

		//Wkey�������ꂽ���̏���
		//�����O�i�̏���
		if (Input.GetKey(KeyCode.W))
		{
			//LeftShiftkey�������ꂽ��
            if (Input.GetKey(KeyCode.LeftShift))
            {
				//�q�̃I�u�W�F�N�g����]�����(�R���C�_�[�����Ă���)
				//�����O�i��Ԃɂ���
				//�����O�i�A�j���[�V�������Đ�����
				//�d�͂��I���ɂ���
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
				//������ĂȂ�������
				//�����O�i�A�j���[�V�������~�߂�
				//�R���C�_�[��߂�
				animator.SetBool("Creeping", false);
				child.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

				if (run)
				{
					//�����Ԃ�������
					if (!SMC.tired)
					{
						//���Ă��Ȃ�������
						//�����Ԃɂ���
						//����A�j���[�V�������Đ�����
						PStatus = DashID;
						animator.SetBool("Running", true);
						animator.SetBool("Walking", false);
						velocity = new Vector3(0f, 0f, vert * dashSpeed);

					}
					else
					{
						//���Ă�����
						//�����Ă��Ȃ��悤�ɂ���
						//������Ԃɂ���
						//�����A�j���[�V�������Đ�����
						run = false;
						PStatus = WalkID;
						animator.SetBool("Walking", true);
						animator.SetBool("Running", false);
						velocity = new Vector3(0f, 0f, vert * walkSpeed);
					}
				}
				else
				{
					//�����Ԃł͂Ȃ�������
					//������Ԃɂ���
					//�����A�j���[�V�������Đ�����
					PStatus = WalkID;
					animator.SetBool("Walking", true);
					animator.SetBool("Running", false);
					velocity = new Vector3(0f, 0f, vert * walkSpeed);
				}
			}
		}
		else
		{
			//������ĂȂ�������
			//�ʏ��Ԃɂ���
			//�����A����A�����O�i�̃A�j���[�V�������~�߂�
			PStatus = IdleID;
			animator.SetBool("Walking", false);
			animator.SetBool("Running", false);
			animator.SetBool("Creeping", false);
			run = false;
			velocity = Vector3.zero;
		}

		//�gkey�������ꂽ��
		//�w���v�p
        if (Input.GetKeyDown(KeyCode.H))
        {
			//�ʏ��Ԃɂ���
			//y�������ɗ͂�������
			PStatus = IdleID;
			rb.AddRelativeForce(new Vector3(0f,500f,0f));
		}

		//�n�ʂɐG��Ă��Ȃ�������
		if (!isGround)
		{
			//�W�����v�A�j���[�V�������Đ�����
			animator.SetBool("Jumping", true);
			animator.SetBool("Walking", false);
			animator.SetBool("Running", false);
			animator.SetBool("Landing", false);
		}

		//�}�E�X�̈ړ��ɍ��킹�Ď��_�ړ�������
		Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed, Input.GetAxis("Mouse Y") * rotateSpeed, 0);
		transform.RotateAround(transform.position, transform.up, angle.x);
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		//Player�̏�Ԃɍ��킹�ď������s��
		switch (PStatus)
        {
			case IdleID:
				//Idle�ȊO�̃A�j���[�V�������~�߂�
				animator.SetBool("Jumping", false);
				animator.SetBool("Walking", false);
				animator.SetBool("Running", false);
				animator.SetBool("Landing", false);
				animator.SetBool("Climbing", false);
				break;
			case WalkID:
			case DashID:
			case CreepID:
				//�W�����v�Ȃǂ̃A�j���[�V�������~�߂�
				//�ړ��x�N�g���̗͂�������
				animator.SetBool("Jumping", false);
				animator.SetBool("Landing", false);
				animator.SetBool("Climbing", false);
				rb.AddRelativeForce(velocity);
				break;
			case JumpID:
				//�������Ȃ�
				break;
			case LandID:
				//���n���Ă���ʏ��Ԃɖ߂�����
				idleTime += Time.fixedDeltaTime;
				if(idleTime > 0.4)
                {
					//�ʏ��Ԃɂ���
					animator.SetBool("Landing", false);
					PStatus = IdleID;
					idleTime = 0;
                }
				break;
		}

		//��̏����Ƃ͕ʂ̏����p
		switch (PStatus)
        {
			case DashID:
				//�X�^�~�i���[�^�[�X�^�~�i�X�V
				SMC.StaminaMeterUpdate(1);
				break;
			case JumpID:
				
				break;

			case WalkID:
				//�X�^�~�i���[�^�[�X�^�~�i�X�V
				//�X�^�~�i��
				SMC.StaminaMeterUpdate(-0.2f);
				break;
			case IdleID:
			case LandID:
				//�X�^�~�i���[�^�[�X�^�~�i�X�V
				//�X�^�~�i��
				SMC.StaminaMeterUpdate(-0.4f);
				break;
        }
		//�������Ƃ��ׁA�ǉ��d�͏���
		rb.AddForce((Multiplier - 1f) * Physics.gravity, ForceMode.Acceleration);
	}

	private void OnCollisionEnter(Collision collision)
	{
		//����A�ǁA�����ɐG��Ă�����
		if (collision.collider.tag == "Floor" || collision.collider.tag == "Wall" || collision.collider.tag == "Building")
		{
			//���n���Ă��Ȃ�������
			if(!isGround)
            {
				//�G��Ă���悤�ɂ���
				//�W�����v���Ă��Ȃ���Ԃɂ���
				//���n�A�j���[�V�������Đ�����
				//��Ԃ𒅒n�h�c�ɂ���
				isGround = true;
				jump = false;
				animator.SetBool("Landing", true);
				PStatus = LandID;
				rb.velocity = Vector3.zero;
			}
        }

		//�n�ʂɐG��Ă�����
		if (collision.collider.tag == "Terrain")
        {
			//�X�|�[���|�C���g���擾
			//�X�|�[���|�C���g�ֈړ�����
			var trans = GameObject.FindGameObjectWithTag("Point").transform;
			transform.position = trans.position;
			trans.rotation = trans.rotation;

        }
	}
}