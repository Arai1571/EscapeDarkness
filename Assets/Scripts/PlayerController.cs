using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの基礎ステータス")]
    public float playerSpeed = 3.0f;

    float axisH;  //横方向の入力状況
    float axisV;  //縦方向の入力状況

    [Header("プレイヤーの角度計算用")]
    public float angleZ = -90f; //プレイヤーの角度取得用

    [Header("オン・オフの対象スポットライト")]
    public GameObject spotLight; //対象のスポットライト

    bool inDamage; //ダメージ中かどうかのフラグ管理

    //Component
    Rigidbody2D rbody;
    Animator anime;

    void Start()
    {
        //コンポーネントの取得
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        //スポットライトを所持していればスポットライト表示
        if (GameManager.hasSpotLight)
        {
            spotLight.SetActive(true); // 表示  
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイ中でなければ何もしない
        if (GameManager.gameState != GameState.playing) return;
        
        Move(); //上下左右の入力値の取得
        angleZ = GetAngle();//その時の角度を変数angleZに反映
        Animation();  //angleZを利用してアニメーション
    }

    private void FixedUpdate()
    {
        //プレイ中でなければ何もしない
        if (GameManager.gameState != GameState.playing) return;

        //ダメージフラグが立っている間
        if (inDamage)
        {
            //点滅演出。SpriteRenderを一定周期で表示・非表示を繰り返す
            //Sinメソッドの角度情報にゲーム開始からの経過時間を与える
            float val = Mathf.Sin(Time.time * 50);

            if (val > 0)
            {
                //描写機能を有効
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //描写機能を無効
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            //入力によるvelocityが入らないようにここでリターン
            return;
        }

        // 入力状況に応じてPlayerを動かす
        rbody.linearVelocity = (new Vector2(axisH, axisV)).normalized * playerSpeed;
    }

    //上下左右の入力値の取得
    public void Move()
    {
        //axisHとaxisVに入力状況を代入する
        axisH = Input.GetAxisRaw("Horizontal"); // 左右キー
        axisV = Input.GetAxisRaw("Vertical");   // 上下キー
    }

    //その時のプレイヤーの角度を取得するメソッド
    public float GetAngle()
    {
        //現在座標の取得
        Vector2 fromPos = transform.position;

        //その瞬間のキー入力値（axisH,axisV）に応じた予測座標の取得
        Vector2 toPos = new Vector2(fromPos.x + axisH, fromPos.y + axisV);

        float angle;  //returnされる値の準備

        //もしも何かしらの入力があれば新たに角度算出
        if (axisH != 0 || axisV != 0)
        {
            // fromPos → toPos のベクトルを計算
            float dirX = toPos.x - fromPos.x;
            float dirY = toPos.y - fromPos.y;

            // Atan2でラジアン角を取得 → 度に変換
            angle = Mathf.Atan2(dirY, dirX) * Mathf.Rad2Deg;
        }
        //何も入力されていなければ前フレームの角度情報を据え置き
        else
        {
            angle = angleZ;
        }
        return angle;
    }

    void Animation()
    {
        //何らかの入力がある場合
        if (axisH != 0 || axisV != 0)
        {
            //ひとまずRunアニメを走らせる
            anime.SetBool("run", true);

            //angleZを利用して方角を決める.  パラメータdirection int型
            //int型のdirection 下：０、上：１、右：２、左：それ以外(3)

            if (angleZ > -135f && angleZ < -45f)  //下方向
            {
                anime.SetInteger("direction", 0);
            }
            else if (angleZ >= -45f && angleZ <= 45f) //右方向
            {
                anime.SetInteger("direction", 2);
                transform.localScale = new Vector2(1, 1);
            }
            else if (angleZ > 45f && angleZ < 135f) //上方向
            {
                anime.SetInteger("direction", 1);
            }
            else //左方向
            {
                anime.SetInteger("direction", 3);
                transform.localScale = new Vector2(-1, 1);
            }
        }

        else//入力がない場合
        {
            anime.SetBool("run", false);  //走るフラグをOff
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ぶつかった相手がEnemyだったら
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetDamage(collision.gameObject); //ダメージ処理の開始

        }
    }

    void GetDamage(GameObject enemy)
    {
        //ステータスがPlayingでなければ何もせず終わり
        if (GameManager.gameState != GameState.playing) return;

        GameManager.playerHP--; //プレイヤーHPを１減らす

        if (GameManager.playerHP > 0)
        {
            //そこまでのプレイヤーの動きをストップ
            rbody.linearVelocity = Vector2.zero; //new Vector2(0,0)
            //プレイヤーと敵との差を取得し、方向を決める
            Vector3 v = (transform.position - enemy.transform.position).normalized;
            //決まった方向に押される
            rbody.AddForce(v * 4, ForceMode2D.Impulse);

            //点滅するためのフラグ
            inDamage = true;

            //時間差で0.25秒後に点滅フラグを解除
            Invoke("DamageEnd", 0.25f);
        }
        else
        {
            //残HPが残っていなければゲームオーバー
            GameOver();
        }
    }

    void DamageEnd()
    {
        inDamage = false; //点滅ダメージフラグを解除
        gameObject.GetComponent<SpriteRenderer>().enabled = true; //プレイヤーを確実に表示
    }

    void GameOver()
    {
        //ゲームStateを変える
        GameManager.gameState = GameState.gameover;

        //ゲームオーバー演出.Playerが持っている当たり判定のコンポーネント[CircleCollider2D]の無効化
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        rbody.linearVelocity = Vector2.zero; //動きを止める
        rbody.gravityScale = 1.0f;  //重力の復活
        anime.SetTrigger("dead");   //死亡アニメクリップの発動
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); //上に跳ね上げる
        Destroy(gameObject, 1.0f); //１秒後に存在を消去
    }
}
