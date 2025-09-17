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
    static public bool hasSpotLight = false; // スポットライトを所持しているかどうか。初期状態はfalse、所持していれば true

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
