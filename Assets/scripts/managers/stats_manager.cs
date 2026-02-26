using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class stats_manager : MonoBehaviour
{
    public struct tags
    {
        public static string coreHolder = "core";
        public static string cam = "MainCamera";

    }
    public static stats_manager inst;
    [Tooltip("ticks per second (setting to 0 just means there is a tick per frame)")]
    [SerializeField, Min(0f)] public float tps;
    [Serialize] public int day = 0;
    [Tooltip("power drain/tick")]
    [SerializeField] public float power_drain_rate = .0107f;
    [SerializeField] public float seconds_in_day = 100;
    
    [SerializeField] public List<float> power_needed_day = new List<float>();
    [SerializeField] public List<int> core_size_day = new List<int>();
    [SerializeField] public List<float> max_power_day = new List<float>();
    [Header("dev game stats ---NOT CONFIG---")]
    public float power = 0f; // in watts
    public float max_power = 10f;
    public float power_needed = 0f;
    public float power_sent = 0f;
    public int core_rad = 0;
    public float time_left = 0f;
    public float reactor_temp = 0f; //percentage

    private float second_kounter = 0f;
    

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        second_kounter += Time.deltaTime;
        if(second_kounter > 1f)
        {
            second_kounter = 0f;
            time_left -= 1;
        }
    }
    
    public void startDay(int targetDay)
    {
        day = targetDay;
        time_left = seconds_in_day;
        max_power = max_power_day[day];
        core_rad = core_size_day[day];
        power_needed = power_needed_day[day];

    }
}
