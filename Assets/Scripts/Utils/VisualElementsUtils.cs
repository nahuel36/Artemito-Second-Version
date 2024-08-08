using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class VisualElementsUtils 
{

    public static void ShowVisualElement(VisualElement element)
    {
        element.visible = true;

        StyleEnum<Position> pos = new StyleEnum<Position>();
        pos.value = Position.Relative;
        element.style.position = pos;
    }

    public static void HideVisualElement(VisualElement element)
    {
        element.visible = false;
        element.StretchToParentSize();
    }
}
