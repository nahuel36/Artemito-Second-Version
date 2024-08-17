using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class CustomEnumFlags
{
    public enum contentType
    { 
        variable,
        objectWithProperties
    }


    [SerializeField] private int value = 0;
    [SerializeField] List<VariableType> members;
    public List<VariableType.VariableData> variableData = new List<VariableType.VariableData>();

    public delegate void CustomEnumFlagsDelegate();
    public event CustomEnumFlagsDelegate OnValueChange;
    private EnumFlagsField enumfield;
    public EnumFlagsField Enumfield { 
        get 
        {
            return enumfield;    
        } 
        set 
        { 
            enumfield = value;
            LoadData();
        } }
    public contentType type;
    public CustomEnumFlags(int valueToSet)
    {
        SetIntValue(valueToSet);
    }

    public void LoadData()
    {
        if (members == null)
            members = new List<VariableType>();

        CheckContainsVariables();
        for (int i = 0; i < variableData.Count; i++)
        {
            for (int j = 0; j < members.Count; j++)
            {
                if (members[j].TypeName == variableData[i].variableType)
                    members[j].data = variableData[i];
            }
            
        }
    }

    public void SaveData()
    {
        CheckContainsVariables();
        variableData = new List<VariableType.VariableData>();
        for (int i = 0; i < members.Count; i++)
        {
            VariableType.VariableData data = members[i].data;
            data.variableType = members[i].TypeName;
            variableData.Add(data);
        }
    }

    public void AddValue(EnumerableType valueToAdd)
    {
        value = value | (1 << valueToAdd.Index);
    }

    public bool ContainsValue(EnumerableType valueToFind) 
    {
        return ContainsValue(valueToFind.Index);
    }


    private bool ContainsValue(int index)
    {
        return (value & (1 << index)) != 0;
    }

    public bool FieldContainsValue(string typename)
    {
        if (Enumfield != null)
        {            
            return Enumfield.choices.Contains(typename);
        }
        return false;
    }

    public void RemoveValue(EnumerableType valueToRemove)
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

        if (members == null)
            members = new List<VariableType>();

        CheckContainsVariables();

        OnValueChange?.Invoke();
    }

    private void CheckContainsVariables()
    {
        if (type == contentType.variable)
        {
            var variables = EnumerablesUtility.GetAllVariableTypes();
            for (int i = 0; i < variables.Length; i++)
            {
                if (FieldContainsValue(variables[i].TypeName))
                {
                    List<int> list = new List<int>(); 
                    bool contains = false;
                    for (int j = 0; j < members.Count; j++)
                    {
                        if (members[j] == null || string.IsNullOrEmpty(members[j].TypeName)||(contains == true && members[j].TypeName == variables[i].TypeName))
                        {
                            list.Add(j);
                        }
                        if (contains == false && (members[j] != null && members[j].TypeName == variables[i].TypeName))
                        {
                            contains = true;   
                        }
                    }

                    for (int k = list.Count-1; k >= 0 ; k--)
                    {
                        members.RemoveAt(list[k]);
                    }

                    if (contains == false && ContainsValue(variables[i]))
                    {
                        members.Add((VariableType)(variables[i].Copy()));
                    }
                }
                else
                {
                    if (members == null) return;
                    for (int j = members.Count-1; j >= 0; j--)
                    {
                        if (members[j] == null || members[j].TypeName == variables[i].TypeName)
                        {
                            members.RemoveAt(j);
                        }
                    }                        
                }
            }
        }
    }

    public void SetPropertyField(VariableType variable, VisualElement variableItemElement)
    {
        CheckContainsVariables();

        if (type == contentType.variable)
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (members[i] != null && members[i].GetType() == variable.GetType())
                {
                    ((VariableType)members[i]).saveData = SaveData;
                    ((VariableType)members[i]).SetPropertyField(variableItemElement);
                }
            }
        }
    }

    public void SetDefaultValue(VariableType variable, VisualElement variableItemElement)
    {
        CheckContainsVariables();

        if (type == contentType.variable)
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
    }

    public static void SetChoicesMasksByChoicesInOrder(List<int> choicesMasks, List<string> choices)
    {
        choicesMasks.Clear();

        for (int i = 0; i < choices.Count; i++)
        {
            choicesMasks.Add(1 << i);
        }
    }

    public CustomEnumFlags Copy()
    {
        CustomEnumFlags newEnum = new CustomEnumFlags(value);
        newEnum.members = new List<VariableType>();
        for (int i = 0; members !=null   && i < members.Count ; i++)
        {
            if (members[i])
                newEnum.members.Add((VariableType)members[i].Copy());
        }
        return newEnum;
    }

    internal void InitializeVariables()
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (type == contentType.variable)
                ((VariableType)members[i]).changedIngame = false;
        }
    }

    internal string GetStringValue(string type)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].TypeName == type)
            { 
                if(((VariableType)members[i]).data.isString)
                    return ((VariableType)members[i]).GetStringValue();
            }
        }
        return default(string);
    }

    internal UnityEngine.Object GetObjectValue(string type)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].TypeName == type)
            {
                if (((VariableType)members[i]).data.isString)
                    return ((VariableType)members[i]).GetObjectValue();
            }
        }
        return default(UnityEngine.Object);
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

    internal void SetOnChange(VariableType variable, Action onChange)
    {
                for (int i = 0; i < members.Count; i++)
                {
                    if (members[i] != null && members[i].GetType() == variable.GetType())
                    {
                        ((VariableType)members[i]).onChange = onChange;
                        
                    }
                }
    }
}

[System.Flags]
public enum GenericEnum
{
    value1 = 1 << 0,
    value2 = 1 << 1,
    value3 = 1 << 2,
    value4 = 1 << 3,
    value5 = 1 << 4,
    value6 = 1 << 5,
    value7 = 1 << 6,
    value8 = 1 << 7,
    value9 = 1 << 8,
    value10 = 1 << 9,
    value11 = 1 << 10,
    value12 = 1 << 11,
    value13 = 1 << 12,
    value14 = 1 << 13,
    value15 = 1 << 14,
    value16 = 1 << 15,
    value17 = 1 << 16,
    value18 = 1 << 17,
    value19 = 1 << 18,
    value20 = 1 << 19,
    value21 = 1 << 20,
    value22 = 1 << 21,
    value23 = 1 << 22,
    value24 = 1 << 23,
    value25 = 1 << 24,
    value26 = 1 << 25,
    value27 = 1 << 26,
    value28 = 1 << 27,
    value29 = 1 << 28,
    value30 = 1 << 29,
    value31 = 1 << 30,
    value32 = 1 << 31,
    value33 = 1 << 32,
    value34 = 1 << 33,
    value35 = 1 << 34,
    value36 = 1 << 35,
    value37 = 1 << 36,
    value38 = 1 << 37,
    value39 = 1 << 38,
    value40 = 1 << 39,
    value41 = 1 << 40,
    value42 = 1 << 41,
    value43 = 1 << 42,
    value44 = 1 << 43,
    value45 = 1 << 44,
    value46 = 1 << 45,
    value47 = 1 << 46,
    value48 = 1 << 47,
    value49 = 1 << 48,
    value50 = 1 << 49,
}
