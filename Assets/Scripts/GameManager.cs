using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

//ゲーム状態を管理する列挙型
public enum GameState
{
    playing,    // プレイ中
    talk,       // トーク中
    gameover,   // ゲームオーバー
    gameclear,  // ゲームクリア
    ending      // エンディング
}

public class GameManager : MonoBehaviour
{
    public static GameState gameState; //ゲームのステータス
    public static bool[] doorOpenedState={false,false,false}; //ドアの開閉状況
    public static int key1;
    public static int key2;
    public static int key3;
    public static bool[] keysPickedState={false,false,false}; //鍵の取得状況

    public static int bill = 10; //お札の残数
    public static bool[] itemsPickedState={false,false,false,false,false}; //アイテムの所持状況

    public static bool hasSpotLight; // スポットライトを所持しているかどうか。

    public static int playerHP = 3;  //プレイヤーのHP

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //まずはゲーム開始状態にする
        gameState = GameState.playing;
    }
}
