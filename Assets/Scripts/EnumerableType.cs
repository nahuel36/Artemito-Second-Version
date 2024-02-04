using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnumerableType : ScriptableObject
{
    [SerializeField][HideInInspector] private int enumIndex;
    protected string typeName;

    public int EnumIndex
    {
        get { return enumIndex; }
        set { enumIndex = value; }
    }

    public string TypeName
    {
        get { return typeName; }
    }
}
