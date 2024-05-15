using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterType : PropertyObjectType
{
    public CharacterType()
    {
        Index = 0;
        TypeName = "Character";
    }

    private void OnEnable()
    {
        Index = 0;
        TypeName = "Character";
    }
}
