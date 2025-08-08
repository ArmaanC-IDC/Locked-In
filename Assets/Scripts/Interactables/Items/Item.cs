using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Note,
    Key
}

public class Item : MonoBehaviour {
    public ItemType type;
    public string id;
    public string name;
    [TextArea] public string description;
    public Sprite inventoryIcon;

    public List<string> codes; //leave null for non-keys
}