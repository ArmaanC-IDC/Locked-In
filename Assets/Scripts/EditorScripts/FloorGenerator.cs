using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FloorGenerator : MonoBehaviour
{
    public GameObject floorTilePrefab;
    public int xMax = 5;
    public int yMax = 5;
    public float tileSize;

    public bool generate = false;
    
    void Update()
    {
        if (!Application.isPlaying && generate)
        {
            generate = false;

            //clear current children
            for (int i = transform.childCount - 1; i>=0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            //add new floor tiles
            for (int x = 0; x<xMax; x++)
            {
                for (int y = 0; y<yMax; y++)
                {
                    GameObject tile = Instantiate(floorTilePrefab, transform);
                    tile.transform.localPosition = new Vector3(x * tileSize, 0, y*tileSize);
                }
            }
        }
    }
}
