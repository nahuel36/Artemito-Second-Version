using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSayScript : MonoBehaviour, ISayScript
{
    public string SayWithScript(Interaction interaction)
    {
        //return "hello " + arguments[0].variablesContainer.GetValue("string") + arguments[1].variablesContainer.GetValue("string");
        return "hello " + interaction.GetCharacter().local_properties[0].variablesContainer.GetStringValue("string") + interaction.interaction_properties[0].variablesContainer.GetStringValue("string");
    }

    
}
