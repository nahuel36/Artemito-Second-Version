using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Pnc/InventoryList", order = 1)]
public class InventoryList : ScriptableObject
{
    public InventoryItem[] items = new InventoryItem[0];
    public int specialIndex = 0;

    public List<string> GetListOfItems()
    {
        List<string> list = new List<string>();

        foreach (InventoryItem item in items)
        {
            list.Add(item.itemName);
        }

        return list;
    }

    public int GetIndexBySpecialIndex(int specialIdx)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].specialIndex == specialIdx)
            {
                return i;
            }
        }

        return -1;
    }

}
