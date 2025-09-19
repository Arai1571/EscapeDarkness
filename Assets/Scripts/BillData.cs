using UnityEngine;

public class BillData : MonoBehaviour
{
    Rigidbody2D rbody;
    public int itemNum;  //アイテムの識別番号

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();     //RigidBody2Dコンポーネント取得
        rbody.bodyType = RigidbodyType2D.Static; //Rigidbodyの挙動を静止
    }

    private void OnTiriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.bill++;  //１増やす
            GameManager.itemsPickedState[itemNum] = true;  //該当する取得フラグ(ItemsState)をtrue(ON)にする
            //アイテム取得演出
            //コライダーを無効化
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            //Rigidbody2Dの復活（Dynamicにする）
            rbody.bodyType = RigidbodyType2D.Dynamic;
            //上に打ち上げる（上向き５の力）
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); 
            //オブジェクトを削除する
            Destroy(collision.gameObject); 
        }
   }
}
