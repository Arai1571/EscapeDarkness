using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 移動スピード
    public float speed = 0.5f; // 反応距離
    public float reactionDistance = 4.0f;
    float axisH;                //横軸値x(-1.0 ~ 0.0 ~ 1.0)
    float axisV;                //縦軸値y(-1.0 ~ 0.0 ~ 1.0)

    Rigidbody2D rbody;          //Rigidbody 2D
    Animator animator;          //Animator

    bool isActive = false;      //アクティブフラグ
    //public int arrangeId = 0;   //配置の識別に使う


    public bool onBarrier; //バリア(お札)にあたっているか
    GameObject player; //プレイヤー情報

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();    // Rigidbody2Dを得る
        animator = GetComponent<Animator>();    //Animatorを得る
        player = GameObject.FindGameObjectWithTag("Player"); //プレイヤー情報を得る.startで一度だけ
    }

    void Update() //Playerを追いかける
    {
        //playingモードでないと何もしない
        if (GameManager.gameState != GameState.playing) return;

        //バリアに触れている時は何もしない
        if (onBarrier) return;

        //プレイヤーがいない時は何もしない
        if (player == null) return;

        //移動値初期化
        axisH = 0;
        axisV = 0;

        // Playerとの距離をチェック
        float dist = Vector2.Distance(transform.position, player.transform.position); 
        if (dist < reactionDistance) //reactionDistanceよりも小さかったら
        {
            isActive = true;    // アクティブにする（追いかける）
        }
        else
        {
            isActive = false;    // 非アクティブにする（止まる）
        }

        // アニメーションを切り替える
        animator.SetBool("IsActive", isActive);

        if (isActive)
        {
            animator.SetBool("IsActive", isActive);
            // プレイヤーへの角度を求める(プレイヤーの位置の座標からエネミーの座標を引くと底辺と高さが算出される。底辺と高さから角度算出->アークタンジェント。）

            float dx = player.transform.position.x - transform.position.x;
            float dy = player.transform.position.y - transform.position.y;

            float rad = Mathf.Atan2(dy, dx);  //アークタンジェントで角度を算出
            float angle = rad * Mathf.Rad2Deg;

            // 移動角度でアニメーションを変更する
            int direction;
            if (angle > -45.0f && angle <= 45.0f)
            {
                direction = 3;    //右向き
            }
            else if (angle > 45.0f && angle <= 135.0f)
            {
                direction = 2;    //上向き
            }
            else if (angle >= -135.0f && angle <= -45.0f)
            {
                direction = 0;    //下向き
            }
            else
            {
                direction = 1;    //左向き
            }

            animator.SetInteger("Direction", direction);
            // 移動するベクトルを作る
            axisH = Mathf.Cos(rad) * speed;
            axisV = Mathf.Sin(rad) * speed;
        }

    }

    void FixedUpdate() //バリアに当たった時
    {
        //playingモードでないと何もしない
        if (GameManager.gameState != GameState.playing) return;

        //バリアに触れている時は何もしない
        if (onBarrier)
        {
            rbody.linearVelocity = Vector2.zero;
            Debug.Log("バリア");
            return;
        }

        //プレイヤーがいない時は何もしない
        if (player == null) return;

        if (isActive)
        {
            // 移動
            rbody.linearVelocity = new Vector2(axisH, axisV).normalized;
        }
        else
        {
            rbody.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            onBarrier = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            onBarrier = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, reactionDistance);
    }
}
