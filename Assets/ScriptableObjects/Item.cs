using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "EscapeRoom/Item", order = 0)]
public class Item : ScriptableObject {
    public string id;
    public string name;
    public string description;
    public Sprite icon;
}