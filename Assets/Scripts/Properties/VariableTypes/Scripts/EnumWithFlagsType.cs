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
        isString = true;
    }

    private void OnEnable()
    {
        typeName = "enum with flags";
        Index = 3;
        isString = true;
    }

    public override void SetPropertyField(VisualElement root)
    {
        base.SetPropertyField(root);
#if UNITY_EDITOR
        VisualElement element = root.Q("Field");

        Toggle toggleDefault = root.Q<Toggle>("Default");
        if (toggleDefault != null)
        { 
            //root.Q<Toggle>("Default").visible = false;
            //root.Q<Toggle>("Default").StretchToParentSize();
        }

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

        variablesField.value = GetSelectedVariables(settings.EnumWithFlagVariables);

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

        variablesField.RegisterValueChangedCallback((evt) => SetVariableValue(settings.EnumWithFlagVariables, evt.newValue));

        element.Add(variablesField);

        for (int i = 0; i < settings.EnumWithFlagVariables.Count; i++)
        {
            if (settings.EnumWithFlagVariables[i].values.Count == 0)
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
                EnumFlagsField field = new EnumFlagsField((GenericEnum)0);

                int index = i;

                field.value = (GenericEnum)GetVariableValue(settings.EnumWithFlagVariables[index]);

                field.choices = settings.EnumWithFlagVariables[i].values;

                CustomEnumFlags.SetChoicesMasksByChoicesInOrder(field.choicesMasks, field.choices);

                field.RegisterValueChangedCallback((evt) => SetValue(settings.EnumWithFlagVariables, settings.EnumWithFlagVariables[index], evt.newValue));

                element.Add(field); 
            }        
        }
       
#endif
    }

    public System.Enum GetSelectedVariables(List<Settings.EnumVariablesType> variables)
    {
        if (string.IsNullOrEmpty(stringValue)) return (GenericEnum)0;


        int integerValue = 0;

        XmlReader reader = XmlReader.Create(new StringReader(stringValue));
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

    public System.Enum GetVariableValue(Settings.EnumVariablesType variable)
    {

        int integerValue = 0;

        XmlReader reader = XmlReader.Create(new StringReader(stringValue));
        try
        {
            while (reader.Read())
            {
                if (reader.IsStartElement(XmlUtility.ConvertStringToUseInXml(variable.name)))
                {
                    for (int i = 0; i < variable.values.Count; i++)
                    {
                        for (int j = 0; j < variable.values.Count; j++)
                        {
                            try
                            {
                                if (reader.GetAttribute("value" + j.ToString()) == variable.values[i])
                                {
                                    integerValue |= (1 << i);
                                }
                            }
                            catch
                            {

                            }
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


    public void SetVariableValue(List<Settings.EnumVariablesType> variables, System.Enum typeValue) {
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
                    if ((Convert.ToInt32((GenericEnum)GetVariableValue(variables[i])) & (1 << j)) != 0)
                    {
                        writer.WriteAttributeString("value" + j.ToString(), variables[i].values[j]);
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

        stringValue = sw.ToString();
    }

    public void SetValue(List<Settings.EnumVariablesType> variables, Settings.EnumVariablesType variable, System.Enum typeValue)
    {
        StringWriter sw = new StringWriter();
        XmlWriter writer = XmlWriter.Create(sw);

        writer.WriteStartDocument();
        for (int i = 0; i < variables.Count; i++)
        {
            writer.WriteStartElement(XmlUtility.ConvertStringToUseInXml(variables[i].name));
            if ((Convert.ToInt32(GetSelectedVariables(variables)) & (1 << i)) != 0)
            {
                writer.WriteAttributeString("selected", "true");

                for (int j = 0; j < variables[i].values.Count; j++)
                {
                    if (variable == variables[i] && (Convert.ToInt32(typeValue) & (1 << j))!=0)
                    {
                        writer.WriteAttributeString("value" + j.ToString(), variables[i].values[j]);
                    }
                    if (variable != variables[i] && (Convert.ToInt32((GenericEnum)GetVariableValue(variables[i])) & (1 << j)) != 0)
                    {
                        writer.WriteAttributeString("value" + j.ToString(), variables[i].values[j]);
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

        stringValue = sw.ToString();
    }


    public override EnumerableType Copy()
    {
        EnumWithFlagsType newEnum = new EnumWithFlagsType();
        newEnum.isDefaultValue = isDefaultValue;
        newEnum.stringValue = stringValue;
        newEnum.stringIngameValue = stringIngameValue;
        newEnum.changedIngame = changedIngame;
        return newEnum;
    }

}
