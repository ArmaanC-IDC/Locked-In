using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabRegistry", menuName = "EscapeRoom/PrefabRegistry", order = 0)]
public class PrefabRegistry : ScriptableObject
{
    public List<string> prefabNames = new List<string>();
    public List<GameObject> prefabs = new List<GameObject>();
    public List<string> locations = new List<string>();
    public List<string> displayNames = new List<string>();
}
