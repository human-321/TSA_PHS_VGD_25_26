using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class doohicky_ui_manager : MonoBehaviour
{
    public static doohicky_ui_manager inst;
    [SerializeField] private GameObject reactor_power_dial;
    [SerializeField] private GameObject power_sent_dial;
    [SerializeField] private GameObject water_level_dial;
    [SerializeField] private GameObject clock_obj;
    [SerializeField] private GameObject thermo;
    [SerializeField] private List<Sprite> thermo_chamber_sprites = new List<Sprite>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inst = this;
    }

    public void update_doohickeys()
    {

        reactor_power_dial.GetComponent<rect_dial>().value = stats_manager.inst.power/stats_manager.inst.max_power;
        power_sent_dial.GetComponent<rect_dial>().value = stats_manager.inst.power_sent/stats_manager.inst.power_needed;
        water_level_dial.GetComponent<rect_dial>().value = stats_manager.inst.water_level;
        var c = thermo_chamber_sprites.Count;
        thermo.GetComponentInChildren<Image>().sprite = thermo_chamber_sprites[myMath.clamp(Mathf.FloorToInt(c * stats_manager.inst.reactor_heat),0,c-1)];


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

}
