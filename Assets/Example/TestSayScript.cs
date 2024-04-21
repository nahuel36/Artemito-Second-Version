using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSayScript : MonoBehaviour, ISayScript
{
    public string SayWithScript(List<InteractionProperty> arguments)
    {
        Debug.Log(GetComponent<Character>().local_properties[0].variablesContainer.GetStringValue("string"));
        //return "hello " + arguments[0].variablesContainer.GetValue("string") + arguments[1].variablesContainer.GetValue("string");
        return "hello " + GetComponent<Character>().local_properties[0].variablesContainer.GetStringValue("string");
    }

    
}
