using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using Unity.VisualScripting;
using System.Reflection;
[System.Serializable]
public class PropertyObjectType : EnumerableType
{
    public bool hasSpecificFields = false;

    public virtual void SetPropertyEditorField(VisualElement element) { 
        
    }

    public virtual List<LocalProperty> GetLocalPropertys() 
    {
        return null;
    }

    public UnityEngine.Object genericFieldObject;

    public void SetGenericField(VisualElement element)
    {
        ObjectField field = new ObjectField();

        field.value = genericFieldObject;

        field.RegisterValueChangedCallback((evt) => genericFieldObject = evt.newValue);

        field.label = "Origin";

        //SetGenericEspecificFieldByType(field, field.value);

        element.Add(field);

        /*
        object obj2 = new DialogOption();
        ((DialogOption)obj2).initialText = "lala";
        propertyContainer = obj2;*/
    }
    public static void SetGenericEspecificFieldByType(VisualElement element, UnityEngine.Object value)
    {
        if (value is GameObject && ((GameObject)value).GetComponent<Character>() != null)
        { 
            CharacterType type = new CharacterType();
            type.SetGenericEspecificField(element);
        }
    }

    public virtual void SetGenericEspecificField(VisualElement element)
    {
       
    }

    internal static GenericProperty GetGenericPropertyWithField(VisualElement visualElement, object propertyOrigin)
    {
        throw new NotImplementedException();
    }


    public virtual EnumerableType Copy()
    {
        PropertyObjectType newcopy = new PropertyObjectType();
        newcopy.Index = Index;
        newcopy.typeName = typeName;
        newcopy.hasSpecificFields = hasSpecificFields;
        newcopy.genericFieldObject = genericFieldObject;
        return newcopy;
    }
}
