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

public class EnumClassicType : VariableType
{
    public EnumClassicType()
    {
        typeName = "enum classic";
        Index = 4;
    }

    private void OnEnable()
    {
        typeName = "enum classic";
        Index = 4;
    }

    public override void SetPropertyField(VisualElement root, GenericProperty property)
    {
        base.SetPropertyField(root, property);
#if UNITY_EDITOR
        VisualElement element = root.Q("Field");

        Toggle defaultToggle = root.Q<Toggle>("Default");
        if (defaultToggle != null)
        { 
            root.Q<Toggle>("Default").visible = false;
            root.Q<Toggle>("Default").StretchToParentSize();
        }

        Settings settings = Resources.Load<Settings>("Settings/Settings");

        if (settings.EnumClassicVariables.Count == 0)
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

        variablesField.value = GetSelectedVariables(property, settings.EnumClassicVariables);

        List<string> choices = new List<string>();

        for (int i = 0; i < settings.EnumClassicVariables.Count; i++)
        {
            choices.Add(settings.EnumClassicVariables[i].name);
        }

        variablesField.choices = choices;

        List<int> choicesMasks = new List<int>();

        for (int i = 0; i < settings.EnumClassicVariables.Count; i++)
        {
            choicesMasks.Add(1<<i);
        }

        variablesField.choicesMasks = choicesMasks;

        variablesField.RegisterValueChangedCallback((evt) => SetVariableValue(settings.EnumClassicVariables, evt.newValue, property));

        element.Add(variablesField);

        for (int i = 0; i < settings.EnumClassicVariables.Count; i++)
        {
            if (settings.EnumClassicVariables[i].values.Count == 0)
            {
                Label advice = new Label("Your enum has no values");
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
                continue;
            }

            if ((Convert.ToInt32((GenericEnum)variablesField.value) & (1<<i)) != 0)
            {
                DropdownField field = new DropdownField();

                int index = i;

                field.value = GetVariableValue(property, settings.EnumClassicVariables[index]);

                field.choices = settings.EnumClassicVariables[i].values;

                field.RegisterValueChangedCallback((evt) => SetValue(settings.EnumClassicVariables, settings.EnumClassicVariables[index], evt.newValue, property));

                element.Add(field); 
            }        
        }
       
#endif
    }

    public System.Enum GetSelectedVariables(GenericProperty property, List<Settings.EnumVariablesType> variables)
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
                    if(reader.IsStartElement(XmlUtility.ConvertStringToUseInXml(variables[i].name)))
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

    public string GetVariableValue(GenericProperty property, Settings.EnumVariablesType variable)
    {
        if (property.variableValues == null || Index >= property.variableValues.Length)
            return "";

        string value = "";

        XmlReader reader = XmlReader.Create(new StringReader(property.variableValues[Index]));
        try
        {
            while (reader.Read())
            {
                if (reader.IsStartElement(XmlUtility.ConvertStringToUseInXml(variable.name)))
                {
                    for (int i = 0; i < variable.values.Count; i++)
                    {
                        try
                        {
                            if (reader.GetAttribute("value") == variable.values[i])
                            {
                                value = variable.values[i];
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return value;
    }


    public void SetVariableValue(List<Settings.EnumVariablesType> variables, System.Enum typeValue, GenericProperty property) {
        StringWriter sw = new StringWriter();
        XmlWriter writer = XmlWriter.Create(sw);

        writer.WriteStartDocument();
        for (int i = 0; i < variables.Count; i++)
        {
            writer.WriteStartElement(XmlUtility.ConvertStringToUseInXml(variables[i].name));
            if ((Convert.ToInt32((GenericEnum)typeValue) & (1 << i)) != 0)
            {
                writer.WriteAttributeString("selected", "true");

                for (int j = 0; j < variables[i].values.Count; j++)
                { 
                    if (GetVariableValue(property, variables[i]) == variables[i].values[j])
                    {
                        writer.WriteAttributeString("value", variables[i].values[j]);
                    }
                }
            }
            else
            {
                writer.WriteAttributeString("selected", "false");
            }
        }
        writer.WriteEndDocument();
        writer.Flush();

        property.variableValues[Index] = sw.ToString();
    }

    public void SetValue(List<Settings.EnumVariablesType> variables, Settings.EnumVariablesType variable, string typeValue, GenericProperty property)
    {
        StringWriter sw = new StringWriter();
        XmlWriter writer = XmlWriter.Create(sw);

        writer.WriteStartDocument();
        for (int i = 0; i < variables.Count; i++)
        {
            writer.WriteStartElement(XmlUtility.ConvertStringToUseInXml(variables[i].name));
            if ((Convert.ToInt32(GetSelectedVariables(property, variables)) & (1 << i)) != 0)
            {
                writer.WriteAttributeString("selected", "true");

                for (int j = 0; j < variables[i].values.Count; j++)
                {
                    if (variable == variables[i] && typeValue == variables[i].values[j])
                    {
                        writer.WriteAttributeString("value", variables[i].values[j]);
                    }
                    if (variable != variables[i] && GetVariableValue(property, variables[i]) == variables[i].values[j])
                    {
                        writer.WriteAttributeString("value", variables[i].values[j]);
                    }
                }
            }
            else
            {
                writer.WriteAttributeString("selected", "false");
            }
        }
        writer.WriteEndDocument();
        writer.Flush();

        property.variableValues[Index] = sw.ToString();
    }


    public override EnumerableType Copy()
    {
        EnumClassicType newEnum = new EnumClassicType();
        newEnum.isDefaultValue = isDefaultValue;
        return newEnum;
    }

}
