using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Book", menuName = "EscapeRoom/Book", order = 0)]
public class Book : ScriptableObject {
    public string title;
    public string author;
    [TextArea] public string content;
}