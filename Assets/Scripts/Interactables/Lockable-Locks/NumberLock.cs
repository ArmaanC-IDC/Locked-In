using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberLock : Lock
{
    [SerializeField] private GameObject numberLockUIPrefab;
    private GameObject numberLockUI;

    public override void TryUnlock(Item item)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        numberLockUI = Instantiate(numberLockUIPrefab, transform.position, transform.rotation);
        numberLockUI.GetComponent<NumberLockUI>().OpenKeypad(this);
        UIManager.uiManager.UIOpen = true;
    }

    public void SubmitAttempt(string enteredCode)
    {
        UIManager.uiManager.UIOpen = false;
        Destroy(numberLockUI);
        if (codes.Contains(enteredCode))
        {
            lockable.locked = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            lockable.OnInteract(null);
        }
        else
        {
            Debug.Log("Incorrect code!");
        }
    }
}
