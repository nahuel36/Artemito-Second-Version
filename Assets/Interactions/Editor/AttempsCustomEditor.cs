using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class AttempsCustomEditor : Editor
{
    public VisualElement ShowGUI(AttempsContainer attempsContainer, UnityEngine.Object myTarget, bool isDuplicate)
    {
        VisualElement InvAttempsVE = new VisualElement();

        Label tittle = new Label("Attemps");

        tittle.style.unityFontStyleAndWeight = FontStyle.Bold;

        InvAttempsVE.Add(tittle);

        
        CustomListView<InteractionsAttemp> listViewAttemps = new CustomListView<InteractionsAttemp>();
        listViewAttemps.ItemsSource = attempsContainer.attemps;
        listViewAttemps.ItemContent = (indexAttemp) =>
        {
            VisualElement interactionVE = new VisualElement();
            Foldout foldout = new Foldout();
            foldout.text = "attemp " + (indexAttemp + 1).ToString();
            interactionVE.Add(foldout);
            VisualElement visualElement = new VisualElement();
            InteractionsCustomEditor interactionCustomEditor = (InteractionsCustomEditor)CreateInstance(typeof(InteractionsCustomEditor));
            interactionCustomEditor.ShowGUI(visualElement, attempsContainer.attemps[indexAttemp].interactions, myTarget, isDuplicate, true);
            foldout.Add(visualElement);
            return interactionVE;
        };

        listViewAttemps.CopyItem = (attempOrigin) =>
        {
            return attempOrigin.CopyItem(attempOrigin);
        };

        listViewAttemps.highlightedColor = Color.white * 0.5f;
        listViewAttemps.OnAdd = () => { return new InteractionsAttemp(); };

        

        listViewAttemps.Init(InvAttempsVE, true);



        return InvAttempsVE;
    }
}
