using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "EscapeRoom/Room", order = 0)]
public class Room : ScriptableObject
{
    public int width;
    public int height;
    public List<ObjectData> objectData;
}
