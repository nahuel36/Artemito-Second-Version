using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
public static class FoldoutUtils 
{
    public static void SetFoldout(VisualElement VE, bool expandedVariable, string foldoutText, Func<bool,VisualElement> content) 
    {
        Foldout fold = new Foldout();
        fold.text = foldoutText; 
        fold.value = expandedVariable;
        fold.RegisterValueChangedCallback<bool>((value) => {
            expandedVariable = value.newValue;
            if (value.newValue == true)
            {
                VE.Add(content(value.newValue));
                
            }
            else
            {
                VE.Clear();
                VE.Add(fold);
            }
        });
        VE.Add(fold);
        if (fold.value)
        {
            VE.Add(content(true));
        }
    }
}
