using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookshelfUI : MonoBehaviour
{
    public GameObject readBookUIPrefab;

    public List<Book> books;

    public void Init(List<Book> b, List<Color> c)
    {
        books = b;

        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            GameObject panel = transform.GetChild(1).GetChild(i).GetChild(0).GetChild(0).gameObject;

            //update color
            panel.GetComponent<Image>().color = c[i];

            //update title
            panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = b[i].title;

            //update description
            panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = b[i].author;
            
        }
    }

    public void OpenBook(int num)
    {
        GameObject readBookUI = Instantiate(readBookUIPrefab, transform.position, transform.rotation);
        readBookUI.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = books[num].content;
        readBookUI.GetComponent<BackButton>().exitUI = false;
    }

    public void CloseMenu()
    {
        Destroy(gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.uiManager.UIOpen = false;
    }
}
