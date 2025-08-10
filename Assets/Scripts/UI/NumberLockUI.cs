using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberLockUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentCodeText;
    private string currentEnteredCode = "";
    private NumberLock numberLock;

    public void OpenKeypad(NumberLock l)
    {
        numberLock = l;
    }

    public void OnButtonPress(int number){
        currentEnteredCode += number.ToString();
        currentCodeText.text = currentEnteredCode;
    }

    public void OnSubmitButtonPress(){
        numberLock.SubmitAttempt(currentEnteredCode);
    }
}
