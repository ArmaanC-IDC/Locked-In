using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    wall, //inset into the wall. like door.
    item //anything else
}

[CreateAssetMenu(fileName = "Object", menuName = "EscapeRoom/Object", order = 0)]
public class ObjectData : ScriptableObject
{
    public ObjectType type;
    public string prefabName;
    public Vector3 position;
    public Vector3 rotation;
    public List<string> paramNames;
    public List<string> paramValues;
}
