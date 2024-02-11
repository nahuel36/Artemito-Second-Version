using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class AttempsCustomEditor : Editor
{

    public VisualElement ShowGUI(AttempsContainer attempsContainer, UnityEngine.Object myTarget)
    {
        VisualElement InvAttempsVE = new VisualElement();
        CustomListView<InteractionsAttemp> listViewAttemps = new CustomListView<InteractionsAttemp>();
        listViewAttemps.ItemsSource = attempsContainer.attemps;
        listViewAttemps.ItemContent = (indexAttemp) =>
        {
            InteractionsCustomEditor interactionCustomEditor = (InteractionsCustomEditor)CreateInstance(typeof(InteractionsCustomEditor));
            VisualElement interactionVE = new VisualElement();
            interactionCustomEditor.ShowGUI(interactionVE, attempsContainer.attemps[indexAttemp].interactions, myTarget, true);
            return interactionVE;
        };
        listViewAttemps.highlightedColor = Color.white * 0.5f;
        listViewAttemps.Init(InvAttempsVE, true);
        listViewAttemps.OnAdd = () => { return new InteractionsAttemp(); };
        return InvAttempsVE;
    }
}
