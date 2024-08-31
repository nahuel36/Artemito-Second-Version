using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class LeafInteraction 
{
    public string type;
    public string subtype;
    public List<InteractionProperty> interaction_properties;
    public InteractionAction action;
    public InteractionAction.ActionData actionData;
    public bool expandedInInspector;

    public LeafInteraction CopyItem(LeafInteraction interOrigin) {
        LeafInteraction interDestiny = new LeafInteraction();
        interDestiny.type = interOrigin.type;
        interDestiny.subtype = interOrigin.subtype;
        interDestiny.action = interOrigin.action.Copy();
        interDestiny.actionData = interOrigin.actionData;
        interDestiny.interaction_properties = new List<InteractionProperty>();
        if(interOrigin.interaction_properties != null)
            for (int i = 0; i < interOrigin.interaction_properties.Count;i++)
                interDestiny.interaction_properties.Add((InteractionProperty)interOrigin.interaction_properties[i].Copy());
        return interDestiny;
    }

    public Character GetCharacter()
    {
        if (action is CharacterInteraction interaction)
        {
            return ((Character)((CharacterType)interaction.characterType).character);
        }

        return null;
    }
}
