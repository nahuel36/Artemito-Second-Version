using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class InventoryItemEditor : Editor
{
    public static EventCallback<ChangeEvent<UnityEngine.Object>> OnChangeItemImageCallback;
    public static InventoryItem actualItem;
    public static void Show(InventoryItem item, SerializedProperty obj, VisualElement root)
    {
        VisualElement visualElem = root;
        
        TextField text = visualElem.Q<TextField>("itemName");
        text.value = item.itemName;       
        
        ObjectField itemImage = visualElem.Q<ObjectField>("itemImage");
        itemImage.objectType = typeof(Sprite);

        if (OnChangeItemImageCallback == null)
            OnChangeItemImageCallback = (evt) => OnChangeItemImage(evt, item);
        else
            itemImage.UnregisterValueChangedCallback(OnChangeItemImageCallback);

        actualItem = item;

        itemImage.RegisterValueChangedCallback(OnChangeItemImageCallback);
        
        itemImage.value = item.normalImage;

        ObjectField selectedImage = visualElem.Q<ObjectField>("selectedImage");
        selectedImage.objectType = typeof(Sprite);
        selectedImage.value = item.selectedImage;

        Toggle startWith = visualElem.Q<Toggle>("startWith");
        startWith.value = item.startWithThisItem;

        FloatField cuantity = visualElem.Q<FloatField>("cuantity");
        cuantity.value = item.cuantity;

        IntegerField priority = visualElem.Q<IntegerField>("priority");
        priority.value = item.priority;
    }

    private static void OnChangeItemImage(ChangeEvent<UnityEngine.Object> evt, InventoryItem item)
    {
        if (item != actualItem) return;

        item.normalImage = (Sprite)evt.newValue;
    }
}
