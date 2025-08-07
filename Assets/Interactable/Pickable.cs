using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickable : Interactable
{
    public Item item;
    public InventoryManager inventory;

    void Start()
    {
        base.Start();
        
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();

        if (inventory==null)
        {
            Debug.LogError("\"Player\" not found at top level, or player has no inventory manager attached");
            Destroy(this);
        }
    }

    public override void OnInteract()
    {
        inventory.Add(item);
        Destroy(gameObject);
    }
}
