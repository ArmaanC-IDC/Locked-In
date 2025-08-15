using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : Lock
{
    public override void TryUnlock(Item item)
    {
        if (lockable.locked)
        {
            if (item?.code==null)
            {
                Debug.Log("Key must be used.");
                return ;
            }
            if (code==item.code)
            {
                lockable.locked = false;
                lockable.OnInteract(item);
                return ;
            }
            Debug.Log("That is the wrong key.");
        }else
        {
            lockable.locked = false;
            lockable.OnInteract(item);
        }
    }
}
