using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static int[] doorsPositionNumber = { 0, 0, 0 }; //各入り口の配置番号
    public static int key1PositionNumber;   //鍵１の配置番号
    public static int[] itemsPositionNumber; //アイテムの配置番号

    public GameObject items;     //５つのアイテムプレハブの内訳
    public GameObject room;      //ドアのプレハブ
    public GameObject dummyDoor; //ダミーのドアプレハブ
    public GameObject key;       //キーのプレハブ

    public static bool positioned;     //初回配置が済みかどうか
    public static string toRoomNumber; //Playerが配置されるべき位置

    GameObject player;   //プレイヤーの情報

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //プレイヤー情報の取得
        player = GameObject.FindGameObjectWithTag("Player");

        if (!positioned) //初期配置がまだ
        {
            StartKeysPosition(); //キーの初回配置
            positioned = true;   //初回配置は済み
        }
    }
    void StartKeysPosition()
    {
        //全スポットの取得
        GameObject[] keySpots = GameObject.FindGameObjectsWithTag("KeySpot");

        //ランダムに番号を取得（第一引数以上第二引数未満）
        int rand = Random.Range(1, (keySpots.Length + 1));

        //全スポットをチェックしに行く
        foreach (GameObject spots in keySpots)
        {
            //一つひとつspotNumの中身を確認してrandと同じかチェック
            if (spots.GetComponent<KeySpot>().spotNum == rand)
            {
                //キー１を生成
                Instantiate(key, spots.transform.position, Quaternion.identity);

                //どのスポット番号にキーを配置したか記録
                key1PositionNumber = rand;
            }
        }
    }

}
