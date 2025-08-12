using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackButton : MonoBehaviour
{
    public bool exitUI = true;
    public void OnBackButton()
    {
        if (exitUI)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UIManager.uiManager.UIOpen = false;
        }

        Destroy(gameObject);
    }
}
