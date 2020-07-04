using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Chankiyu22.UnityCommander
{

[Serializable]
class CommandAction
{
    [SerializeField]
    private Command m_command = null;

    public Command command
    {
        get
        {
            return m_command;
        }
    }

    [SerializeField]
    private CommandUnityEvent m_actions = null;

    public CommandUnityEvent actions
    {
        get
        {
            return m_actions;
        }
    }
}

public class CommandDispatcher : MonoBehaviour
{
    [SerializeField]
    private List<CommandAction> m_commandActions = new List<CommandAction>();

    private Dictionary<Command, CommandAction> m_commandActionMap = new Dictionary<Command, CommandAction>();

    public event EventHandler OnDiapatch;

    void Awake()
    {
        foreach (CommandAction commandAction in m_commandActions)
        {
            if (commandAction.command != null)
            {
                m_commandActionMap.Add(commandAction.command, commandAction);
            }
        }
    }

    public void DispatchCommand(Command command)
    {
        if (m_commandActionMap.ContainsKey(command))
        {
            m_commandActionMap[command].actions.Invoke(command);

            if (OnDiapatch != null)
            {
                OnDiapatch.Invoke(this, new CommandEventArgs() {
                    command = command
                });
            }
        }
    }
}

}
