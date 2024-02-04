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
        value = value | (1 << valueToAdd.Index);
    }

    public bool ContainsValue(T valueToFind) 
    {
        return (value & (1 << valueToFind.Index)) != 0;
    }

    public void RemoveValue(T valueToRemove)
    {
        value = value & ~(1 << valueToRemove.Index);
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
