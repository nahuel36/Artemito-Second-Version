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
    [HideInInspector][SerializeField]GameObject scriptObject;
    ObjectField scriptField;
    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR 
        base.SetEditorField(visualElement, interaction);

        InteractionProperties properties = (InteractionProperties)CreateInstance(typeof(InteractionProperties));

        properties.CreateGUI(interaction.interaction_properties, visualElement);

        scriptField = new ObjectField();
        scriptField.objectType = typeof(GameObject);
        scriptField.value = scriptObject;
        scriptField.RegisterValueChangedCallback(changeScript);
        visualElement.Add(scriptField);
#endif
    }

    private void changeScript(ChangeEvent<UnityEngine.Object> evt)
    {
        if (evt.newValue != null && ((GameObject)evt.newValue).GetComponent<ISayScript>() != null)
            scriptObject = (GameObject)evt.newValue;
        else
            scriptField.value = scriptObject;
    }

    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        base.ExecuteAction(properties, interaction);

        Character character = interaction.SubtypeToCharacter(interaction.subtypeObject);

        CommandTalk normalTalk = new CommandTalk();
        normalTalk.Queue(character.messageTalker, ((GameObject)scriptObject).GetComponent<ISayScript>().SayWithScript(properties), false, false);
    }

    public override InteractionAction Copy()
    {
        SayWithScript interaction = new SayWithScript();
        interaction.scriptField = scriptField;  
        interaction.scriptObject = scriptObject;
        return interaction;
    }
}
