using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentCodeText;
    private string currentEnteredCode = "";
    private KeypadDoor door;

    public void OpenKeypad(KeypadDoor d)
    {
        door = d;
    }

    public void OnButtonPress(int number){
        currentEnteredCode += number.ToString();
        currentCodeText.text = currentEnteredCode;
    }

    public void OnSubmitButtonPress(){
        door.TryUnlock(currentEnteredCode);
    }
}
