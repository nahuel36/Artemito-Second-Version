using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringType", menuName = "Pnc/PropertyVariablesType/StringType", order = 1)]
public class StringType : VariableType
{
    public StringType()
    {
        typeName = "string";
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(typeName))
            typeName = "string";
    }
}
