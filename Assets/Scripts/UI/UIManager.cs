using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager {get; private set;}

    public bool UIOpen {get; set;}

    private Transform hotbar;
    private InventoryManager inventory;

    void Awake()
    {
        if (uiManager==null)
        {
            uiManager = this;
            Debug.Log("UI manager created");
        }else
        {
            Debug.LogError("Cannot have more than one UI Manager");
            Destroy(this);
        }
    }

    void Start()
    {
        hotbar = transform.Find("Hotbar");
        if (hotbar==null)
        {
            Debug.LogError("Could not find \"Hotbar\" as direct child");
        }

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        if (inventory==null)
        {
            Debug.LogError("\"Player\" not found at top level, or it has no inventory manager attached");
            Destroy(this);
        }
    }

    public void UpdateInventoryDisplay()
    {
        //update the active slot
        for (int i = 0; i < hotbar.childCount; i++)
        {
            Image currentChild = hotbar.GetChild(i).gameObject.GetComponent<Image>();
            Color currentColor = currentChild.color;
            if (i!=inventory.activeInventorySlot)
            {
                currentColor.a = 0;
            }else
            {
                currentColor.a = 1;
            }
           currentChild.color = currentColor;
        }
    }
}
