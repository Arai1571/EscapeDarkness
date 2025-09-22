using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//プレイヤーが出てきた時の方向
public enum DoorDirection
{
    up,
    down
}

public class RoomData : MonoBehaviour
{
    public string roomName;  //出入り口の識別名
    public string nextRoomName;  //シーン切り替え先での行方
    public string nextScene;  //シーン切り替え
    public bool openedDoor; //ドアの開閉状況フラグ
    public DoorDirection direction; //プレイヤーの配置位置
    public MessageData message;  //
    public GameObject door;  //

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        //このRoomに触れたらどこに行くのかを変数next RoomNameで決めておく
        //シーンが切り替わって情報がリセットされる前に
        // static変数であるtoRoomNumberに行き先情報を記録
        RoomManager.toRoomNumber = nextRoomName;
        SceneManager.LoadScene(nextScene);
    }

    //ドアの開閉状況をチェックするメソッド
    public void DoorOpenCheck()
    {
        //もし開錠されていたら子オブジェクトである変数doorは非表示
        if (openedDoor) door.SetActive(false);
    }
}
