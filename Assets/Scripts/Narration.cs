using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Narration : MonoBehaviour
{
    public MessageData message;
    public TextMeshProUGUI messageText;

    public bool isEnding; //エンディングシーンで使われるかどうか

    void Start()
    {
        StartCoroutine(TalkStart());
    }

    IEnumerator TalkStart()
    {
        //対象としたScriptbleObject(変数message)が扱っている配列msgArrayの数だけ繰り返す
        for (int i = 0; i < message.msgArray.Length; i++)
        {
            messageText.text = message.msgArray[i].message;

            //yield return new WaitForSeconds(0.1f); //0.1秒待つ
            yield return new WaitForSecondsRealtime(0.1f); //0.1秒待つ

            while (!Input.GetKeyDown(KeyCode.E))
            { //Eキーがおされるまで
                yield return null; //何もしない
            }
        }

        yield return new WaitForSeconds(3.0f); //３秒待って

        if (!isEnding)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            SceneManager.LoadScene("Title");
        }

        SceneManager.LoadScene("Main"); //メインシーンへ移動

    }


}
