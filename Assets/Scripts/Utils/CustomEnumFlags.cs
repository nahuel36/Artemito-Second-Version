using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomEnumFlags<T> where T : EnumerableType
{
    [SerializeField][HideInInspector]private int value = 0;
    public CustomEnumFlags(int valueToSet)
    {
        SetIntValue(valueToSet);
    }

    public void AddValue(T valueToAdd)
    {
        value = value | valueToAdd.EnumIndex;
    }

    public bool ContainsValue(T valueToFind) 
    {
        return (value & valueToFind.EnumIndex) != 0;
    }

    public void RemoveValue(T valueToRemove)
    {
        value = value & ~(valueToRemove.EnumIndex);
    }

    public int GetIntValue()
    {
        return value;
    }
    public void SetIntValue(int valueToSet)
    {
        value = valueToSet;
    }
}
