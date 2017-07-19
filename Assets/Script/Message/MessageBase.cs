using System;



public interface IMessage : IDisposable
{
    uint MsgCode { get; }
    bool IsDeliveryMsg { get; }
    bool IsNetMsg { get; }
    eMessage MsgType { get; }
}

public abstract class MessageBase : IMessage
{
    public virtual bool IsDeliveryMsg
    {
        get { return false; }
    }

    public uint MsgCode
    {
        get { return (uint)MsgType; }
    }

    public virtual bool IsNetMsg
    {
        get { return false; }
    }

    public abstract eMessage MsgType { get; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public override string ToString()
    {
        return string.Format("{0} DeliveryMsg {1} IsNetMsg {2}", this.GetType().Name, IsDeliveryMsg, IsNetMsg);
    }
}

public class TriggerEnterInfo
{
    uint Id;

    public TriggerEnterInfo(uint id)
    {
        Id = id;
    }

    public uint ID
    {
        get { return Id; }
    }
}

public class TriggerExitInfo
{
    public TriggerExitInfo()
    {
    }
}

public class TriggerEnterMsg : MessageBase
{
    public TriggerEnterInfo EnterInfo { get; set; }

    public override eMessage MsgType
    {
        get { return eMessage.TriggerEnter; }
    }
}

public class TriggerExitMsg : MessageBase
{
    public TriggerExitInfo ExitInfo { get; set; }

    public override eMessage MsgType
    {
        get { return eMessage.TriggerExit; }
    }
}