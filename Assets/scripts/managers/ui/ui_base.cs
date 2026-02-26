using System;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ui_base : MonoBehaviour
{
    public static ui_base inst;
    public GameObject office_holder;
    public GameObject titlescreen_holder;

    [SerializeField] private GameObject reactor_power_dial;
    [SerializeField] private GameObject power_sent_dial;
    [SerializeField] private GameObject clock_obj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        update_doohickeys();
    }

    private void update_doohickeys()
    {

        reactor_power_dial.GetComponent<rect_dial>().value = stats_manager.inst.power/stats_manager.inst.max_power;
        power_sent_dial.GetComponent<rect_dial>().value = stats_manager.inst.power_sent/stats_manager.inst.power_needed;

        //clock
        // the game takes place during a 9am-5pm or 28,800 seconds and we gotta convert that
        float seconds = stats_manager.inst.seconds_in_day - stats_manager.inst.time_left;
        seconds /= stats_manager.inst.seconds_in_day;
        seconds *= 28800;
        int min = Mathf.FloorToInt(seconds/60);
        int hour = Mathf.FloorToInt(min/60);
        hour += 9;
        bool post_midday = hour > 11; // gotta remeber if its past before we loop back the values
        // loop around the hour and min
        if(hour > 12) {hour -= 12;}
        min %= 60;
        string display_hour = $"{(hour < 10? "0" : "")}{hour}";
        string display_min  = $"{(min < 10 ? "0" : "")}{min}";
        clock_obj.GetComponentInChildren<TextMeshProUGUI>().text = $"{display_hour}:{display_min} {(post_midday ? "PM" : "AM")}";
    }

    public void changeState(gameState s)
    {
        office_holder.SetActive(false);
        titlescreen_holder.SetActive(false);
        switch(s)
        {
            case gameState.coreLoop:
                office_holder.SetActive(true);
            break;
            case gameState.titleScreen:
                titlescreen_holder.SetActive(true);
            break;
                
        }
    }

}
