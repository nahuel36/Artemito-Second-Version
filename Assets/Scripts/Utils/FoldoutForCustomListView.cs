using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
public class FoldoutForCustomListView<T>
{
    public List<Func<VisualElement>> content;
    public List<Action<bool>> changeVariable;
    private bool addedReorder = false;
    public void SetFoldout(VisualElement VE, bool expandedVariable, string foldoutText, int index, CustomListView<T> customListView) 
    {
        Foldout fold = new Foldout();
        fold.text = foldoutText; 
        fold.value = expandedVariable;
        fold.RegisterValueChangedCallback<bool>((value) => {
            expandedVariable = value.newValue;
            if (value.newValue == true)
            {
                changeVariable[index](true);
                VE.Add(content[index]());
            }
            else
            {
                changeVariable[index](false);
                VE.Clear();
                VE.Add(fold);
            }
        });
        VE.Add(fold);
        if (fold.value)
        {
            VE.Add(content[index]());
        }
        if (!addedReorder)
        { 
            customListView.OnReorderItem += (reorderlist) =>
            { 
                List<Func<VisualElement>> newListContent = new List<Func<VisualElement>>();
                List<Action<bool>> newListChange = new List<Action<bool>>();
                for (int i = 0; i < content.Count; i++)
                {
                    newListContent.Add(content[reorderlist[i]]);
                    newListChange.Add(changeVariable[reorderlist[i]]);
                }
                changeVariable = newListChange;
                content = newListContent;
            };
            addedReorder = true;
        }
    }
}
