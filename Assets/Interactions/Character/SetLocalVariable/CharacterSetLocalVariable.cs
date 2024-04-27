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
    private LocalProperty propertyToSet;
    public Character characterToSet;

    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        characterToSet.local_properties[0].variablesContainer.SetValue("string", "pepe");
    }

    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR
        base.SetEditorField(visualElement, interaction);

        ObjectField characterField = new ObjectField();
        characterField.objectType = typeof(Character);
        characterField.value = characterToSet;
        characterField.RegisterValueChangedCallback((newvalue) => { characterToSet = (Character)newvalue.newValue; }) ;
        visualElement.Add(characterField);
#endif
    }


}
