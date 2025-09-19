using Unity.VisualScripting;
using UnityEngine;

public class BillController : MonoBehaviour
{
    public float deleteTime = 2.0f;  //自動発動までの時間  
    public GameObject barrierPrefab; //自己消滅と引き換えに生成するプレハブ

    void Start()
    {
        //deleteTime秒後に「バリア展開して消滅」
        Invoke("FieldExpansion", deleteTime);
    }

    void FieldExpansion()
    {
        Instantiate(barrierPrefab, transform.position, Quaternion.identity); //お札と同じ場所にバリア生成
        Destroy(gameObject);  //お札は消滅
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            FieldExpansion();
        }
    }
}
