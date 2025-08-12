using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Note,
    Key
}

[CreateAssetMenu(fileName = "Item", menuName = "EscapeRoom/Item", order = 0)]
public class Item : ScriptableObject {
    public ItemType type;
    public string id;
    public string name;
    [TextArea] public string description;
    public Sprite inventoryIcon;

    public List<string> codes; //leave null for non-keys
}