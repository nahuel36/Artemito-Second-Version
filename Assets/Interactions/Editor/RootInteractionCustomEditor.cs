using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RootInteractionCustomEditor : MonoBehaviour
{
    public static VisualElement ShowGUI(RootInteractions interactions, UnityEngine.Object myTarget, bool isDuplicate)
    {
        VisualElement attempsVE = AttempsCustomEditor.ShowGUI(interactions.attempsContainer, myTarget, isDuplicate);
        return attempsVE;

    }

}
