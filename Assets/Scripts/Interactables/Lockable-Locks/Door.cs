using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Lockable
{
    public int openDir = 1;

    private Transform hinge;

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
        if (locked)
        {
            _lock.TryUnlock(item);
            return ;
        }
        if (open)
        {
            Close();
        }else 
        {
            Open();
        }
    }

    override public void Open()
    {
        hinge.Rotate(0, 90 * openDir, 0, Space.World);
        open = true;
    }

    override public void Close()
    {
        hinge.Rotate(0, 90 * -1 * openDir, 0, Space.World);
        open = false;
    }
}
