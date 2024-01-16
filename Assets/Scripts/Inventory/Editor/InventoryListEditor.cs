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

    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();

        images = new List<Image>();

        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();

        root.Add(labelFromUXML);

        imagesContainer = new VisualElement();

        imagesContainer.style.flexDirection = FlexDirection.Row;     

        for (int i = 0; i < serializedObject.FindProperty("items").arraySize; i++)
        {
            AddImage(i);
        }

        root.Add(imagesContainer);

        Button addButton = new Button();
        addButton.text = "+";
        addButton.clicked += Add;
        root.Add(addButton);

        eraseButton = new Button();
        eraseButton.text = "-";
        eraseButton.clicked += Erase;
        eraseButton.visible = false;
        root.Add(eraseButton);

        return root;
    }

    private void AddImage(int i)
    {
        
        Image image = new Image();
        image.style.width = 100;
        image.style.height = 100; 
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
        root.Remove(itemSelectedVisualElement);
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

        AddImage(index);

        serializedObject.Update();
    }

    private void OnClick(int specialIndex)
    {
        if (root.Contains(itemSelectedVisualElement))
            root.Remove(itemSelectedVisualElement);

        selectedIndex = ((InventoryList)target).GetIndexBySpecialIndex(specialIndex);

        itemSelectedVisualElement = new InventoryItemEditor().Show(((InventoryList)target).items[selectedIndex], serializedObject.FindProperty("items").GetArrayElementAtIndex(selectedIndex));
                
        if (!root.Contains(itemSelectedVisualElement))
        {
            root.Add(itemSelectedVisualElement);
        }

        eraseButton.visible = true;
        
    }

}

