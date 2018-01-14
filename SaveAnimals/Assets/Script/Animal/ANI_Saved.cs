using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// 泡泡破裂邏輯於animation中
/// </summary>
public class ANI_Saved : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        AnimalController2 Control = Obj.GetComponent<AnimalController2>();
        Control.InBubble = false;
        Control.BubbleOff();

        Animator _animat = Obj.GetComponent<Animator>();
        _animat.SetTrigger("IsSaved");

        Respawn(3, Obj);
    }

    public async void Respawn(double _duration, GameObject _obj)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        MainTask.Singleton.AddTask(delegate
        {
            _obj.SetActive(false);
            ObserverSystem.share.Notify(_obj.name, AnimalState.Idle);
        });

        await Task.Delay(TimeSpan.FromSeconds(_duration));
        MainTask.Singleton.AddTask(delegate
        {            
            _obj.SetActive(true);
        });
    }
}
