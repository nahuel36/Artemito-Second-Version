using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class LocalAndGlobalProperties : Editor
{
    [SerializeField]
    VisualTreeAsset variableItem;
    [SerializeField]
    VisualTreeAsset localProperty;
    CustomListView<LocalProperty> customListView;
    public VisualElement CreateGUI(List<LocalProperty> local_properties, VisualElement root)
    {
        // Each editor window contains a root VisualElement object
        // Instantiate UXML

        if(customListView == null)
            customListView = new CustomListView<LocalProperty>();

        customListView.ItemsSource = local_properties;

        customListView.ItemContent = (i) => ItemContent(i, local_properties[i]);

        customListView.OnAdd = () => {
            int variablesLength = EnumerablesUtility.GetAllVariableTypes().Length;
            LocalProperty localprop = new LocalProperty();
            localprop.variablesContainer = new CustomEnumFlags<VariableType>(0);
            return localprop;        
        };

        VisualElementsUtils.HideVisualElement(root.Q("LocalProperties").Q("LocalProperty"));
        
        customListView.Init(root.Q("LocalProperties").Q("CustomListView"));

        StyleLength lenght = new StyleLength(StyleKeyword.Auto);

        root.Q("LocalProperties").Q("CustomListView").style.height = lenght;

        return root;
    }

    private VisualElement ItemContent(int index, LocalProperty property)
    {
        Foldout foldout = new Foldout();

        foldout.text = "property " + (index+1).ToString();

        VisualElement element = new VisualElement();

        element.Add(localProperty.CloneTree());

        VisualElementsUtils.HideVisualElement(element.Q("VariableItem"));

        element.Q<TextField>("PropertyName").value = property.name;
        element.Q<TextField>("PropertyName").RegisterValueChangedCallback((name) => { property.name = name.newValue; });



        EnumerablesUtility.ShowEnumFlagsField("VariableTypes",element,property.variablesContainer, ()=> { EnumerablesUtility.UpdateAllVariablesFields(element, property.variablesContainer); });
        EnumerablesUtility.UpdateAllVariablesFields(element, property.variablesContainer);

        foldout.Add(element);

        return foldout;
    }
}
