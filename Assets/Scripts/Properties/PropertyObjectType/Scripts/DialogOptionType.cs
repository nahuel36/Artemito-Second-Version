using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogOptionType : PropertyObjectType
{

    public DialogOptionType()
    {
        Index = 3;
        TypeName = "Dialog Option";
    }

    private void OnEnable()
    {
        Index = 3;
        TypeName = "Dialog Option";
    }
}
