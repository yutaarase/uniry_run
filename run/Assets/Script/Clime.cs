using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁を登る処理を行うクラス
/// </summary>
public class Clime : MonoBehaviour
{
    //アニメーター
    public Animator animator;
    //登スピード
    public float climbSpeed = 4000;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //インスタンスを初期化
        rb = this.transform.parent.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        //壁に触れていたら
        if (collision.gameObject.tag == "Wall")
        {
            //他のアニメーションの再生をやめる
            //登るアニメーションを再生させる
            //y軸上方向に力を加える
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
