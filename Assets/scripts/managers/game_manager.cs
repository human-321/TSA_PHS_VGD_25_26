using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum gameState
{
    invalid,
    titleScreen,
    menu,
    minigame,
    coreLoop,
    dayEndScreen,
    outro,
}

public enum failState
{
    invalid,
    none,
    timeRanOut,
    meltdown
}

public class game_manager : MonoBehaviour
{

    public static game_manager inst;
    
    
    [Header("do not edit ----DEV INFO-----")]
    public gameState state = gameState.invalid;
    public bool      lastWinDay = false;
    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        DontDestroyOnLoad(this);
        changeState(gameState.titleScreen);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == gameState.coreLoop) {dayCheck();}
    }

    public void startOfficeLoop()
    {
        startDay(0);
    }
    /// <summary>
    /// go from one state to another
    /// a state is like what you can do like the core gameplay loop area vs the main menu
    /// </summary>
    /// <param name="target">the type of state to go</param>
    void changeState(gameState target)
    {
        ui_base.inst.changeState(target);
        switch(target)
        {
            case gameState.coreLoop:
                if(SceneManager.GetActiveScene().name != stats_manager.sceneNames.mainLoopScene)
                {
                    SceneManager.LoadScene(stats_manager.sceneNames.mainLoopScene);
                }
                cell_manager.inst.delete_core();
                cell_manager.inst.generate_core(stats_manager.inst.core_rad);
            break;
            case gameState.dayEndScreen:
                if(SceneManager.GetActiveScene().name != stats_manager.sceneNames.dayEndScreen)
                {
                    SceneManager.LoadScene(stats_manager.sceneNames.dayEndScreen);
                }

            break;
        }
        state = target;
    }

    public void startDay(int day)
    {
        if(day >= stats_manager.inst.core_size_day.Count)
        {
            changeState(gameState.outro);
        }
        else
        {    
            stats_manager.inst.startDay(day);
            changeState(gameState.coreLoop);
        }
    }

    #region // day fail/win system

    failState failureCheck()
    {
        failState o = failState.none;
        if(stats_manager.inst.time_left <= 0 && stats_manager.inst.power_sent < stats_manager.inst.power_needed) 
        {
            o = failState.timeRanOut;
        }
        if(stats_manager.inst.seconds_in_day - stats_manager.inst.time_left != 0)
        {
            if(stats_manager.inst.water_level <= 0 || stats_manager.inst.reactor_heat >= 1) {o = failState.meltdown;}
        }
        return o;
    }

    bool winCheck()
    {
        return stats_manager.inst.time_left <= 0 && stats_manager.inst.power_sent >= stats_manager.inst.power_needed;    
    }

    void dayCheck()
    {
        if(failureCheck() == failState.none)
        {
            if(winCheck()) {winday();}
        }
        else {failday(failureCheck());}

    }

    void failday(failState s)
    {
        lastWinDay = false;
        dayEnd();
    }

    void winday()
    {
        lastWinDay = true;
        dayEnd();
    }

    void dayEnd()
    {
        if(stats_manager.inst.day + 1 >= stats_manager.inst.core_size_day.Count)
        {
            changeState(gameState.outro);
        }
        else {changeState(gameState.dayEndScreen);}
    }

    public void restartGame()
    {
        stats_manager.inst.day = 0;
        changeState(gameState.titleScreen);
    }

    #endregion
}
