using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interaction 
{
    public string type;
    public string subtype;
    public string subtypeObject;
    public List<InteractionProperty> interaction_properties;
    public InteractionAction action;
    public bool expandedInInspector;
    public string InventoryNameToSubtypeObject(string item)
    {
        return "inventory-" + item;
    }

    public string SubtypeObjectToInventoryName(string subtype)
    {
        return subtype.Replace("inventory-", "");
    }


    public Character SubtypeToCharacter(string subtype)
    {
        return UnityEngine.Object.FindAnyObjectByType<Character>();
    }

    public Interaction CopyItem(Interaction interOrigin) {
        Interaction interDestiny = new Interaction();
        interDestiny.type = interOrigin.type;
        interDestiny.subtype = interOrigin.subtype;
        interDestiny.subtypeObject = interOrigin.subtypeObject;
        interDestiny.action = interOrigin.action.Copy();
        interDestiny.interaction_properties = new List<InteractionProperty>();
        for (int i = 0; i < interOrigin.interaction_properties.Count;i++)
            interDestiny.interaction_properties.Add(interOrigin.interaction_properties[i].Copy());
        return interDestiny;
    }
}
