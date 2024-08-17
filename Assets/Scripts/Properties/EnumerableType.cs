using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnumerableType : ScriptableObject
{
    
        [SerializeField] private int index;
        [SerializeField] protected string typeName;
    

    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    public string TypeName
    {
        get { return typeName; }
        set { typeName = value; }
    }

    public virtual EnumerableType Copy()
    {
        EnumerableType newcopy = new EnumerableType();
        newcopy.index = index;
        newcopy.typeName = typeName;
        return newcopy;
    }
}
