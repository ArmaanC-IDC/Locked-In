using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Stacker : MonoBehaviour
{
    public GameObject prefab;
    public int num;
    public Vector3 offset;

    public bool generate = false;
    
    void Update()
    {
        if (!Application.isPlaying && generate)
        {
            generate = false;

            //add new children
            for (int x = 0; x<num; x++)
            {
                GameObject obj = Instantiate(prefab, transform.position, transform.rotation, transform.parent);
                obj.transform.position = offset * x + transform.position;
            }
        }
    }
}
