using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SayWithScript : InteractionAction
{
    [HideInInspector][SerializeField]private string message;

    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR 
        base.SetEditorField(visualElement, interaction);

        InteractionProperties properties = (InteractionProperties)CreateInstance(typeof(InteractionProperties));

        properties.CreateGUI(interaction.interaction_properties, visualElement);

        TextField messageField = new TextField();
        messageField.value = message;
        messageField.RegisterValueChangedCallback(changeMessage);
        visualElement.Add(messageField);
#endif
    }

    private void changeMessage(ChangeEvent<string> evt)
    {
        message = evt.newValue;
    }

    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        base.ExecuteAction(properties, interaction);

        Character character = interaction.SubtypeToCharacter(interaction.subtypeObject);

        CommandTalk normalTalk = new CommandTalk();
        normalTalk.Queue(character.messageTalker, message, false, false);
    }
}
