using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "BooleanType", menuName = "Pnc/PropertyVariablesType/BooleanType", order = 1)]
public class BooleanType: VariableType
{
    public BooleanType() {
        typeName = "boolean";
        Index = 0;
    }

    private void OnEnable()
    {
        typeName = "boolean";
        Index = 0;
    }

    public override void SetPropertyField(VisualElement element, GenericProperty property)
    {
        base.SetPropertyField(element, property);

        Toggle field = new Toggle();
               
        field.value = GetVariableValue(property);

        field.RegisterValueChangedCallback((evt) => SetVariableValue(property, evt.newValue));

        element.Add(field);
    }

    public bool GetVariableValue(GenericProperty property)
    {
        if (property.variableValue[Index].ToLower() == "true")
            return true;
        else
            return false;
    }

    public void SetVariableValue(GenericProperty property, bool value)
    {
        property.variableValue[Index] = value.ToString().ToLower();
    }
}
