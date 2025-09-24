using System.Xml;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static int[] doorsPositionNumber = { 0, 0, 0 }; //各入り口の配置番号
    public static int key1PositionNumber;   //鍵１の配置番号
    public static int[] itemsPositionNumber = { 0, 0, 0, 0, 0 }; //アイテムの配置番号

    public GameObject[] items = new GameObject[5];     //５つのアイテムプレハブの内訳

    public GameObject room;      //ドアのプレハブ
    public MessageData[] messages;  //配置したドアに割り振るScriptableObject

    public GameObject dummyDoor; //ダミーのドアプレハブ
    public GameObject key;       //キーのプレハブ

    public static bool positioned;     //初回配置が済みかどうか
    public static string toRoomNumber = "fromRoom1"; //Playerが配置されるべき位置

    GameObject player;   //プレイヤーの情報

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //プレイヤー情報の取得
        player = GameObject.FindGameObjectWithTag("Player");

        if (!positioned) //初期配置がまだ
        {
            StartKeysPosition(); //キーの初回配置
            StartItemsPosition();//アイテムの初回配置
            StartDoorsPosition(); //ドアの初回配置
            positioned = true;   //初回配置は済み
        }
        else  //初期配置済みだった場合は配置を再現
        {
            LoadKeysPosition(); //キーの配置の再現
            LoadItemsPosition(); //アイテムの配置の再現
            LoadDoorsPosition(); //ドアの配置の再現
        }
    }
    void StartKeysPosition()
    {
        //全key1スポットの取得
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

        //Key2およびKey3の対象スポット
        GameObject keySpot;
        GameObject obj;       //生成したKey2,およびKey3が入る予定

        //Key2スポットの取得
        keySpot = GameObject.FindGameObjectWithTag("KeySpot2");
        //Keyの生成とobjへの格納
        obj = Instantiate(key, keySpot.transform.position, Quaternion.identity);
        //生成したKeyのタイプをkey2に変換
        obj.GetComponent<KeyData>().keyType = KeyType.key2;

        //Key3スポットの取得
        keySpot = GameObject.FindGameObjectWithTag("KeySpot3");
        //Keyの生成とobjへの格納
        obj = Instantiate(key, keySpot.transform.position, Quaternion.identity);
        //生成したKeyのタイプをkey3に変換
        obj.GetComponent<KeyData>().keyType = KeyType.key3;
    }


    void StartItemsPosition()
    {
        GameObject[] itemSpots = GameObject.FindGameObjectsWithTag("ItemSpot");

        for (int i = 0; i < items.Length; i++)
        {
            //ランダムな数字の取得

            //ただしアイテムの割り振り済みの番号を引いたらランダム引き直し

            int rand;  //ランダムな数の受け皿
            bool unique;  //重複していないかのフラグ

            do
            {
                unique = true; //問題なければそのままループを抜ける予定
                rand = Random.Range(1, (itemSpots.Length + 1));  //１番からスポット数の番号をランダムで取得

                //すでにランダムに取得した番号がどこかのスポットとして割り当てられていないかitemsPositionNumber配列の状況を全点検
                foreach (int numbers in itemsPositionNumber)
                {
                    //取り出した情報とランダム番号が一致していたら重複したということになる
                    if (numbers == rand)
                    {
                        unique = false; //唯一のユニークなものではない
                        break;
                    }
                }
            }
            while (!unique);

            //スポットの全チェック（ランダム値とスポット番号の一致）
            //一致していれば、そこにアイテムを生成
            foreach (GameObject spots in itemSpots)
            {
                if (spots.GetComponent<ItemSpot>().spotNum == rand) //RoomManagerのInspectorにあるItemsの数だけ生成を繰り返す
                {
                    GameObject obj = Instantiate(
                    items[i],
                    spots.transform.position,
                    Quaternion.identity
                    );
                    //どのスポット番号がどのアイテムに割り当てられているのかを記録
                    itemsPositionNumber[i] = rand;

                    //生成したアイテムに識別番号を割り振っていく
                    if (obj.CompareTag("Bill"))
                    {
                        obj.GetComponent<BillData>().itemNum = i;
                    }
                    else
                    {
                        obj.GetComponent<DrinkData>().itemNum = i;
                    }
                }
            }
        }
    }

    void StartDoorsPosition()
    {
        //全スポットの取得
        GameObject[] roomSpots = GameObject.FindGameObjectsWithTag("RoomSpot");

        //出入り口(鍵１〜鍵３の３つの出入り口)の分だけ繰り返し
        for (int i = 0; i < doorsPositionNumber.Length; i++) //0から２まで
        {
            int rand;  //ランダムな数の受け皿
            bool unique;  //重複していないかのフラグ

            do
            {
                unique = true; //問題なければそのままループを抜ける予定
                rand = Random.Range(1, (roomSpots.Length + 1));  //１番からスポット数の番号をランダムで取得

                //すでにランダムに取得した番号がどこかのスポットとして割り当てられていないかdoosPositionNumber配列の状況を全点検
                foreach (int numbers in doorsPositionNumber)
                {
                    //取り出した情報とランダム番号が一致していたら重複したということになる
                    if (numbers == rand)
                    {
                        unique = false; //唯一のユニークなものではない
                        break;
                    }
                }
            }
            while (!unique);

            //全スポットを見回りしてrandと同じスポットを探す
            foreach (GameObject spots in roomSpots)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == rand)
                {
                    //ルームを生成する
                    GameObject obj = Instantiate(room, spots.transform.position, Quaternion.identity);
                    //何番スポットが選ばれたのかStatic変数に記憶していく
                    doorsPositionNumber[i] = rand;
                    //生成したドアのセッティング
                    DoorSetting(
                        obj, //対象オブジェクト
                        "fromRoom" + (i + 1), //生成したドアの識別名
                        "Room" + (i + 1), //そこの出入り口に触れたときどこにいくのか
                        "Main", //行き先となるシーン名
                        false,  //ドアの開錠の状況
                        DoorDirection.down,  //この出入り口に戻った時のプレイヤーの配置
                        messages[i]);
                }
            }
        }
    }

    //生成したドアのセッティング
    void DoorSetting(GameObject obj, string roomName, string nextRoomName, string sceneName, bool openedDoor, DoorDirection direction, MessageData message)
    {
        RoomData roomData = obj.GetComponent<RoomData>();
        //第一引数に指定したオブジェクトのRoomDataスクリプトの各変数に第二引数以降で指定した値を代入
        roomData.roomName = roomName;
        roomData.nextRoomName = nextRoomName;
        roomData.nextScene = sceneName;
        roomData.openedDoor = openedDoor;
        roomData.direction = direction;
        roomData.message = message;

        roomData.DoorOpenCheck(); //ドアの開閉状況フラグを見てドアを表示/非表示メソッド
    }

    void LoadKeysPosition()
    {
        //Key1が未取得だったら
        if (!GameManager.keysPickedState[0])
        {
            //全Keyスポットの取得
            GameObject[] keySpots = GameObject.FindGameObjectsWithTag("KeySpot");

            //全スポットを順番に点検
            foreach (GameObject spots in keySpots)
            {
                if (spots.GetComponent<KeySpot>().spotNum == key1PositionNumber)
                {
                    //Key1の生成
                    Instantiate(key, spots.transform.position, Quaternion.identity);
                }
            }
        }

        //Key2が未取得だったら
        if (!GameManager.keysPickedState[1])
        {
            //Key2スポットの取得
            GameObject keySpot2 = GameObject.FindGameObjectWithTag("KeySpot2");
            GameObject obj = Instantiate(key, keySpot2.transform.position, Quaternion.identity);
            //生成したKeyのタイプを変えておく
            obj.GetComponent<KeyData>().keyType = KeyType.key2;
        }
        //Key3が未取得だったら
        if (!GameManager.keysPickedState[2])
        {
            //Key3スポットの取得
            GameObject keySpot3 = GameObject.FindGameObjectWithTag("KeySpot3");
            GameObject obj = Instantiate(key, keySpot3.transform.position, Quaternion.identity);
            //生成したKeyのタイプを変えておく
            obj.GetComponent<KeyData>().keyType = KeyType.key3;
        }

    }
    void LoadItemsPosition()
    {
        //全部のアイテムスポットを取得
        GameObject[] itemSpots = GameObject.FindGameObjectsWithTag("ItemSpot");

        for (int i = 0; i < items.Length; i++)
        {
            if (!GameManager.itemsPickedState[i]) //まだ未取得であれば再配置。取得済みのものは行わない
            {
                //スポットの全チェック（ランダム値とスポット番号の一致）
                //一致していれば、そこにアイテムを生成
                foreach (GameObject spots in itemSpots)
                {
                    if (spots.GetComponent<ItemSpot>().spotNum == itemsPositionNumber[i])
                    {
                        GameObject obj = Instantiate(
                        items[i],
                        spots.transform.position,
                        Quaternion.identity
                        );

                        //生成したアイテムに識別番号を割り振っていく
                        if (obj.CompareTag("Bill"))
                        {
                            obj.GetComponent<BillData>().itemNum = i;
                        }
                        else
                        {
                            obj.GetComponent<DrinkData>().itemNum = i;
                        }
                    }
                }
            }
        }
    }
    void LoadDoorsPosition()
    {
         //全スポットの取得
        GameObject[] roomSpots = GameObject.FindGameObjectsWithTag("RoomSpot");

        //出入り口(鍵１〜鍵３の３つの出入り口)の分だけ繰り返し
        for (int i = 0; i < doorsPositionNumber.Length; i++) //0から２まで
        {
            //全スポットを見回りしてrandと同じスポットを探す
            foreach (GameObject spots in roomSpots)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == doorsPositionNumber[i])
                {
                    //ルームを生成する
                    GameObject obj = Instantiate(room, spots.transform.position, Quaternion.identity);

                    //生成したドアのセッティング
                    DoorSetting(
                        obj, //対象オブジェクト
                        "fromRoom" + (i + 1), //生成したドアの識別名
                        "Room" + (i + 1), //そこの出入り口に触れたときどこにいくのか
                        "Main", //行き先となるシーン名
                        GameManager.doorsOpenedState[i],  //ドアの開錠の状況をStaticから読み取る
                        DoorDirection.down,  //この出入り口に戻った時のプレイヤーの配置
                        messages[i]);
                }
            }
        }
    }
}
