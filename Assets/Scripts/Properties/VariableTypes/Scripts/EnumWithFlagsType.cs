using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using System;
using System.Xml;
using System.IO;
public class EnumWithFlagsType : VariableType
{
    public EnumWithFlagsType()
    {
        typeName = "enum with flags";
        Index = 3;
    }

    private void OnEnable()
    {
        typeName = "enum with flags";
        Index = 3;
    }

    public override void SetPropertyField(VisualElement element, GenericProperty property)
    {
        base.SetPropertyField(element, property);
#if UNITY_EDITOR

        Settings settings = Resources.Load<Settings>("Settings/Settings");

        if (settings.EnumWithFlagVariables.Count == 0)
        {
            Label advice = new Label("You must create Enum With Flags variables in settings");
            advice.style.color = Color.red;
            advice.style.whiteSpace = WhiteSpace.Normal;
            advice.style.unityTextAlign = TextAnchor.MiddleCenter;
            element.Add(advice);
            Button button = new Button(() =>
            {
                UnityEditor.Selection.objects = new UnityEngine.Object[] { settings };
                UnityEditor.EditorGUIUtility.PingObject(settings);
            });
            button.text = "Go to settings";
            element.Add(button);
            return;
        }

        EnumFlagsField variablesField = new EnumFlagsField((GenericEnum)0);

        variablesField.value = GetSelectedVariables(property, settings.EnumWithFlagVariables);

        List<string> choices = new List<string>();

        for (int i = 0; i < settings.EnumWithFlagVariables.Count; i++)
        {
            choices.Add(settings.EnumWithFlagVariables[i].name);
        }

        variablesField.choices = choices;

        List<int> choicesMasks = new List<int>();

        for (int i = 0; i < settings.EnumWithFlagVariables.Count; i++)
        {
            choicesMasks.Add(1<<i);
        }

        variablesField.choicesMasks = choicesMasks;

        variablesField.RegisterValueChangedCallback((evt) => SetSelectedVariableValue(property, evt.newValue, settings.EnumWithFlagVariables));

        element.Add(variablesField);

        /*
        EnumFlagsField field = new EnumFlagsField((GenericEnum)0);

        field.value = (GenericEnum)GetVariableValue(property);

        field.choices = new List<string>() { "choice1", "choice2", "choice3" };

        CustomEnumFlags<EnumerableType>.SetChoicesMasksByChoicesInOrder(field.choicesMasks, field.choices);

        field.RegisterValueChangedCallback((evt) => SetVariableValue(property,evt.newValue));
        
        element.Add(field);*/
#endif
    }

    public System.Enum GetSelectedVariables(GenericProperty property, List<Settings.EnumWithFlagVariablesType> variables)
    {
        if (property.variableValues == null || Index >= property.variableValues.Length)
            return (GenericEnum)0;

        int integerValue = 0;

        XmlReader reader = XmlReader.Create(new StringReader(property.variableValues[Index]));
        try
        {
            while (reader.Read())
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    if(reader.IsStartElement(variables[i].name.Replace(" ", "-")))
                    {
                        if (reader.GetAttribute("selected") == "true")
                        {
                            integerValue |= (1 << i);
                        }
                    }
                }
            }
        }
        catch
        {
            return (GenericEnum)0;
        }
        return (GenericEnum)integerValue;
    }

    public void SetSelectedVariableValue(GenericProperty property, System.Enum value, List<Settings.EnumWithFlagVariablesType> variables)
    {
        StringWriter sw = new StringWriter();
        XmlWriter writer = XmlWriter.Create(sw);

        writer.WriteStartDocument();
        for (int i = 0; i < variables.Count; i++)
        {
            writer.WriteStartElement(variables[i].name.Replace(" ", "-"));
            if ((Convert.ToInt32((GenericEnum)value) & (1 << i)) != 0)
            {
                writer.WriteAttributeString("selected", "true");
            }
            else
            {
                writer.WriteAttributeString("selected", "false");
            }
        }
        writer.WriteEndDocument();
        writer.Flush();

        property.variableValues[Index] = sw.ToString();

        Debug.Log(sw.ToString());
    }

    public System.Enum GetVariableValue(GenericProperty property)
    {
        if (property.variableValues == null || Index >= property.variableValues.Length)
            return (GenericEnum)0;

        int integerValue = 0;
        if (int.TryParse(property.variableValues[Index], out integerValue))
            return (GenericEnum)integerValue;
        return (GenericEnum)0;
    }

    public void SetVariableValue(GenericProperty property,System.Enum value)
    {
        property.variableValues[Index] = (Convert.ToInt32((GenericEnum)value)).ToString();
    }
}
