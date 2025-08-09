using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : Door
{
    [SerializeField] private List<string> codes = new List<string>();
    private bool isLocked = true;

    protected new void Start()
    {
        base.Start();
        isLocked = true;
    }

    public override void OnInteract(Item item)
    {
        if (isLocked)
        {
            if (item?.codes==null)
            {
                Debug.Log("Key must be used.");
                return ;
            }
            foreach (string code in codes)
            {
                if (item.codes.Contains(code))
                {
                    base.OnInteract(item);
                    isLocked = false;
                    return ;
                }
            }
            Debug.Log("That is the wrong key.");
        }else
        {
            base.OnInteract(item);
        }
    }
}
