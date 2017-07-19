using Lib.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : SingletonMonoBehaviour<Server>
{
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

    IMessage Message;
    public static void RequestMsg(IMessage msg)
    {
        Instance.Message = msg;
        
        ResponseMsg();
    }

    private static void ResponseMsg()
    {
        Global.PostMessage(Instance.Message);

        Instance.Message = null;
    }
}
