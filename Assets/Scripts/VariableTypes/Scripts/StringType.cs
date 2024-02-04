using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringType", menuName = "Pnc/PropertyVariablesType/StringType", order = 1)]
public class StringType : VariableType
{
    public StringType()
    {
        typeName = "string";
        EnumIndex = 4;
    }

    private void OnEnable()
    {
        typeName = "string";
        EnumIndex = 4;
    }
}
