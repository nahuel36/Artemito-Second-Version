using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSayScript : MonoBehaviour, ISayScript
{
    public string SayWithScript(List<InteractionProperty> arguments)
    {
        return "hello " + arguments[0].variableValues[2] + arguments[0].variableValues.Length;
    }

    
}
