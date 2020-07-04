using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chankiyu22.UnityCommander
{

[CreateAssetMenu(menuName="Unity Commander/Command")]
public class Command : ScriptableObject
{
    [SerializeField]
    private string m_description = null;

    public string description
    {
        get
        {
            return m_description;
        }
    }
}

}
