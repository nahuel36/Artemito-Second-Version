using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;

[System.Serializable]
public class InteractionsCustomEditor : Editor
{

    CustomListView<Interaction> listCustom;
    List<Interaction> interactions;

    [SerializeField] VisualTreeAsset InteractionsVT;
    [SerializeField] VisualTreeAsset InteractionVT;
    public void ShowGUI(VisualElement root, List<Interaction> interactions, UnityEngine.Object myTarget, bool isDuplicate, bool generateVisualTree=false) 
    {
        if (isDuplicate)
        {
            for (int i = 0; i < interactions.Count; i++)
            {
                interactions[i] = interactions[i].CopyItem(interactions[i]);
            }
        }



        if (generateVisualTree)
        {
            root.Add(InteractionsVT.CloneTree());
        }

        VisualElementsUtils.HideVisualElement(root.Q("Interaction"));
        
        listCustom = new();

        listCustom.ItemsSource = interactions;
        this.interactions = interactions;

        
        Func<int, VisualElement> itemContent = (i) =>
        {
            Foldout foldout = new Foldout();

            foldout.text = "interaction " + (i+1).ToString();

            int index = i;

            VisualElement visualElem = InteractionVT.CloneTree();

            InteractionSelect interactionSelect = (InteractionSelect)CreateInstance(typeof(InteractionSelect));

            visualElem.Q("InteractionSelect").Clear();
            visualElem.Q("InteractionSelect").Add(interactionSelect.ShowAndConfigure(interactions[index]));
            interactionSelect.OnChangeTypeEvent += (inter) => {
                UpdateAction(inter, index, visualElem.Q("Action"));
            };
            interactionSelect.OnChangeSubTypeEvent += (inter) =>
            {
                UpdateAction(inter, index, visualElem.Q("Action"));
            };
            UpdateAction(interactions[index], index, visualElem.Q("Action"));
            foldout.Add(visualElem);
            return foldout;
        };

        listCustom.ItemContent = itemContent;

        //listCustom.OnChangeItem += OnChange;

        listCustom.OnAdd = () => OnAdded(myTarget);

        listCustom.OnReorderItem += (reorderList) => { SaveTargetChanges(myTarget); };

        listCustom.OnRemoveItem += (list) => { SaveTargetChanges(myTarget); };

        root.RegisterCallback<ChangeEvent<string>>((evt) => { SaveTargetChanges(myTarget); });

        listCustom.highlightedColor = Color.white * 0.5f;

        listCustom.CopyItem = (interOrigin) => 
        {
            return interOrigin.CopyItem(interOrigin);
        };

        listCustom.Init(root.Q<VisualElement>("CustomListView"));
    }

    private void UpdateAction(Interaction interaction, int index, VisualElement visualElem)
    {
        if (interaction != interactions[index]) return;

        visualElem.Clear();

        if (string.IsNullOrEmpty(interaction.type) || string.IsNullOrEmpty(interaction.subtype)) return;

        List<string> files = FileUtils.GetFilesList(Application.dataPath + "/Interactions/" + interaction.type + "/"  + interaction.subtype + "/");

        for (int i = 0; i < files.Count; i++)
        {
            InteractionAction var = AssetDatabase.LoadAssetAtPath<InteractionAction>("Assets/Interactions/" + interaction.type + "/"  + interaction.subtype + "/" + files[i]);
            if (var != null && ((interaction.action != null && var.GetType() != interaction.action.GetType()) || interaction.action == null))
            {
                interaction.action = (InteractionAction)ScriptableObject.CreateInstance(var.GetType());
            }

        }

        if (interaction.action != null)
        {
            interaction.action.SetEditorField(visualElem, interaction);
        }
    }

    private void SaveTargetChanges(UnityEngine.Object myTarget)
    {
        EditorUtility.SetDirty(myTarget);
    }

    private Interaction OnAdded(UnityEngine.Object myTarget)
    {
        SaveTargetChanges(myTarget);
        return new Interaction();
    }

}
