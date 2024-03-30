using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class AttempsCustomEditor : Editor
{
    private FoldoutForCustomListView<InteractionsAttemp> foldouts;
    public VisualElement ShowGUI(AttempsContainer attempsContainer, UnityEngine.Object myTarget, bool isDuplicate)
    {
        VisualElement InvAttempsVE = new VisualElement();

        Label tittle = new Label("Attemps");

        tittle.style.unityFontStyleAndWeight = FontStyle.Bold;

        InvAttempsVE.Add(tittle);

        foldouts = new FoldoutForCustomListView<InteractionsAttemp>();
        foldouts.content = new List<System.Func<VisualElement>>();
        foldouts.changeVariable = new List<System.Action<bool>>();

        CustomListView<InteractionsAttemp> listViewAttemps = new CustomListView<InteractionsAttemp>();
        listViewAttemps.ItemsSource = attempsContainer.attemps;
        listViewAttemps.ItemContent = (indexAttemp) =>
        {
            VisualElement interactionVE = new VisualElement();
            foldouts.changeVariable.Add((newvalue) => attempsContainer.attemps[indexAttemp].expandedInInspector = newvalue);
            foldouts.content.Add(() =>
            {
                VisualElement visualElement = new VisualElement();
                InteractionsCustomEditor interactionCustomEditor = (InteractionsCustomEditor)CreateInstance(typeof(InteractionsCustomEditor));
                interactionCustomEditor.ShowGUI(visualElement, attempsContainer.attemps[indexAttemp].interactions, myTarget, isDuplicate, true);
                return visualElement;
            });
            foldouts.SetFoldout(interactionVE, attempsContainer.attemps[indexAttemp].expandedInInspector, (indexAttemp + 1).ToString() + "° attemp", indexAttemp,listViewAttemps);
            return interactionVE;
        };

        listViewAttemps.CopyItem = (attempOrigin) =>
        {
            return attempOrigin.CopyItem(attempOrigin);
        };

        listViewAttemps.highlightedColor = Color.white * 0.5f;
        listViewAttemps.Init(InvAttempsVE, true);
        listViewAttemps.OnAdd = () => { return new InteractionsAttemp(); };
        return InvAttempsVE;
    }
}
