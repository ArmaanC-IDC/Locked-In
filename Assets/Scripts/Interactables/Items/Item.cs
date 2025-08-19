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
    [EditableParamTag] public new string name;
    [EditableParamTag] [TextArea] public string description;
    public Sprite inventoryIcon;

    [EditableParamTag] public string code; //leave null for non-keys
}