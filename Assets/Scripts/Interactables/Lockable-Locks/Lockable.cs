using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockable : Interactable
{
    public bool locked = false;
    public bool open = false;
    public Lock _lock;

    protected virtual void Start()
    {
        base.Start();
        _lock =  gameObject.GetComponent<Lock>();
    }

    public virtual void Open()
    {

    }
    
    public virtual void Close()
    {

    }
}
