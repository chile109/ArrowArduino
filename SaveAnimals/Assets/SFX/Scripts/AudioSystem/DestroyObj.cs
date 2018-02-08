using UnityEngine;
using System.Collections;

public class DestroyObj : MonoBehaviour
{
    public float DestroyTime;

    public DestroyObj(float DestroyTime)
    {
        this.DestroyTime = DestroyTime;
    }
    
    // Use this for initialization
    void Start()
    {
        StartCoroutine(DelayTime(DestroyTime));
    }

    IEnumerator DelayTime(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        AudioManager.inst.DelAudioObj(gameObject);
        Destroy(gameObject);
    }
}	  
