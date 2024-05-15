using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesContainerInSceneType : PropertyObjectType
{
    public PropertiesContainerInSceneType()
    {
        Index = 4;
        TypeName = "Properties Container In Scene";
    }


    private void OnEnable()
    {
        Index = 4;
        TypeName = "Properties Container In Scene";
    }
}
