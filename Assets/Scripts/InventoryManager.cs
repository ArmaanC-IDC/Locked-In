using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InputAction swapControls;

    private List<Item> inventory = new List<Item>();
    private UIManager uiManager;

    public int numInventorySlots = 10;
    private int _activeInventorySlot;  // backing field. So other one's getter doesn't call itself
    public int activeInventorySlot
    {
        get { return _activeInventorySlot; }
        set
        {
            if(value > numInventorySlots - 1)
            {
                _activeInventorySlot = 0;
            }
            else if (value < 0)
            {
                _activeInventorySlot = numInventorySlots - 1;
            }
            else
            {
                _activeInventorySlot = value;
            }
            if (uiManager!=null) uiManager.UpdateInventoryDisplay();
        }
    }

    void Start()
    {
        activeInventorySlot = 0;
        uiManager = UIManager.uiManager;
    }

    public void Add(Item item)
    {
        if (item==null){
            Debug.LogError("Null item added to inventory");
        }
        inventory.Add(item);
        uiManager.UpdateInventoryDisplay();
    }

    #region enable/disable controls
    private void OnEnable()
    {
        swapControls.Enable();
        swapControls.performed += OnControlSwap;
    }
    
    private void OnDisable()
    {
        swapControls.performed -= OnControlSwap;
        swapControls.Disable();
    }
    #endregion

    private void OnControlSwap(InputAction.CallbackContext context)
    {
        float inputVal = context.ReadValue<float>();
        activeInventorySlot += (int)(Mathf.Clamp(inputVal, -1f, 1f) * -1f);
    }

    public List<Item> GetInventory()
    {
        return inventory;
    }

    public Item GetActiveItem()
    {
        if (activeInventorySlot >= inventory.Count)
        {
            return null;
        }
        return inventory[activeInventorySlot];
    }
}
