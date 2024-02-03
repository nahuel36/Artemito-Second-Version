using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BooleanType", menuName = "Pnc/PropertyVariablesType/BooleanType", order = 1)]
public class BooleanType: VariableType
{
    public BooleanType() {
        typeName = "boolean";    
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(typeName))
            typeName = "boolean";
    }
}
