using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
public class InteractionsCustomEditor : Editor
{
    Dictionary<Interaction, SubtypeSelector> subTypeSelectors;

    CustomListView<Interaction> listCustom;
    List<Interaction> interactions;

    [SerializeField] VisualTreeAsset InteractionsVT;
    [SerializeField] VisualTreeAsset InteractionVT;
    public void ShowGUI(VisualElement root, List<Interaction> interactions, UnityEngine.Object myTarget, bool generateVisualTree=false) {

        if (generateVisualTree)
        {
            root.Add(InteractionsVT.CloneTree());
        }

        subTypeSelectors = new Dictionary<Interaction, SubtypeSelector>();
        
        root.Q("Interaction").visible = false;
        root.Q("Interaction").StretchToParentSize();

        listCustom = new();

        listCustom.ItemsSource = interactions;
        this.interactions = interactions;

        Func<int, VisualElement> itemContent = (i) =>
        {
            int index = i;

            VisualElement visualElem = InteractionVT.CloneTree();

            visualElem.Q("InteractionSelect").visible = false;
            visualElem.Q("InteractionSelect").StretchToParentSize();

            visualElem.Q("ObjectTypeSelect").visible = false;
            visualElem.Q("ObjectTypeSelect").StretchToParentSize();


            InteractionSelect select3 = (InteractionSelect)CreateInstance(typeof(InteractionSelect));
            
            visualElem.Add(select3.ShowAndConfigure(interactions[index]));
            select3.OnChangeTypeEvent += (inter) => {
                UpdateSelector(inter, index, visualElem);
                UpdateAction(inter, index, visualElem);
            };
            select3.OnChangeSubTypeEvent += (inter) =>
            {
                UpdateAction(inter, index, visualElem);
            };
            UpdateSelector(interactions[index], index, visualElem);
            UpdateAction(interactions[index], index, visualElem);
            return visualElem;
        };

        listCustom.ItemContent = itemContent;

        listCustom.ItemHeight = itemHeight;

        //listCustom.OnChangeItem += OnChange;

        listCustom.OnAdd = () => OnAdded(myTarget);

        listCustom.OnReorderItem += (element, index) => { SaveTargetChanges(myTarget); };

        listCustom.OnRemoveItem += (element, index) => { SaveTargetChanges(myTarget); };

        root.RegisterCallback<ChangeEvent<string>>((evt) => { SaveTargetChanges(myTarget); });

        listCustom.reOrderMode = CustomListView<Interaction>.ReOrderModes.animatedDynamic;

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

        if (string.IsNullOrEmpty(interaction.type) || string.IsNullOrEmpty(interaction.subtype)) return;

        List<string> files = FileUtils.GetFilesList(Application.dataPath + "/Interactions/" + interaction.type + "/"  + interaction.subtype + "/");

        InteractionAction action = null;

        for (int i = 0; i < files.Count; i++)
        {
            InteractionAction var = AssetDatabase.LoadAssetAtPath<InteractionAction>("Assets/Interactions/" + interaction.type + "/"  + interaction.subtype + "/" + files[i]);
            if (var != null)
                action = var;
        }

        if (action != null)
        {
            action.SetEditorField(visualElem, interaction);
        }
    }

    private void UpdateSelector(Interaction interaction, int index, VisualElement element)
    {
        if (interaction != interactions[index]) return;

        if (!string.IsNullOrEmpty(interaction.type))
        {
            if (interaction.type == "Inventory" && !subTypeSelectors.ContainsKey(interaction))
            {
                subTypeSelectors.Add(interaction, (SelectInventory)CreateInstance(typeof(SelectInventory)));
            }
            else if (interaction.type != "Inventory" && subTypeSelectors.ContainsKey(interaction))
            {
                ((SelectInventory)subTypeSelectors[interaction]).Remove(element);
                subTypeSelectors.Remove(interaction);
            }
        }

        if (subTypeSelectors.ContainsKey(interaction))
        {
            ((SelectInventory)subTypeSelectors[interaction]).Add(interaction, element);
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

    private StyleLength itemHeight(int index)
    {
        return new StyleLength(StyleKeyword.Auto);
    }
}
