using System;
using System.Threading.Tasks;
using UnityEngine;

public class ANI_Idle : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        
        Obj.transform.position = Obj.GetComponent<AnimalController2>().SpawnPos;
        Init(Obj);
    }

    public async void Init(GameObject _obj)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        MainTask.Singleton.AddTask(delegate
        {
            
            _obj.GetComponent<AnimalController2>().BubbleOff();
            _obj.SetActive(true);
        });
    }
}
