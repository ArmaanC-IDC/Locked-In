using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : Lockable
{
    public Vector3 closedPos;
    public Vector3 openPos;
    private Vector3 originalPos;

    override protected void Start()
    {
        base.Start();
        originalPos = mesh.transform.position;
    }

    override public void OnInteract(Item item)
    {
        if (locked)
        {
            _lock.TryUnlock(item);
            return ;
        }

        Debug.Log("Here");
        if (open)
        {
            open = false;
            Close();
        }
        else
        {
            open = true;
            Open();
        }
    }

    override public void Open()
    {
        mesh.transform.position = originalPos + openPos;
    }

    override public void Close()
    {
        mesh.transform.position = originalPos + closedPos;
    }
}
