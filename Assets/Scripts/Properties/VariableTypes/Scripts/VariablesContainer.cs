using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class VariablesContainer 
{

    [SerializeField][HideInInspector]public List<EnumerableType> members;

    internal VariablesContainer Copy()
    {
        VariablesContainer newVar = new VariablesContainer();
        newVar.members = new List<EnumerableType>();
        for (int i = 0; i < members.Count; i++)
        {
            newVar.members.Add(members[i].Copy());
        }
        return newVar;
    }

    internal string GetStringValue(string type)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].TypeName == type)
            {
                if (((VariableType)members[i]).data.isString)
                    return ((VariableType)members[i]).GetStringValue();
            }
        }
        return default(string);
    }

    internal void SetValue<T2>(string type, T2 value)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].TypeName == type)
            {
                ((VariableType)members[i]).SetValue(value);
            }
        }
    }

    internal void InitializeVariables()
    {
        for (int i = 0; i < members.Count; i++)
        {
                ((VariableType)members[i]).changedIngame = false;
        }
    }
    public bool FieldContainsValue(string typename, EnumFlagsField enumfield, CustomEnumFlags enumflag, EnumerableType variable) 
    {
        if (enumfield != null)
        {
            return enumfield.choices.Contains(typename) && enumflag.ContainsValue(variable);
        }
        return false;
    }
    public void CheckContainsVariables(EnumFlagsField enumfield, CustomEnumFlags enumflag)
    {
            var variables = EnumerablesUtility.GetAllVariableTypes();
            for (int i = variables.Length-1; i >=0; i--)
            {
                if (FieldContainsValue(variables[i].TypeName, enumfield, enumflag, variables[i]))
                {
                        members.Add((VariableType)ScriptableObject.CreateInstance(variables[i].GetType()));
                }
                else
                {
                    int indexToRemove = GetMemberIndex(variables[i]);
                    if(indexToRemove >= 0)
                        members.RemoveAt(indexToRemove);
                }
            }
    }
    public void SetPropertyField(VariableType variable, VisualElement variableItemElement)
    {
        
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i] != null && members[i].GetType() == variable.GetType())
                ((VariableType)members[i]).SetPropertyField(variableItemElement);
        }
        
    }

    internal void SetOnChange(VariableType variable, Action onChange)
    {
        
    /*    for (int i = 0; i < members.Count; i++)
        {
            if (members[i] != null && members[i].GetType() == variable.GetType())
            {
                ((VariableType)members[i]).OnChange = onChangeAVariableContentValue;
            }
        }
      */  
    }

    internal UnityEngine.Object GetObjectValue(string type)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].TypeName == type)
            {
                if (!((VariableType)members[i]).data.isString)
                    return ((VariableType)members[i]).GetObjectValue();
            }
        }
        return default(UnityEngine.Object);
    }
    public void SetDefaultValue(VariableType variable, VisualElement variableItemElement)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i] != null && members[i].GetType() == variable.GetType())
            {
                variableItemElement.Q<Toggle>("Default").value = ((VariableType)members[i]).data.isDefaultValue;
                int index = i;
                if (((VariableType)members[i]).data.isDefaultValue)
                    variableItemElement.Q<VisualElement>("Value").visible = false;
                else
                    variableItemElement.Q<VisualElement>("Value").visible = true;
                variableItemElement.Q<Toggle>("Default").RegisterValueChangedCallback((newvalue) => {
                    int indexi = index;
                    ((VariableType)members[indexi]).data.isDefaultValue = newvalue.newValue;
                    if (((VariableType)members[indexi]).data.isDefaultValue)
                        variableItemElement.Q<VisualElement>("Value").visible = false;
                    else
                        variableItemElement.Q<VisualElement>("Value").visible = true;
                });
            }
        }

    }


    public bool ContainsValue<T>(T valueToFind) where T:EnumerableType
    {
        return ContainsValue(valueToFind.Index);
    }

    private bool ContainsValue(int index)
    {
        for (int i = 0; i < members.Count;i++) 
        {
            if (members[i].Index == index)
                return true;
        }
        return false;
    }

    private int GetMemberIndex(EnumerableType valueToFind)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].Index == valueToFind.Index)
                return i;
        }
        return -1;
    }
    internal void SetDefautlValue(string type)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].TypeName == type)
            {
                ((VariableType)members[i]).data.isDefaultValue = true;
            }
        }
    }
}
