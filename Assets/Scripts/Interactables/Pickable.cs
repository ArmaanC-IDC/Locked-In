using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickable : Interactable
{
    public Item item;
    public InventoryManager inventory;

    protected new void Start()
    {
        base.Start();
        
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();

        item = GetComponent<Item>();

        if (inventory==null)
        {
            Debug.LogError("\"Player\" not found at top level, or player has no inventory manager attached");
            Destroy(this);
        }

        if (item==null){
            Debug.LogError("No item data attached to " + gameObject.name);
        }
    }

    public override void OnInteract(Item item)
    {
        inventory.Add(this.item);
        Destroy(gameObject);
    }
}
