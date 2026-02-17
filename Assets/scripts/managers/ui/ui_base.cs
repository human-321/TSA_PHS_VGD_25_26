using System;
using Unity.VisualScripting;
using UnityEngine;

public class ui_base : MonoBehaviour
{
    public static ui_base inst;
    public GameObject office_holder;
    public GameObject titlescreen_holder;

    [SerializeField] private GameObject reactor_power_dial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void update_dials()
    {
        reactor_power_dial.GetComponent<rect_dial>().value = stats_manager.inst.power/stats_manager.inst.max_power;
    }

    public void changeState(game_manager.gameState s)
    {
        office_holder.SetActive(false);
        titlescreen_holder.SetActive(false);
        switch(s)
        {
            case game_manager.gameState.coreLoop:
                office_holder.SetActive(true);
            break;
            case game_manager.gameState.titleScreen:
                titlescreen_holder.SetActive(true);
            break;
                
        }
    }

}
