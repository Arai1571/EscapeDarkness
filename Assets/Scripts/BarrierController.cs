using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public float deleteTime = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DeleteTime秒後に消滅
        Destroy(gameObject,deleteTime);
    }
}
