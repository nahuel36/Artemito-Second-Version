using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;

public class VariableTypesUtility 
{
    public static VariableType[] GetAllVariableTypes()
    {
        List<string> files = FileUtils.GetFilesList(Application.dataPath + "/Scripts/VariableTypes/");
        int varIndex = 0;
        List<VariableType> list = new List<VariableType>();
        for (int i = 0; i < files.Count; i++)
        {
            VariableType var = AssetDatabase.LoadAssetAtPath<VariableType>("Assets/Scripts/VariableTypes/" + files[i]);
            if (var != null)
            {
                list.Add(var);
                varIndex++;
            }
        }
        return list.ToArray();
    }

    public static void ShowEnumFlagsField(VisualElement element, CustomEnumFlags<VariableType> variableTypes)
    {
        CustomEnumFlagsEditor<VariableType> customFlagsEditor = new CustomEnumFlagsEditor<VariableType>();

        VariableType[] list = GetAllVariableTypes();

        customFlagsEditor.SetChoices((i) => { return list[i].TypeName; }, list.Length);
        customFlagsEditor.SetChoicesMasksByChoicesInOrder();

        customFlagsEditor.Show(variableTypes,element);
    }
}
