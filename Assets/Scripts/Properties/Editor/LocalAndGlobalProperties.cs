using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
public class LocalAndGlobalProperties : Editor
{
    public static VisualElement CreateGUI(List<LocalProperty> local_properties, VisualElement root, Action OnChange = null, Action OnUpdateField = null)
    {
        // Each editor window contains a root VisualElement object
        // Instantiate UXML
        CustomListView<LocalProperty> customListView = new CustomListView<LocalProperty>();

        customListView.ItemsSource = local_properties;

        customListView.ItemContent = (i) => 
        {
            if (i >= 0 && local_properties.Count > 0)
                return ItemContent(i, local_properties[i], OnChange, OnUpdateField);
            else
                return null;
        };

        customListView.OnAdd = () => {
            int variablesLength = EnumerablesUtility.GetAllVariableTypes().Length;
            LocalProperty localprop = new LocalProperty();
            localprop.variablesContainer = new CustomEnumFlags(0);
            localprop.variablesContainer.type = CustomEnumFlags.contentType.variable;
            return localprop;        
        };

        VisualElementsUtils.HideVisualElement(root.Q("LocalProperties").Q("LocalProperty"));
        
        customListView.Init(root.Q("LocalProperties").Q("CustomListView"));

        StyleLength lenght = new StyleLength(StyleKeyword.Auto);

        root.Q("LocalProperties").Q("CustomListView").style.height = lenght;

        return root;
    }

    private static VisualElement ItemContent(int index, LocalProperty property, Action onChange = null, Action onUpdateField = null)
    {
        Foldout foldout = new Foldout();

        foldout.text = "property " + (index+1).ToString();

        VisualElement element = new VisualElement();

        VisualElement localPropertyVisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Properties/Editor/LocalProperty.uxml").CloneTree();

        element.Add(localPropertyVisualTree);

        VisualElementsUtils.HideVisualElement(element.Q("VariableItem"));

        element.Q<TextField>("PropertyName").value = property.name;
        element.Q<TextField>("PropertyName").RegisterValueChangedCallback((name) => { 
            property.name = name.newValue;
            onChange?.Invoke();
        });



        EnumerablesUtility.ShowEnumFlagsField("VariableTypes",element,property.variablesContainer, ()=> { 
            EnumerablesUtility.UpdateAllVariablesFields(element, property.variablesContainer, onChange);
            onChange?.Invoke();
        },null, onUpdateField);
        EnumerablesUtility.UpdateAllVariablesFields(element, property.variablesContainer, onChange);

        foldout.Add(element);

        return foldout;
    }
}
