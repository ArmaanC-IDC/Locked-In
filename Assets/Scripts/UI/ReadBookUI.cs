using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadBookUI : MonoBehaviour
{
    public void Init(Book book)
    {
        transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = book.content;
    }

    public void BackButton()
    {
        Destroy(gameObject);
    }
}
