using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum gameState
{
    invalid,
    titleScreen,
    menu,
    minigame,
    coreLoop,
}

public class game_manager : MonoBehaviour
{


    

    public static game_manager inst;

    
    [SerializeField] public string mainLoopScene;
    
    [Header("do not edit ----DEV INFO-----")]
    public gameState state = gameState.invalid;
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
                if(SceneManager.GetActiveScene().name != mainLoopScene)
                {
                    SceneManager.LoadScene(mainLoopScene);
                }
                if(GameObject.FindWithTag(stats_manager.tags.coreHolder).transform.childCount == 0)
                {
                    cell_manager.inst.generate_core(stats_manager.inst.core_rad);
                } 
            break;
        }
        state = target;
    }

    void startDay(int day)
    {
        stats_manager.inst.startDay(day);
        changeState(gameState.coreLoop);
    }
}
