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
            else Debug.Log("No UI manager found");
        }
    }

    public void Add(Item item)
    {
        inventory.Add(item);
    }

    void Start()
    {
        activeInventorySlot = 0;
        uiManager = UIManager.uiManager;
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

    void Update()
    {
        float input = swapControls.ReadValue<float>();
        if (input!=0f)
        {
            Debug.Log(swapControls.ReadValue<float>());
        }
    }

    private void OnControlSwap(InputAction.CallbackContext context)
    {
        float inputVal = context.ReadValue<float>();
        activeInventorySlot += (int)(Mathf.Clamp(inputVal, -1f, 1f) * -1f);
    }
}
