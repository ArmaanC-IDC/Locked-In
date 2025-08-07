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
        Debug.Log("Here");
        base.Start();
        hinge = transform.Find("Hinge");
        if (hinge==null)
        {
            Debug.LogError("Hinge not found");
        }
    }

    override public void OnInteract()
    {
        if (doorOpen)
        {
            hinge.rotation = hinge.rotation * Quaternion.Euler(0, 90 * -1 * openDir, 0);
            doorOpen = false;
        }else 
        {
            hinge.rotation = hinge.rotation * Quaternion.Euler(0, 90 * openDir, 0);
            doorOpen = true;
        }
    }
}
