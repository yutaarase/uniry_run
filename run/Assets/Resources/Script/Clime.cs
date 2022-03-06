using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǂ�o�鏈�����s���N���X
/// </summary>
public class Clime : MonoBehaviour
{
    //�A�j���[�^�[
    public Animator animator;
    //�o�X�s�[�h
    public float climbSpeed = 4000;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //�C���X�^���X��������
        rb = this.transform.parent.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        //�ǂɐG��Ă�����
        if (collision.gameObject.tag == "Wall")
        {
            //���̃A�j���[�V�����̍Đ�����߂�
            //�o��A�j���[�V�������Đ�������
            //y��������ɗ͂�������
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
