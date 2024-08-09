using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Interaction 
{
    public string type;
    public string subtype;
    public List<InteractionProperty> interaction_properties;
    public InteractionAction action;
    public bool expandedInInspector;

    public Interaction CopyItem(Interaction interOrigin) {
        Interaction interDestiny = new Interaction();
        interDestiny.type = interOrigin.type;
        interDestiny.subtype = interOrigin.subtype;
        interDestiny.action = interOrigin.action.Copy();
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
            return interaction.characterType.character;
        }

        return null;
    }
}
