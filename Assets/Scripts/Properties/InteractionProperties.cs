using System.Collections.Generic;
using System.Xml.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
public class InteractionProperties : Editor
{
    [SerializeField]
    VisualTreeAsset variableItem;
    [SerializeField]
    VisualTreeAsset interaction_property;
    CustomListView<InteractionProperty> customListView;
    public VisualElement CreateGUI(List<InteractionProperty> local_properties, VisualElement root)
    {
        // Each editor window contains a root VisualElement object
        // Instantiate UXML

        Label tittle = new Label("Interaction Properties");

        tittle.style.unityFontStyleAndWeight = FontStyle.Bold;

        root.Add(tittle);

        if(customListView == null)
            customListView = new CustomListView<InteractionProperty>();

        customListView.ItemsSource = local_properties;

        customListView.ItemContent = (i) => ItemContent(i, local_properties[i]);

        customListView.ItemHeight = (i) => { return new StyleLength(StyleKeyword.Auto); };

        customListView.OnAdd = () => {
            int variablesLength = EnumerablesUtility.GetAllVariableTypes().Length;
            InteractionProperty localprop = new InteractionProperty();
            localprop.variablesContainer = new CustomEnumFlags<VariableType>(0);
            return localprop;        
        };

        customListView.CopyItem = (propOrigin) =>
        { return propOrigin.Copy(); };

        customListView.Init(root, true);

        return root;
    }

    

    private VisualElement ItemContent(int index, InteractionProperty property)
    {
        VisualElement element = new VisualElement();

        //element.Add(interaction_property.CloneTree());
        element.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Properties/Editor/InteractionProperty.uxml").CloneTree());

        element.Q("VariableItem").visible = false;
        element.Q("VariableItem").StretchToParentSize();

        element.Q<TextField>("PropertyName").value = property.name;
        element.Q<TextField>("PropertyName").RegisterValueChangedCallback((name) => { property.name = name.newValue; });


        EnumerablesUtility.ShowEnumFlagsField(element, property.variablesContainer, () => {
            EnumerablesUtility.UpdateAllVariablesFields(element, property.variablesContainer); });

        EnumerablesUtility.UpdateAllVariablesFields(element, property.variablesContainer);

        return element;
    }
}
#endif
