using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LocalAndGlobalProperties : Editor
{
    [SerializeField]
    VisualTreeAsset variableItem;
    [SerializeField]
    VisualTreeAsset localProperty;
    
    public VisualElement CreateGUI(LocalProperty[] local_properties, VisualElement root)
    {
        // Each editor window contains a root VisualElement object
        // Instantiate UXML


        CustomListView<LocalProperty> customListView = new CustomListView<LocalProperty>();
        customListView.ItemsSource = local_properties;

        customListView.ItemContent = (i) => ItemContent(i, local_properties);

        customListView.ItemHeight = (i) => { return new StyleLength(StyleKeyword.Auto); };

        root.Q("LocalProperties").Q("LocalProperty").visible = false;
        root.Q("LocalProperties").Q("LocalProperty").StretchToParentSize();

        customListView.Init(root.Q("LocalProperties").Q("CustomListView"));

        StyleLength lenght = new StyleLength(StyleKeyword.Auto);

        root.Q("LocalProperties").Q("CustomListView").style.height = lenght;

        return root;
    }

    private VisualElement ItemContent(int index, GenericProperty[] properties)
    {
        VisualElement element = new VisualElement();

        element.Add(localProperty.CloneTree());

        element.Q("VariableItem").visible = false;
        element.Q("VariableItem").StretchToParentSize();

        element.Q<TextField>("PropertyName").value = properties[index].name;
        element.Q<TextField>("PropertyName").RegisterValueChangedCallback((name) => { properties[index].name = name.newValue; });


        if (properties is LocalProperty[])
        {
            VariableTypesUtility.ShowEnumFlagsField(element, ((LocalProperty)properties[index]).variableTypes);

            foreach (var variable in VariableTypesUtility.GetAllVariableTypes())
            {
                if (((LocalProperty)properties[index]).variableTypes.ContainsValue(variable))
                {
                    VisualElement variableItemElement = variableItem.CloneTree();
                    variableItemElement.Q<VisualElement>("Value").Q<Label>("Label").text = variable.TypeName;
                    variable.SetPropertyField(variableItemElement.Q<VisualElement>("Field"), properties[index]);
                    element.Add(variableItemElement);
                }
            }
        }

        return element;
    }
}
