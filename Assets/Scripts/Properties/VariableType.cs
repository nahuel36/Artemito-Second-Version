using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class VariableType : EnumerableType
{
    public bool isDefaultValue = false;
    [SerializeField] public bool changedIngame = false;
    [SerializeField] public string StringValue;
    [SerializeField] public string StringIngameValue;
    [SerializeField] public UnityEngine.Object ObjectValue;
    [SerializeField] public UnityEngine.Object ObjectIngameValue;
    [SerializeField] public bool isString;
    public virtual void SetPropertyField(VisualElement element, GenericProperty property)
    {        

    }

    public virtual void SetValue<T>(T value)
    {
        this.changedIngame = true;
        if(isString)
            this.StringIngameValue = value as string;
        else
            this.ObjectIngameValue = value as UnityEngine.Object;
    }

    public virtual string GetStringValue()
    {
        if (changedIngame)
            return StringIngameValue;
        else
            return StringValue;
    }

    public virtual UnityEngine.Object GetObjectValue()
    {
        if (changedIngame)
            return ObjectIngameValue;
        else
            return ObjectValue;
    }

}
