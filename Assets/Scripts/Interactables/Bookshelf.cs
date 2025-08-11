using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookshelf : Interactable
{
    public GameObject bookshelfUIPrefab;
    public List<Book> books;
    public List<Color> colors;

    override public void OnInteract(Item item)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameObject menu = Instantiate(bookshelfUIPrefab, transform.position, transform.rotation);
        menu.GetComponent<BookshelfUI>().Init(books, colors);
        UIManager.uiManager.UIOpen = true;
    } 
}
