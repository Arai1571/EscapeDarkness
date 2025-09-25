using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public float deleteTime = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.instance.SEPlay(SEType.Barrier); //バリアの音


        //DeleteTime秒後に消滅
        Destroy(gameObject, deleteTime);
    }
}
