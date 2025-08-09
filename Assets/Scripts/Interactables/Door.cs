using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public int openDir = 1;

    private Transform hinge;
    private bool doorOpen = false;

    protected new void Start()
    {
        base.Start();
        hinge = transform.Find("Hinge");
        if (hinge==null)
        {
            Debug.LogError("Hinge not found");
        }
    }

    override public void OnInteract(Item item) //KeypadDoor.cs calls this with null as item bcs doesn't use item now
    {
        if (doorOpen)
        {
            hinge.Rotate(0, 90 * -1 * openDir, 0, Space.World);
            doorOpen = false;
        }else 
        {
            hinge.Rotate(0, 90 * openDir, 0, Space.World);
            doorOpen = true;
        }
    }
}
