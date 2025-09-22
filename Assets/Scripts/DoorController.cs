using System.Collections;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public RoomData roomData;  //親オブジェクトが持っているスクリプトを取得
    MessageData message;       //親オブジェクトの持つScriptableObject情報を取得

    bool isPlayerInRange;           //プレイヤーが領域に入ったかどうか
    bool isTalk;                  //トークが開始されたかどうか
    GameObject canvas;            //トークUIを含んだCanvasオブジェクト
    GameObject talkPanel;         //対象となるトークUIパネル
    TextMeshProUGUI nameText;     //対象となるトークUIパネルの名前
    TextMeshProUGUI messageText;  //対象となるトークUIパネルのメッセージ

    void Start()
    {
        message = roomData.message; //トークデータは親オブジェクトのスクリプトにある変数を参照

        canvas = GameObject.FindGameObjectWithTag("Canvas");
        talkPanel = canvas.transform.Find("TalkPanel").gameObject;
        nameText = talkPanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        messageText = talkPanel.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //ドアの領域内にいる　かつ　トーク中でない　かつ　Eキーが押されたら
        if (isPlayerInRange && !isTalk && Input.GetKeyDown(KeyCode.E))
        {
            //トークの始まり
            StartConversation();
        }
    }

    void StartConversation()
    {
        isTalk = true; //トーク中フラグがon
        GameManager.gameState = GameState.talk;  //ゲームステータスがTalk
        talkPanel.SetActive(true);  //トークUIを表示
        nameText.text = message.msgArray[0].name;  //親オブジェクトから取得したmessageの配列の先頭の名前を表示
        messageText.text = message.msgArray[0].message; //親オブジェクトから取得したmessageの入れるの先頭のメッセージを表示
        Time.timeScale = 0;
        StartCoroutine(TalkProcess()); //TalkProcess子ルーチンの発動
    }

    //TalkProcessコルーチンの設計
    IEnumerator TalkProcess()
    {
        //フラッシュ入力阻止のため、少し処理を止める
        yield return new WaitForSecondsRealtime(0.1f);

        //Eキーが押されるまで
        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;  //Eキーが押されるまで何もしない
        }

        bool nextTalk = false;  //トークを更に展開するかどうか

        switch (roomData.roomName)
        {
            case "fromRoom1":
                if (GameManager.key1 > 0) //該当する鍵を持っていたら
                {
                    GameManager.key1--;   //鍵の消耗
                    nextTalk = true;      //次のトーク展開をさせる
                    GameManager.doorsOpenedState[0] = true;  //記録用の施錠状況をtrue
                }
                break;
            case "fromRoom2":
                if (GameManager.key2 > 0) //該当する鍵を持っていたら
                {
                    GameManager.key2--;   //鍵の消耗
                    nextTalk = true;      //次のトーク展開をさせる
                    GameManager.doorsOpenedState[1] = true;  //記録用の施錠状況をtrue
                }
                break;
            case "fromRoom3":
                if (GameManager.key3 > 0) //該当する鍵を持っていたら
                {
                    GameManager.key3--;   //鍵の消耗
                    nextTalk = true;      //次のトーク展開をさせる
                    GameManager.doorsOpenedState[2] = true;  //記録用の施錠状況をtrue
                }
                break;
        }

        if (nextTalk)
        {
            nameText.text = message.msgArray[1].name;
            messageText.text = message.msgArray[1].message;

            yield return new WaitForSecondsRealtime(0.1f);

            while(!Input.GetKeyDown(KeyCode.E))
            {
                yield return null;
            }

            roomData.openedDoor = true;
            roomData.DoorOpenCheck();
        }

        EndConversation();
    }

    void EndConversation()
    {
        talkPanel.SetActive(false);  //トークUIを非表示
        GameManager.gameState = GameState.playing;
        isTalk = false; //トーク中フラグをOFF
        Time.timeScale = 1.0f; //ゲーム進行を元に戻す
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーが領域に入ったら
        if (collision.gameObject.CompareTag("Player"))
        {
            //フラグがON
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤーが領域からでたら
        if (collision.gameObject.CompareTag("Player"))
        {
            //フラグがOFF
            isPlayerInRange = false;
        }
    }
}
