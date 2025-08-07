using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<Item> inventory = new List<Item>();

    public void Add(Item item)
    {
        inventory.Add(item);
    }
}
