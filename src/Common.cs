using System;
using UnityEngine.Events;

namespace Chankiyu22.UnityCommander
{

[Serializable]
public class CommandUnityEvent : UnityEvent<Command> { }

public class CommandEventArgs : EventArgs
{
    public Command command;
}

}
