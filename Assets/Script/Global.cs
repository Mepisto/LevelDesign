using Lib.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Global : SingletonMonoBehaviour<Global>
{
    public static float time { get; private set; }

    private MessageQueue m_messageQueue = new MessageQueue();

    void Awake()
    {
        if (null != m_instance)
        {
            return;
        }
        else
        {
            m_instance = this;
        }

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        Application.targetFrameRate = 30;

        if ((Application.platform == RuntimePlatform.Android) || (Application.platform == RuntimePlatform.IPhonePlayer))
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (Application.isPlaying)
        {
            DontDestroyOnLoad(this);
        }
    }

    static public void PostMessage(IMessage message)
    {
        Instance.m_messageQueue.Push(message);
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        time += delta;
    }

    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        m_messageQueue.BhvFixedUpdate(delta);

        FixedMessageProcess(delta);
    }

    public void FixedMessageProcess(float dt)
    {
        float messageLoopTime = time;
        while (true)
        {
            IMessage message = m_messageQueue.Pop();
            uint msgCode = 0;
            if (message != null)
            {
                msgCode = message.MsgCode;
                bool succ = OnMessage(message);

                float currentTime = time;
                if (currentTime - messageLoopTime > dt)
                {
                    messageLoopTime = currentTime;
                }
                message.Dispose();
            }
            else
            {
                break;
            }
        }
    }

    // TODO Mepi : 임시.
    public static void InstantiateEnemy(UnityEngine.Object enemy, Vector3 pos, Quaternion rot)
    {
        var enemyGO = (GameObject)Instantiate(enemy, pos, rot);
    }

    public bool OnMessage(IMessage message)
    {
        Client.OnMessage(message);
                
        return true;
    }
}
