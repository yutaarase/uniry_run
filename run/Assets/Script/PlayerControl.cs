using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playerの動きを制御するクラス
/// </summary>
public class PlayerControl : MonoBehaviour
{
	Rigidbody rb; //Rigidbody
	private Vector3 velocity; //移動ベクトル
	private Animator animator; //アニメーター
	public float walkSpeed = 20f; //歩きスピード
	public float dashSpeed = 40f; //ダッシュスピード
	public float creepSpeed = 10f; //匍匐前進スピード
	public float jumpSpeed = 500; //ジャンプスピード
	public float Multiplier = 1f; //重力の割合
	public bool run = false; //走ってるかどうか
	public bool push = false; //押したかどうか
	public float nextButtonDownTime = 0.3f; //ダブルタップ検出制限時間
	private float nowTime = 0f; //経過時間
	public float rotateSpeed = 10f; //回転スピード
	private float hori, vert; //垂直方向、平行方向入力変数
	private bool isGround; //着地したかどうか
	private bool jump; //ジャンプしているかどうか
	private const int IdleID = 0; //通常状態ID
	private const int WalkID = 1; //歩き状態ID
	private const int DashID = 2; // 走り状態ID
	private const int CreepID = 3; //匍匐前進状態ID
    private const int JumpID = 4; //ジャンプ状態ID
    private const int LandID = 5; //着地状態ID
    private int PStatus; //現在のPlayerの状態
	private float idleTime; //通常状態に戻す処理用変数
	public GameObject child; //自身の子のオブジェクト
	StaminaMeterControl SMC; //スタミナメーター
	void Start()
	{
		//インスタンス初期化
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		SMC = GetComponent<StaminaMeterControl>();
	}

	private void Update()
	{
		//Wkey押されたときの処理
		//歩き走り処理
		if (Input.GetKeyDown(KeyCode.W))
		{
			//ダブルタップ処理
			if (!push)
			{
				//押されてなかったら
				//押されたことにする
				//変数初期化
				//歩き状態にする
				push = true;
				nowTime = 0f;
				PStatus = WalkID;
			}
			else
			{
				//押されていたら
				//経過時間が検出制限時間以下でスタミナメーターが
				if (nowTime <= nextButtonDownTime && !SMC.tired)
				{
					//走り状態にする
					PStatus = DashID;
					run = true;
				}
			}
		}

		///押されたら
		if (push)
		{
			//経過時間加算
			nowTime += Time.deltaTime;

			//経過時間が制限時間より大きかったら
			if (nowTime > nextButtonDownTime)
			{
				//押されていない状態にする
				push = false;
			}
		}

		//Spaceを押された時の処理
		//ジャンプ処理
		if (Input.GetKeyDown(KeyCode.Space))
		{
			//ジャンプ状態じゃなかったら
			if (!jump)
			{
				//スタミナメーターの変数でジャンプが出来るなら
				if (!SMC.CntJump)
				{
					//ジャンプ状態にする
					//地面に触れてないようにする
					//ジャンプアニメーションを再生
					//y軸上方向の力を加える
					jump = true;
					isGround = false;
					PStatus = JumpID;
					animator.SetBool("Jumping", true);
					animator.SetBool("Landing", false);
					rb.AddRelativeForce(new Vector3(0f, jumpSpeed, 0f));
					
					//スタミナメーターの値を更新
					SMC.StaminaMeterUpdate(20);
				}
			}
		}

		//垂直方向、平行方向の入力取得
		hori = Input.GetAxis("Horizontal");
		vert = Input.GetAxis("Vertical");

		//Wkeyを押された時の処理
		//匍匐前進の処理
		if (Input.GetKey(KeyCode.W))
		{
			//LeftShiftkeyが押されたら
            if (Input.GetKey(KeyCode.LeftShift))
            {
				//子のオブジェクトを回転される(コライダーがついている)
				//匍匐前進状態にする
				//匍匐前進アニメーションを再生する
				//重力をオンにする
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
				//押されてなかったら
				//匍匐前進アニメーションを止める
				//コライダーを戻す
				animator.SetBool("Creeping", false);
				child.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

				if (run)
				{
					//走り状態だったら
					if (!SMC.tired)
					{
						//疲れていなかったら
						//走り状態にする
						//走るアニメーションを再生する
						PStatus = DashID;
						animator.SetBool("Running", true);
						animator.SetBool("Walking", false);
						velocity = new Vector3(0f, 0f, vert * dashSpeed);

					}
					else
					{
						//疲れていたら
						//走っていないようにする
						//歩き状態にする
						//歩きアニメーションを再生する
						run = false;
						PStatus = WalkID;
						animator.SetBool("Walking", true);
						animator.SetBool("Running", false);
						velocity = new Vector3(0f, 0f, vert * walkSpeed);
					}
				}
				else
				{
					//走り状態ではなかったら
					//歩き状態にする
					//歩きアニメーションを再生する
					PStatus = WalkID;
					animator.SetBool("Walking", true);
					animator.SetBool("Running", false);
					velocity = new Vector3(0f, 0f, vert * walkSpeed);
				}
			}
		}
		else
		{
			//押されてなかったら
			//通常状態にする
			//歩き、走り、匍匐前進のアニメーションを止める
			PStatus = IdleID;
			animator.SetBool("Walking", false);
			animator.SetBool("Running", false);
			animator.SetBool("Creeping", false);
			run = false;
			velocity = Vector3.zero;
		}

		//Ｈkeyが押されたら
		//ヘルプ用
        if (Input.GetKeyDown(KeyCode.H))
        {
			//通常状態にする
			//y軸方向に力を加える
			PStatus = IdleID;
			rb.AddRelativeForce(new Vector3(0f,500f,0f));
		}

		//地面に触れていなかったら
		if (!isGround)
		{
			//ジャンプアニメーションを再生する
			animator.SetBool("Jumping", true);
			animator.SetBool("Walking", false);
			animator.SetBool("Running", false);
			animator.SetBool("Landing", false);
		}

		//マウスの移動に合わせて視点移動をする
		Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed, Input.GetAxis("Mouse Y") * rotateSpeed, 0);
		transform.RotateAround(transform.position, transform.up, angle.x);
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		//Playerの状態に合わせて処理を行う
		switch (PStatus)
        {
			case IdleID:
				//Idle以外のアニメーションを止める
				animator.SetBool("Jumping", false);
				animator.SetBool("Walking", false);
				animator.SetBool("Running", false);
				animator.SetBool("Landing", false);
				animator.SetBool("Climbing", false);
				break;
			case WalkID:
			case DashID:
			case CreepID:
				//ジャンプなどのアニメーションを止める
				//移動ベクトルの力を加える
				animator.SetBool("Jumping", false);
				animator.SetBool("Landing", false);
				animator.SetBool("Climbing", false);
				rb.AddRelativeForce(velocity);
				break;
			case JumpID:
				//何もしない
				break;
			case LandID:
				//着地してから通常状態に戻す処理
				idleTime += Time.fixedDeltaTime;
				if(idleTime > 0.4)
                {
					//通常状態にする
					animator.SetBool("Landing", false);
					PStatus = IdleID;
					idleTime = 0;
                }
				break;
		}

		//上の処理とは別の処理用
		switch (PStatus)
        {
			case DashID:
				//スタミナメータースタミナ更新
				SMC.StaminaMeterUpdate(1);
				break;
			case JumpID:
				
				break;

			case WalkID:
				//スタミナメータースタミナ更新
				//スタミナ回復
				SMC.StaminaMeterUpdate(-0.2f);
				break;
			case IdleID:
			case LandID:
				//スタミナメータースタミナ更新
				//スタミナ回復
				SMC.StaminaMeterUpdate(-0.4f);
				break;
        }
		//速く落とす為、追加重力処理
		rb.AddForce((Multiplier - 1f) * Physics.gravity, ForceMode.Acceleration);
	}

	private void OnCollisionEnter(Collision collision)
	{
		//足場、壁、建物に触れていたら
		if (collision.collider.tag == "Floor" || collision.collider.tag == "Wall" || collision.collider.tag == "Building")
		{
			//着地していなかったら
			if(!isGround)
            {
				//触れているようにする
				//ジャンプしていない状態にする
				//着地アニメーションを再生する
				//状態を着地ＩＤにする
				isGround = true;
				jump = false;
				animator.SetBool("Landing", true);
				PStatus = LandID;
				rb.velocity = Vector3.zero;
			}
        }

		//地面に触れていたら
		if (collision.collider.tag == "Terrain")
        {
			//スポーンポイントを取得
			//スポーンポイントへ移動する
			var trans = GameObject.FindGameObjectWithTag("Point").transform;
			transform.position = trans.position;
			trans.rotation = trans.rotation;

        }
	}
}