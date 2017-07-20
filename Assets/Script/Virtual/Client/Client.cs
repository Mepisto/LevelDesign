using Lib.Pattern;

using System.Collections.Generic;
using UnityEngine;
using Orca.Contents.LevelDesign;

public class Client : SingletonMonoBehaviour<Client>
{
    public List<LdSpawner> Spawners = new List<LdSpawner>();

    public List<LdTrigger> Triggers = new List<LdTrigger>();

    public GameObject player;

    private void Awake()
    {
        if (null != m_instance)
        {
            return;
        }
        else
        {
            m_instance = this;
        }
    }

    private void Start()
    {
        foreach (var spawner in Instance.Spawners)
        {
            spawner.Init();
        }

        foreach (var trigger in Instance.Triggers)
        {
            trigger.Init();
        }
    }

    public static void OnMessage(IMessage message)
    {
        if (message is TriggerEnterMsg)
        {
            var msg = message as TriggerEnterMsg;

            Debug.LogError(message.MsgType + " : " + msg.EnterInfo.ID);

            foreach (var spawner in Instance.Spawners)
            {
                if (spawner.IsValidID(msg.EnterInfo.ID))
                {
                    spawner.Active(true);
                    spawner.Spawn();
                    break;
                }
            }

            foreach (var trigger in Instance.Triggers)
            {
                if (trigger.IsValidID(msg.EnterInfo.ID))
                {
                    trigger.Active(false);
                    break;
                }
            }
        }
    }
}
