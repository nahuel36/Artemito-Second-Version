using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SayWithScript : InteractionAction
{
    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR 
        base.SetEditorField(visualElement, interaction);

        InteractionProperties properties = (InteractionProperties)CreateInstance(typeof(InteractionProperties));

        properties.CreateGUI(interaction.interaction_properties, visualElement);
#endif
    }

    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        base.ExecuteAction(properties, interaction);

        Character character = interaction.SubtypeToCharacter(interaction.subtypeObject);

        CommandTalk normalTalk = new CommandTalk();
        normalTalk.Queue(character.messageTalker, "hello", false, false);
    }
}
