using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(PropertiesContainer))]
public class PropertiesContainerCustomEditor : Editor
{
    [SerializeField] VisualTreeAsset generalTree;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        root.Add(generalTree.CloneTree());

        LocalAndGlobalProperties properties = new LocalAndGlobalProperties();

        properties.CreateGUI(((PropertiesContainer)target).local_properties, root.Q("LocalAndGlobalProperties"));

        return root;
    }

    public VisualElement EnumFlagsTest()
    {
        CustomEnumFlagsEditor<VariableType> customFlagsEditor = new CustomEnumFlagsEditor<VariableType>();

        VisualElement root = new VisualElement();

        List<string> files = FileUtils.GetFilesList(Application.dataPath + "/Scripts/VariableTypes/");
        List<VariableType> list = new List<VariableType>();

        customFlagsEditor.choices.Clear();
        customFlagsEditor.choicesMasks.Clear();

        int varIndex = 0;
        for (int i = 0; i < files.Count; i++)
        {
            VariableType var2 = AssetDatabase.LoadAssetAtPath<VariableType>("Assets/Scripts/VariableTypes/" + files[i]);
            if (var2 != null)
            {
                list.Add(var2);
                customFlagsEditor.choices.Add(var2.TypeName);
                customFlagsEditor.choicesMasks.Add(1 << varIndex);
                varIndex++;
            }
        }

        int index = 0;
        if (((PropertiesContainer)target).local_properties[index].variableTypes == null)
            ((PropertiesContainer)target).local_properties[index].variableTypes = new CustomEnumFlags<VariableType>(list.ToArray());
        root.Add(customFlagsEditor.Show(((PropertiesContainer)target).local_properties[index].variableTypes));

        return root;
    }
}
