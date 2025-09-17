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
        Move(); //上下左右の入力値の取得
    }

    private void FixedUpdate()
    {
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
}
