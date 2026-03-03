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
    public GameObject newDay_holder;
    public GameObject outro_holder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        doohicky_ui_manager.inst.update_doohickeys();
    }


    public void changeState(gameState s)
    {
        office_holder.SetActive(false);
        titlescreen_holder.SetActive(false);
        newDay_holder.SetActive(false);
        outro_holder.SetActive(false);
        switch(s)
        {
            case gameState.coreLoop:     office_holder.SetActive(true); break;
            case gameState.titleScreen:  titlescreen_holder.SetActive(true); break;
            case gameState.dayEndScreen: newDay_holder.SetActive(true); break;
            case gameState.outro:        outro_holder.SetActive(true); break;
                
        }
    }

    public void dayTransitionScreenButton()
    {
        if(game_manager.inst.lastWinDay)
        {
            game_manager.inst.startDay(stats_manager.inst.day + 1);
        }
        else
        {
            game_manager.inst.restartGame();
        }
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
