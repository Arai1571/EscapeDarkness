using TMPro;
using UnityEngine;

public class TalkController : MonoBehaviour
{

    public MessageData message;   //ScriptableObject情報
    bool isPlayerRange;           //プレイヤーが領域に入ったかどうか
    bool isTalk;                  //トークが開始されたかどうか
    GameObject canvas;            //トークUIを含んだCanvasオブジェクト
    GameObject talkPanel;         //対象となるトークUIパネル
    TextMeshProUGUI nameText;     //対象となるトークUIパネルの名前
    TextMeshProUGUI messageText;  //対象となるトークUIパネルのメッセージ

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
