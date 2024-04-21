using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSetLocalVariable : InteractionAction
{
    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        FindFirstObjectByType<TestSayScript>().GetComponent<Character>().local_properties[0].variablesContainer.SetValue("string", "pepe");
    }
}
