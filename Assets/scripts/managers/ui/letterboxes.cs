using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UIElements;

public class letterboxes : MonoBehaviour
{
    public GameObject left;
    public GameObject right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        var w = Screen.width;
        var h = Screen.height;
        var cur_ratio = h/w;
    
        var letterbox_w = (w-h) / 2;
        left.SetActive(true);
        left.GetComponent<RectTransform>().position = new Vector3((w-h)/4,h/2,20);
        left.GetComponent<RectTransform>().sizeDelta = new Vector2(letterbox_w,h);
        right.SetActive(true);
        right.GetComponent<RectTransform>().position = new Vector3((3*w+h)/4,h/2,20);
        right.GetComponent<RectTransform>().sizeDelta = new Vector2(letterbox_w,h);
        
    }


}
