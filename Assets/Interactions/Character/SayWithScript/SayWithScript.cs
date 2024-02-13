using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SayWithScript : InteractionAction
{
    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
        base.SetEditorField(visualElement, interaction);

        visualElement.Add(new Label("test"));

    }
}
