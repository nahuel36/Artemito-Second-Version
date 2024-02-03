using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEnumFlags<T> where T : EnumerableType
{
    private int value = 0;
    private T[] variables;

    public CustomEnumFlags(T[] variablesToSet, params T[] variablesToAdd)
    {
        variables = variablesToSet;
        for (int i = 0; i < variables.Length; i++)
        {
            variables[i].EnumIndex = 1 << i;
        }
        for (int i = 0; i < variablesToAdd.Length; i++)
        {
            AddValue(variablesToAdd[i]);
        }
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
