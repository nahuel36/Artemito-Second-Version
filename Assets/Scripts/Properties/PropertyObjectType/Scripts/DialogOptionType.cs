using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DialogOptionType : PropertyObjectType
{

    public DialogOptionType()
    {
        Index = 3;
        TypeName = "Dialog Option";
    }

    private void OnEnable()
    {
        Index = 3;
        TypeName = "Dialog Option";
    }

    public override EnumerableType Copy()
    {
        DialogOptionType dialogOptionType = new DialogOptionType();
        dialogOptionType.Index = Index;
        dialogOptionType.TypeName = TypeName;
        return dialogOptionType;
    }
}
