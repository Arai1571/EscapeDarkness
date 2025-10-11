# Escape Darkness
## Escape Darknessについて
Escape Darkness は、暗闇の館を探索する 2DのRPG。
限られた視界の中で、鍵を見つけて該当するドアを開け、奥に潜むボスの間を目指します。
追跡してくる敵を避けながら、恐怖と緊張の中を生き延びましょう。

## ゲームプレイ方法
[ゲームのサンプルプレイ]( https://arai1571.github.io/EscapeDarkness_web/)

<<<<<<< HEAD
![ゲーム画面](ReadmeImg/main.png)
=======
![ゲーム画面](ReadmeImg/main.png)

### 操作方法
* Eキー：次へ進む、落ちているノートを読む
* A/Dキー（← →キー）：左右に移動
* W/Sキー（↑ ↓キー）:上下に移動
* スペースキー：お札を飛ばす
  
### ゲームルール
* 敵やボスの攻撃を受けて HP が 0 になると ゲームオーバー。
* 敵は一定距離まで近づくとプレイヤーを追跡してくる。スピードはプレイヤーより遅いので、距離を取れば逃げ切れる。
* お札攻撃 により、敵を一定時間 足止め可能。
* ドリンクアイテムを拾うと HP が 1 回復（最大3）。
* 各部屋にはランダムに配置される 鍵とドア が存在。対応する鍵を見つけて進もう。
* 最奥には ボスルーム があり、5回お札の結界に引き入れると撃破。
* ボス直前の部屋では セーブポイント があり、何度でも挑戦可能。

## 使用技術
* ゲームエンジン：Unity
* 使用言語：C#
* 使用ツール：VisualStudioCode, Illustrator

## 開発の工夫
* 開発期間：18時間
* 担当範囲：企画〜アセットの導入、プログラミング、デバック仕上げ
* こだわった点：
  部屋生成・鍵配置をランダム化し、毎回異なる探索体験を実現
  暗闇表現とスポットライトでホラー感を演出
  シーン遷移時にプレイヤーの座標を正確に引き継ぐRoomManager設計
  
* 技術的な挑戦：
Playerの角度を変えると、子オブジェクトのSpotLightも向きをQuaternion.Eulerで変え、Quaternion.Slerpで滑らかに補正した。

### スクリプトの詳細
* PlayerController.cs
  CharacterControllerコンポーネントのMoveメソッドを用いて自動走行する。
  ダメージを食らった際に一定時間を点滅処理するようにした。
'''C#
 //点滅処理
   void Blinking()
   {
       //その時のゲーム進行時間で正か負かの値を算出
       float val = Mathf.Sin(Time.time * 50);
       //正の周期なら表示
       if (val >= 0) body.SetActive(true);
       //負の周期なら非表示
       else body.SetActive(false);
   }
'''

* CameraRotation.cs
マウスの動きに連動してカメラの視点が変わるように実装した
最大・最小の視野角を決めてそこで視点が止まるようにClampメソッドを活用した
```C#
void Update()
{
    //プレイ状態でなければ動かせないようにしておく
    if (GameManager.gameState != GameState.playing) return;
    //マウスの動きを取得しておく
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
    //その時のマウスの動きに応じた数値（横方向）
    horizontalRotation += mouseX;
    //最大・最小に絞り込みはされる
    horizontalRotation = Mathf.Clamp(horizontalRotation, minHorizontalAngle, maxHorizontalAngle);
    //その時のマウスの動きに応じた数値（縦方向）
    verticalRotation -= mouseY;
    //最大・最小に絞り込みはされる
    verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
    //横の角度の微調整
    //基準としている角度に対してmin～maxの間でのマウス移動の積み重ね
    float yRotation = initialY + horizontalRotation;
    //そのフレームにおけるカメラの角度を最終決定
    transform.rotation = Quaternion.Euler(verticalRotation, yRotation, 0);
}
```
* StageGenerator.cs
プレハブに登録したステージオブジェクトが変数に設定した値で自動生成される。
Playerの位置に応じて新しいステージが生成されたのち、古いステージは削除されヒエラルキーを圧迫しないよう工夫した。
```C#
// 指定のインデックス位置にStageオブジェクトをランダムに生成
GameObject GenerateStage(int chipIndex)
{
    int nextStageChip = Random.Range(0, stageChips.Length);
    GameObject stageObject = Instantiate(
        stageChips[nextStageChip],
        new Vector3(0, 0, chipIndex * StageChipSize),
        Quaternion.identity
    );
    return stageObject;
}
```
```C#
    // 一番古いステージを削除
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
```
* Shooter.cs
カメラの向きに合わせて弾プレハブがシュートされる。
残数を決めて、コルーチンで弾数が自動回復するようにした。
```C#
//shotPowerを消費
void ConsumePower()
{
    shotPower--; //消費
    StartCoroutine(RecoverPower()); //回復コルーチン
}
//回復コルーチン
IEnumerator RecoverPower()
{
    //RecoverySeconds秒待つ
    yield return new WaitForSeconds(recoverySeconds);
    shotPower++; //１つ回復
}
```
## 今後の展望
* 難易度や楽しさの調整（敵の種類を増やす・攻撃アイテムのバリエーションを増やす）
* ボイス・環境音の追加による没入感の強化
* 地図上を通過したら自動でマップが作成されていくマップの実装
* 複数エンディング分岐




>>>>>>> 4d63bf0d2bfbc2324d9aea5f4005dda13da384f1


