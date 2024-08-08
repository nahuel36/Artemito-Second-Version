using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;

[CustomEditor(typeof(InventoryList))]
public class InventoryListEditor : Editor
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    VisualElement root;
    VisualElement imagesContainer;
    VisualElement itemSelectedVisualElement;
    List<Image> images;
    int selectedIndex;
    Button eraseButton;
    StyleLength itemWidth, itemHeight;
    VisualElement selectedItem;

    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        images = new List<Image>();

        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();

        VisualElement item = labelFromUXML.Q<VisualElement>("Item");
        VisualElementsUtils.HideVisualElement(item);

        itemWidth = item.style.width;
        itemHeight = item.style.height;

        imagesContainer = labelFromUXML.Q<VisualElement>("ItemsContainer");

        imagesContainer.style.flexDirection = FlexDirection.Row;

        imagesContainer.style.height = 100;

        for (int i = 0; i < serializedObject.FindProperty("items").arraySize; i++)
        {
            AddImage(i, itemWidth, itemHeight);
        }


        Button addButton = labelFromUXML.Q<Button>("add");
        addButton.clicked += Add;

        eraseButton = labelFromUXML.Q<Button>("remove");
        eraseButton.clicked += Erase;
        eraseButton.visible = false;

        selectedItem = labelFromUXML.Q<VisualElement>("InventoryItem");
        selectedItem.visible = false;

        root.Add(labelFromUXML);

        root.RegisterCallback<ChangeEvent<string>>(OnSomeChange);

        return root;
    }

    private void OnSomeChange(ChangeEvent<string> evt)
    {
        EditorUtility.SetDirty(target);
    }

    private void AddImage(int i, StyleLength width, StyleLength height)
    {
        
        Image image = new Image();
        image.style.width = width;
        image.style.height = height; 
        image.sprite = ((UnityEngine.Sprite)serializedObject.FindProperty("items").GetArrayElementAtIndex(i).FindPropertyRelative("normalImage").objectReferenceValue);
        image.scaleMode = ScaleMode.ScaleToFit;
        int clickedIndex = serializedObject.FindProperty("items").GetArrayElementAtIndex(i).FindPropertyRelative("specialIndex").intValue;
        image.AddManipulator(new Clickable(evt =>
        {
            OnClick(clickedIndex);
        }));
        images.Add(image);
        imagesContainer.Add(image);
    }

    private void Erase()
    {
        serializedObject.FindProperty("items").DeleteArrayElementAtIndex(selectedIndex);
        serializedObject.ApplyModifiedProperties();
        selectedItem.visible = false;
        imagesContainer.Remove(images[selectedIndex]);
        images.Remove(images[selectedIndex]);
        selectedIndex = -1;
        eraseButton.visible = false;
    }

    private void Add()
    {
        serializedObject.FindProperty("items").arraySize++;

        serializedObject.ApplyModifiedProperties();

        serializedObject.FindProperty("specialIndex").intValue++;
        int index = serializedObject.FindProperty("items").arraySize - 1;
        serializedObject.FindProperty("items").GetArrayElementAtIndex(index).FindPropertyRelative("specialIndex").intValue = serializedObject.FindProperty("specialIndex").intValue;
        serializedObject.FindProperty("items").GetArrayElementAtIndex(index).FindPropertyRelative("itemName").stringValue = "new item " + serializedObject.FindProperty("specialIndex").intValue;

        string key = serializedObject.FindProperty("items").GetArrayElementAtIndex(index).FindPropertyRelative("specialIndex").intValue.ToString();

        serializedObject.ApplyModifiedProperties();

        OnClick(serializedObject.FindProperty("items").GetArrayElementAtIndex(index).FindPropertyRelative("specialIndex").intValue);

        AddImage(index,itemWidth,itemHeight);

        serializedObject.Update();
    }

    private void SetColor(Image image, Color color, float borderWidth)
    {
        image.style.borderBottomWidth = borderWidth;
        image.style.borderTopWidth = borderWidth;
        image.style.borderLeftWidth = borderWidth;
        image.style.borderRightWidth = borderWidth;
        image.style.borderBottomColor = color;
        image.style.borderTopColor = color;
        image.style.borderLeftColor = color;
        image.style.borderRightColor = color;
    }

    private void OnClick(int specialIndex)
    {
        selectedIndex = ((InventoryList)target).GetIndexBySpecialIndex(specialIndex);

        InventoryItemEditor.Show(((InventoryList)target).items[selectedIndex], serializedObject.FindProperty("items").GetArrayElementAtIndex(selectedIndex), selectedItem);

        selectedItem.visible = true;

        eraseButton.visible = true;

        for (int i = 0; i < images.Count; i++)
        {
            if (i == selectedIndex)
            {
                SetColor(images[i], Color.yellow, 5);
            }
            else
            {
                SetColor(images[i], Color.clear, 0);
            }
        }
    }

}

