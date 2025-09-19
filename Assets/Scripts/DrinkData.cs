using UnityEngine;

public class DrinkData : MonoBehaviour
{
    Rigidbody2D rbody;
    public int itemNum;  //アイテムの識別番号

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType=RigidbodyType2D.Static;
    }

    // Update is called once per frame
    private void OnTiriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //PlayerのHPが最大なら何もしない
            if (GameManager.playerHP < 3)
            {
                GameManager.playerHP++;
            }

            //該当する識別番号
            GameManager.itemsPickedState[itemNum] = true;

            //アイテム取得の演出
            GetComponent<CircleCollider2D>().enabled = false;
            rbody.bodyType = RigidbodyType2D.Dynamic;
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);
        }
    }
}
