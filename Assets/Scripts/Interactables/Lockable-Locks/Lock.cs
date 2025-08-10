using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public List<string> codes = new List<string>();
    public Lockable lockable;

    protected virtual void Start()
    {
        lockable = gameObject.GetComponent<Lockable>();
        if (lockable==null)
        {
            Debug.LogError("Must have a lockable for every lock");
            Destroy(gameObject);
        }

        lockable.locked = true;
    }

    public virtual void TryUnlock(Item item)
    {

    }
}
