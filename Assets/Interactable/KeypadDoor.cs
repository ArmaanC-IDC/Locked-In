using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadDoor : Door
{
    [SerializeField] private string correctCode = "1234";
    [SerializeField] private GameObject keypadUIPrefab;
    private GameObject keypadUI;
    private bool isLocked = true;

    void Start()
    {
        base.Start();
        isLocked = true;
    }

    public override void OnInteract()
    {
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            keypadUI = Instantiate(keypadUIPrefab, transform.position, transform.rotation);
            keypadUI.GetComponent<KeypadUI>().OpenKeypad(this);
        }else
        {
            base.OnInteract();
        }
    }

     public bool TryUnlock(string enteredCode)
    {
        if (enteredCode == correctCode)
        {
            isLocked = false;
            Debug.Log("Door unlocked!");
            Destroy(keypadUI);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            base.OnInteract();
            return true;
        }
        else
        {
            Debug.Log("Incorrect code!");
            return false;
            // TODO: add sound or error feedback
        }
    }
}
