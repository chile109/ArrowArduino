using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class AnimalController : MonoBehaviour
{

    public Animal _ani = new Animal();
    public StateMachine _FSM;
    public bool isCatched = false;

    public float MoveSpeed = 2.0f;
    public float FleeSpeed = 5.0f;

    public Target m_target = Target.Right;
    public Vector3 m_targetPos;
    Vector3 InitPos;

    void Start()
    {
        _FSM = new StateMachine();
        _FSM.NowState = _ani.idle;
        InitPos = Camera.main.WorldToViewportPoint(this.transform.position);

        BossCome(5);
    }

    void Update()
    {
        _FSM.NowState.StateDoing(this.gameObject);
    }

    public void SetTarget()
    {

        Vector3 scale = this.transform.localScale;
        scale.x = Math.Abs(scale.x) * (m_target == Target.Right ? 1 : -1);
        this.transform.localScale = scale;

        float cameraZ = Camera.main.transform.position.z;
        m_targetPos = Camera.main.ViewportToWorldPoint(new Vector3((int)m_target, InitPos.y, -cameraZ));
        Debug.Log("targetPos:" + m_targetPos);
    }

    public async void BossCome(double _duration)
    {
        Debug.Log("Waiting " + _duration + " second...");
        await Task.Delay(TimeSpan.FromSeconds(_duration));
        //_FSM.NowState = _ani.flee;
        Debug.Log("Done!");
    }

    public delegate void voidDelegate(AnimalController Ani_Control);
    public voidDelegate OnArrested;

    public static AnimalController Create(GameObject prefab, Target target, Vector3 pos)
    {
        if (pos.x < prefab.transform.position.x)
        {
            Vector3 scale = prefab.transform.localScale;
            scale.x = -1;
            prefab.transform.localScale = scale;
        }
        GameObject inst = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        AnimalController _animal = inst.AddComponent<AnimalController>();
        _animal.Init(target);

        return _animal;
    }

    void Init(Target target)
    {
        m_target = target;
    }

    void BecomeBubble()
    {
        OnArrested(this);
    }
}

public class Animal
{
    public ANI_Idle idle = new ANI_Idle();
    public ANI_Traped Traped = new ANI_Traped();
    public ANI_Saved Saved = new ANI_Saved();
    public ANI_Cheering Cheering = new ANI_Cheering();
    public ANI_Cry Cry = new ANI_Cry();
}
public enum AnimalState
{
    Idle,
    Help,
    Saved,
    Success,
    Fail,
}

