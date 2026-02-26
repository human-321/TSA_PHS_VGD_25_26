using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class rect_dial : MonoBehaviour
{
    [SerializeField, Range(0f,1f)] public float value = .75f;
    [SerializeField, Range(0f,1f)] public float red = 0f;
    [SerializeField] private GameObject val_tick;
    [SerializeField] private GameObject tick_25;
    [SerializeField] private GameObject tick_50;
    [SerializeField] private GameObject tick_75;
    [SerializeField] private GameObject red_tick;


    //of the face not the outline
    private float width = 0f;
    private float height = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        width  = transform.Find("face").GetComponent<RectTransform>().rect.width;
        height = transform.Find("face").GetComponent<RectTransform>().rect.height;
        tick_25.transform.position =  determine_pos(tick_25.GetComponent<RectTransform>(),.25f);
        tick_50.transform.position =  determine_pos(tick_50.GetComponent<RectTransform>(),.50f);
        tick_75.transform.position =  determine_pos(tick_75.GetComponent<RectTransform>(),.75f);
        red_tick.transform.position =  determine_pos(red_tick.GetComponent<RectTransform>(),red);
    }

    // Update is called once per frame
    void Update()
    {
        val_tick.transform.position = determine_pos(val_tick.GetComponent<RectTransform>(),value);
        red_tick.transform.position =  determine_pos(red_tick.GetComponent<RectTransform>(),red);
    }

    Vector3 determine_pos(RectTransform t,float val)
    {
        return new Vector3(transform.position.x + (2*Math.Clamp(val,0f,1f) - 1)*width/2f,transform.position.y - (height/2 - t.rect.height/2),0f);
    }
}
