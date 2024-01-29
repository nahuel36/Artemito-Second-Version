using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class InventoryItemEditor : Editor
{
    public void Show(InventoryItem item, SerializedProperty obj, VisualElement root)
    {
        VisualElement visualElem = root;
        
        TextField text = visualElem.Q<TextField>("itemName");
        text.value = item.itemName;       
        
        ObjectField itemImage = visualElem.Q<ObjectField>("itemImage");
        itemImage.objectType = typeof(Sprite);
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
    


}
