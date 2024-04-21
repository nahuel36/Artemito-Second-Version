using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropertiesContainer : MonoBehaviour
{
    public List<LocalProperty> local_properties;

    public void Awake()
    {
        for (int i = 0; i < local_properties.Count; i++)
        {
            local_properties[i].variablesContainer.InitializeVariables();
        } 
    }
}
