using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnumerableType : ScriptableObject
{
    private int enumIndex;
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
