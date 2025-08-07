using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager {get; private set;}

    public bool UIOpen {get; set;}

    void Start()
    {
        if (gameManager==null)
        {
            gameManager = this;
        }else
        {
            Debug.LogError("Cannot have more than one GameManager");
            Destroy(this);
        }
    }
}
