using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class InventoryItemEditor : Editor
{
    public static EventCallback<ChangeEvent<string>> OnChangeItemNameCallback;
    public static EventCallback<ChangeEvent<UnityEngine.Object>> OnChangeItemImageCallback;
    public static EventCallback<ChangeEvent<UnityEngine.Object>> OnChangeItemSelectedImageCallback;
    public static EventCallback<ChangeEvent<bool>> OnChangeItemStartWithCallback;
    public static EventCallback<ChangeEvent<float>> OnChangeItemCuantityCallback;
    public static EventCallback<ChangeEvent<int>> OnChangeItemPriorityCallback;

    public static InventoryItem actualItem;
    public static void Show(InventoryItem item, SerializedProperty obj, VisualElement visualElem)
    {
        actualItem = item;
        
        TextField text = visualElem.Q<TextField>("itemName");
        text.value = item.itemName;
        ChangeSomeItemContent(ref OnChangeItemNameCallback, OnChangeItemName, text, item);
        
        ObjectField itemImage = visualElem.Q<ObjectField>("itemImage");
        itemImage.objectType = typeof(Sprite);
        itemImage.value = item.normalImage;
        ChangeSomeItemContent(ref OnChangeItemImageCallback, OnChangeItemImage , itemImage, item);
        
        ObjectField selectedImage = visualElem.Q<ObjectField>("selectedImage");
        selectedImage.objectType = typeof(Sprite);
        selectedImage.value = item.selectedImage;
        ChangeSomeItemContent(ref OnChangeItemSelectedImageCallback, OnChangeSelectedItemImage, selectedImage, item);

        Toggle startWith = visualElem.Q<Toggle>("startWith");
        startWith.value = item.startWithThisItem;
        ChangeSomeItemContent(ref OnChangeItemStartWithCallback, OnChangeItemStartWith, startWith, item);

        FloatField cuantity = visualElem.Q<FloatField>("cuantity");
        cuantity.value = item.cuantity;
        ChangeSomeItemContent(ref OnChangeItemCuantityCallback, OnChangeItemCuantity, cuantity, item);

        IntegerField priority = visualElem.Q<IntegerField>("priority");
        priority.value = item.priority;
        ChangeSomeItemContent(ref OnChangeItemPriorityCallback, OnChangeItemPriority, priority, item);

        LocalAndGlobalProperties properties = (LocalAndGlobalProperties)CreateInstance(typeof(LocalAndGlobalProperties));

        properties.CreateGUI(item.local_properties, visualElem.Q("LocalAndGlobalProperties"));
    }

    private static void ChangeSomeItemContent<T>(ref EventCallback<ChangeEvent<T>> callback,Action<ChangeEvent<T>,InventoryItem> function, BaseField<T> field, InventoryItem item) 
    {
        if (callback == null)
            callback = (ChangeEvent<T> evt) => function(evt, item);
        else
            field.UnregisterValueChangedCallback(callback);

        callback = (ChangeEvent<T> evt) => function(evt, item);

        field.RegisterValueChangedCallback(callback);
    }



    private static void OnChangeItemImage(ChangeEvent<UnityEngine.Object> evt, InventoryItem item)
    {
        if (item != actualItem) return;

        item.normalImage = (Sprite)evt.newValue;
    }

    private static void OnChangeSelectedItemImage(ChangeEvent<UnityEngine.Object> evt, InventoryItem item)
    {
        if (item != actualItem) return;

        item.selectedImage = (Sprite)evt.newValue;
    }

    private static void OnChangeItemName(ChangeEvent<String> evt, InventoryItem item)
    {
        if (item != actualItem) return;

        item.itemName = evt.newValue;
    }

    private static void OnChangeItemStartWith(ChangeEvent<bool> evt, InventoryItem item)
    {
        if (item != actualItem) return;

        item.startWithThisItem = evt.newValue;
    }

    private static void OnChangeItemCuantity(ChangeEvent<float> evt, InventoryItem item)
    {
        if (item != actualItem) return;

        item.cuantity = evt.newValue;
    }

    private static void OnChangeItemPriority(ChangeEvent<int> evt, InventoryItem item)
    {
        if (item != actualItem) return;

        item.priority = evt.newValue;
    }
}
