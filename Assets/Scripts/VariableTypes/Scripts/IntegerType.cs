using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "IntegerType", menuName = "Pnc/PropertyVariablesType/IntegerType", order = 1)]
public class IntegerType : VariableType
{
    public IntegerType()
    {
        typeName = "integer";
        Index = 1;
    }

    private void OnEnable()
    {
        typeName = "integer";
        Index = 1;
    }

    public override void SetPropertyField(VisualElement element, GenericProperty property)
    {
        base.SetPropertyField(element, property);

        IntegerField field = new IntegerField();

        field.value = GetVariableValue(property);

        field.RegisterValueChangedCallback((evt) => SetVariableValue(property, evt.newValue));

        element.Add(field);
    }

    public int GetVariableValue(GenericProperty property)
    {
        int integerValue = -1; 
        if(int.TryParse(property.variableValue[Index], out integerValue))
            return integerValue;
        return -1;
    }

    public void SetVariableValue(GenericProperty property, int value)
    {
        property.variableValue[Index] = value.ToString();
    }
}
