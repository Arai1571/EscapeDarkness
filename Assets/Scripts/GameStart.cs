using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void NewGame()
    {
        //ゲームの初期化
        PlayerPrefs.DeleteAll();
        RoomManager.isNewGame = true;   // ← 新規ゲーム扱いにする
        RoomManager.toRoomNumber = "fromRoom1"; // 初期値はそのままでOK
        SceneManager.LoadScene("Opening");
    }

    public void ContinueGame()
    {
        RoomManager.toRoomNumber = "SavePoint";
        //ゲームのローディング
        SaveData.LoadGameData();
        //コンティニュー時にHPを回復
        GameManager.playerHP = 3;
        SceneManager.LoadScene("Main");
    }

}
