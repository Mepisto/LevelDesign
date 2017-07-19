using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue
{
    private Queue<IMessage> m_que_Message = new Queue<IMessage>();

    public void Push(IMessage message)
    {
        m_que_Message.Enqueue(message);
    }

    public IMessage Pop()
    {
        if (m_que_Message.Count > 0)
        {
            return m_que_Message.Dequeue();
        }

        return null;
    }

    internal void BhvFixedUpdate(float delta)
    {
        
    }
}
