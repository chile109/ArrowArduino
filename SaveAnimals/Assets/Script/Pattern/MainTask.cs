using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MainTask : MonoBehaviour
{
    public static MainTask Singleton { get; set; }
    Queue<Action> mThreadTaskOnMain = new Queue<Action>();
    void Awake()
    {
        // 確保不死
        DontDestroyOnLoad(this);
        Singleton = this;
    }
    void Update()
    {
        // 記得先 lock 住核心資料(常用作法)
        lock (mThreadTaskOnMain)
        {
            if (mThreadTaskOnMain.Count < 1)
                return;
            // 取出
            var task = mThreadTaskOnMain.Dequeue();
            // 執行
            task();
        }
    }
    // 通常是給非主執行緒的地方呼叫, 當然這種作法也可以在主執行緒委託呼叫
    public void AddTask(Action task)
    {
        // 記得先 lock 住核心資料(常用作法)
        lock (mThreadTaskOnMain)
        {
            mThreadTaskOnMain.Enqueue(task);
        }
    }
}

