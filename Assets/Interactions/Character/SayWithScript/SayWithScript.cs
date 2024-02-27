using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class SayWithScript : InteractionAction
{
    [HideInInspector][SerializeField]private string message;
    [HideInInspector][SerializeField]GameObject script;
    ObjectField sayScript;
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

        sayScript = new ObjectField();
        sayScript.objectType = typeof(GameObject);
        sayScript.value = script;
        sayScript.RegisterValueChangedCallback(changeScript);
        visualElement.Add(sayScript);
#endif
    }

    private void changeScript(ChangeEvent<UnityEngine.Object> evt)
    {
        if (evt.newValue != null && ((GameObject)evt.newValue).GetComponent<ISayScript>() != null)
            script = (GameObject)evt.newValue;
        else
            sayScript.value = script;
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
        normalTalk.Queue(character.messageTalker, ((GameObject)script).GetComponent<ISayScript>().SayWithScript(properties), false, false);
    }
}
