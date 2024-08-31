using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropertiesContainerInScene : MonoBehaviour
{
    public List<LocalProperty> local_properties;

    public void Awake()
    {
        if(Application.isPlaying)
            for (int i = 0; i < local_properties.Count; i++)
            {
                local_properties[i].variablesContainer.InitializeVariables();
            } 
    }
}
