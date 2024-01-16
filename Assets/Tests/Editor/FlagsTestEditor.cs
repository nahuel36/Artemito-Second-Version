using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Linq;

[CustomEditor(typeof(FlagsTest))]
public class FlagsTestEditor : Editor
{

    public VisualTreeAsset visual;

    [System.Flags]
    public enum EntityPartial
    {
        just_two = 1 << 1,
        just_three = 1<<2,
        others = 1 << 0,
    }

    public Label label;

    public override VisualElement CreateInspectorGUI()
    {


        /*
        var imguiContainer = new IMGUIContainer(() =>
        {
            EditorGUI.BeginChangeCheck();

            //System.Enum.Parse(enum, "just_two", "just_three");

            property.intValue = (int)((EntityFlags)EditorGUILayout.EnumFlagsField("Label", (EntityPartial)property.intValue));

            EditorGUILayout.LabelField(serializedObject.FindProperty("flags").enumValueFlag.ToString());

            if (((FlagsTest)target).enumerator == null)
            {
                ((FlagsTest)target).enumerator = FlagsTest.CreateEnumFromArrays(new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }, property2.intValue);
            }
            else
            {
                ((FlagsTest)target).enumerator = EditorGUILayout.EnumFlagsField(((FlagsTest)target).enumerator);
                property2.intValue = Convert.ToInt32(((FlagsTest)target).enumerator);
                EditorGUILayout.LabelField(property2.intValue.ToString());
            }


            if (EditorGUI.EndChangeCheck())
            { 
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        });
        element.Add(imguiContainer);
        */

        // visual.CloneTree(element);


        // DropdownField dropdown = new DropdownField();
        //dropdown.choices = GetStringArrayFromProperty(serializedObject.FindProperty("myList"));
        //  element.Add(dropdown);

        VisualElement element = new VisualElement();
        var property = serializedObject.FindProperty("flags");
        var property2 = serializedObject.FindProperty("enumeratorFlag");

        if (((FlagsTest)target).enumerator == null)
        {
            ((FlagsTest)target).enumerator = FlagsTest.CreateEnumFromArrays(new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }, property2.intValue);
        }
        //UnityEditor.UIElements.EnumField enumFlags = new UnityEditor.UIElements.EnumField(((FlagsTest)target).enumerator);
        MultiSelectionEnumField enumFlags = new MultiSelectionEnumField("Options", new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
        //UnityEditor.UIElements.EnumFlagsField enumFlags = new UnityEditor.UIElements.EnumFlagsField(((FlagsTest)target).enumerator);
        //enumFlags.choicesMasks = new List<int>() { 1 << 0 | 1 << 1, 1 << 2, 1 << 3, 1 << 4, 1 << 5, 1 << 6};
        //enumFlags.choices = new List<string>() { "Mondtues", "WedAnesday", "Thursday", "Friday", "Saturday", "Sunday" };
        enumFlags.RegisterCallback<ChangeEvent<System.Enum>>(onChange);
        element.Add(enumFlags);

        label = new Label(serializedObject.FindProperty("enumeratorFlag").intValue.ToString());
        element.Add(label);

        return element;
    }

    private void onChange(ChangeEvent<Enum> evt)
    {
        serializedObject.FindProperty("enumeratorFlag").intValue = Convert.ToInt32(evt.newValue);
        ((FlagsTest)target).enumerator = FlagsTest.CreateEnumFromArrays(new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }, Convert.ToInt32(evt.newValue));
        label.text = serializedObject.FindProperty("enumeratorFlag").intValue.ToString();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    public List<string> GetStringArrayFromProperty(SerializedProperty prop)
    {
        if (!prop.isArray) return null;

        if (prop.arraySize <= 0) return null;

        if (prop.arrayElementType != "string") return null;

        List<string> array = new List<string>();

        for (int i = 0; i < prop.arraySize; i++)
        {
            array.Add(prop.GetArrayElementAtIndex(i).stringValue);
        }

        return array;
        
    }
}
