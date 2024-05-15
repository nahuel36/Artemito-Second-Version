using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectType : PropertyObjectType
{
    public RoomObjectType()
    {
        Index = 1;
        TypeName = "Room Object";
    }


    private void OnEnable()
    {
        Index = 1;
        TypeName = "Room Object";
    }
}
