using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextUI : MonoBehaviour
{
    public float imageAnimTime = 0.5f;
    [SerializeField] private RectTransform imageMask;

    private float currentTime = 0f;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime < imageAnimTime)
        {
            Vector2 offsetMax = imageMask.offsetMax;
            offsetMax.x = -Mathf.Lerp(2000, 110, currentTime/imageAnimTime);
            imageMask.offsetMax = offsetMax;
        }
    }
}
