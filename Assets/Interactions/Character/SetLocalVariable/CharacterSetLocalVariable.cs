using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class CharacterSetLocalVariable : InteractionAction
{
    public LocalProperty propertyToSet;
    public Character characterToSet;

    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        propertyToSet.variablesContainer.SetValue("string", "pepe");
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
#endif
    }


}
