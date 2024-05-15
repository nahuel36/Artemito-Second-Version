using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Xml.Linq;

using UnityEditor;
using Unity.Collections.LowLevel.Unsafe;


#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class CharacterSetLocalVariable : InteractionAction
{
    public LocalProperty propertyToSet;
    public Character characterToSet;
    public CustomEnumFlags<VariableType> customEnumFlags;
    public CustomEnumFlags<PropertyObjectType> customFlagsPO;
    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        propertyToSet.variablesContainer.SetValue("string", customEnumFlags.GetStringValue("string"));
    }

    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR
        base.SetEditorField(visualElement, interaction);

        ObjectField characterField = new ObjectField();
        characterField.label = "Character";
        characterField.objectType = typeof(Character);
        characterField.value = characterToSet;
        characterField.RegisterValueChangedCallback((newvalue) => { characterToSet = (Character)newvalue.newValue; }) ;
        visualElement.Add(characterField);

        if(characterToSet != null) 
        { 
            for (int i = 0; i < characterToSet.local_properties.Count; i++)
            {
                if (propertyToSet.name == characterToSet.local_properties[i].name)
                    propertyToSet = characterToSet.local_properties[i];
            }
        }

        DropdownField propertyField = new DropdownField();
        propertyField.label = "Property";
        for (int i = 0; i < characterToSet.local_properties.Count; i++)
        {
            propertyField.choices.Add(characterToSet.local_properties[i].name);
        }
        propertyField.value = propertyToSet?.name;
        propertyField.RegisterValueChangedCallback((newvalue) =>
        {
            for (int i = 0; i < characterToSet.local_properties.Count; i++)
            {
                if (newvalue.newValue == characterToSet.local_properties[i].name)
                    propertyToSet = characterToSet.local_properties[i];
            }
        });
        visualElement.Add(propertyField);

        if(customEnumFlags == null)
            customEnumFlags = new CustomEnumFlags<VariableType>(0);

        VisualElement newElement = new VisualElement();

        newElement.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/SetProperty.uxml").CloneTree());

        if(propertyToSet != null)
            EnumerablesUtility.ShowEnumFlagsField(newElement, customEnumFlags,()=>EnumerablesUtility.UpdateAllVariablesFields(newElement,customEnumFlags), propertyToSet.variablesContainer);
        EnumerablesUtility.UpdateAllVariablesFields(newElement, customEnumFlags);

        visualElement.Add(newElement);
#endif
    }


}
