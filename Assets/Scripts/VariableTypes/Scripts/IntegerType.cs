using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntegerType", menuName = "Pnc/PropertyVariablesType/IntegerType", order = 1)]
public class IntegerType : VariableType
{
    public IntegerType()
    {
        typeName = "integer";
        EnumIndex = 2;
    }

    private void OnEnable()
    {
        typeName = "integer";
        EnumIndex = 2;
    }
}
