using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class SayWithScript : CharacterInteraction
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

        CommandTalk normalTalk = new CommandTalk();
        string saystring = ((GameObject)scriptObject).GetComponent<ISayScript>().SayWithScript(interaction);
        normalTalk.Queue(character.messageTalker, saystring, false, false);
        Debug.Log(saystring);
    }

    public override InteractionAction Copy()
    {
        SayWithScript interaction = new SayWithScript();
        interaction.scriptField = scriptField;  
        interaction.scriptObject = scriptObject;
        return interaction;
    }
}
