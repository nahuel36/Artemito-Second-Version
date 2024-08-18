using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

public class EnumerablesUtility 
{
    public static VariableType[] GetAllVariableTypes()
    {
#if UNITY_EDITOR
        List<string> files = FileUtils.GetFilesList(Application.dataPath + "/Scripts/Properties/VariableTypes/");
        int varIndex = 0;
        List<VariableType> list = new List<VariableType>();
        for (int i = 0; i < files.Count; i++)
        {
            VariableType var = AssetDatabase.LoadAssetAtPath<VariableType>("Assets/Scripts/Properties/VariableTypes/" + files[i]);
            if (var != null)
            {
                list.Add(var);
                varIndex++;
            }
        }
        list.Sort((x, y) => x.Index.CompareTo(y.Index));
        return list.ToArray();
#else
        return null;
#endif
    }

    public static PropertyObjectType[] GetAllPropertyObjectTypes()
    {
#if UNITY_EDITOR
        List<string> files = FileUtils.GetFilesList(Application.dataPath + "/Scripts/Properties/PropertyObjectType/");
        int varIndex = 0;
        List<PropertyObjectType> list = new List<PropertyObjectType>();
        for (int i = 0; i < files.Count; i++)
        {
            PropertyObjectType var = AssetDatabase.LoadAssetAtPath<PropertyObjectType>("Assets/Scripts/Properties/PropertyObjectType/" + files[i]);
            if (var != null)
            {
                list.Add(var);
                varIndex++;
            }
        }
        list.Sort((x, y) => x.Index.CompareTo(y.Index));
        return list.ToArray();
#else
        return null;
#endif
    }

    public static void ShowEnumFlagsField(string enumFlagsContainerName, VisualElement element, CustomEnumFlags variableTypes, Action onChangeAction = null, CustomEnumFlags[] flagsToCompare = null) 
    {
        CustomEnumFlagsEditor customFlagsEditor = new CustomEnumFlagsEditor();

        EnumerableType[] list2 = null;
        if (variableTypes.type == CustomEnumFlags.contentType.objectWithProperties)
        {
            PropertyObjectType[] list = GetAllPropertyObjectTypes();
            list2 = new PropertyObjectType[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                list2[i] = ScriptableObject.CreateInstance<PropertyObjectType>();
                list2[i].Index = list[i].Index;
                list2[i].TypeName = list[i].TypeName;
                //aca deberia haber un copy
            }
        }
        if (variableTypes.type == CustomEnumFlags.contentType.variable)
        {
            VariableType[] list = GetAllVariableTypes();
            list2 = new VariableType[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                list2[i] = ScriptableObject.CreateInstance<VariableType>();
                list2[i].Index = list[i].Index;
                list2[i].TypeName = list[i].TypeName;
                //aca deberia haber un copy
            }
        }

        customFlagsEditor.SetChoices((i) => {
            bool flagsToCompareBoolean = true;
            
            if (flagsToCompare != null)
            {
                for (int j = 0; j < flagsToCompare.Length; j++)
                {
                    if (flagsToCompare[j] != null && !(flagsToCompare[j].ContainsValue(list2[i])))
                    {
                        flagsToCompareBoolean = false;
                    }
                }
            }

            if (flagsToCompareBoolean)
                return list2[i].TypeName;
            else
                return null;
        }, list2.Length);
        CustomEnumFlags.SetChoicesMasksByChoicesInOrder(customFlagsEditor.choicesMasks, customFlagsEditor.choices);

        customFlagsEditor.Show(enumFlagsContainerName,variableTypes, element, onChangeAction);
    }

    public static void ShowDropdownField(string propertyValue, DropdownField dropdownField, Action UpdateFieldAction)
    {
        PropertyObjectType[] props = EnumerablesUtility.GetAllPropertyObjectTypes();

        dropdownField.choices = new List<string>();
        dropdownField.value = propertyValue;

        for (int i = 0; i < props.Length; i++)
        {
            dropdownField.choices.Add(props[i].TypeName);
        }
        dropdownField.RegisterValueChangedCallback((value) => 
        {
            dropdownField.value = value.newValue;
            UpdateFieldAction?.Invoke();

        });
        UpdateFieldAction?.Invoke();;        
    }

    

    public static void UpdateAllVariablesFields(VisualElement element, CustomEnumFlags variablesContainer, Action onChange = null)
    {
        VisualElement variablesContainerVE = element.Q<VisualElement>("VariablesContainer");
        variablesContainerVE.Clear();

        foreach (var variable in EnumerablesUtility.GetAllVariableTypes())
        {
            if (variablesContainer.ContainsValueByFields(variable) && variablesContainer.FieldContainsValue(variable.TypeName))
            {
                //VisualElement variableItemElement = variableItem.CloneTree();
                VisualElement variableItemElement = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Properties/Editor/VariableField.uxml").CloneTree();
                variableItemElement.Q<VisualElement>("Value").Q<Label>("Label").text = variable.TypeName;
                variablesContainer.SetPropertyField(variable, variableItemElement);
                variablesContainer.SetOnChange(variable, onChange);
                variablesContainer.SetDefaultValue(variable, variableItemElement);
                variablesContainerVE.Add(variableItemElement);
            }
        }
    }
}
