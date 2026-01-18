using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    [SerializeField, Range(1, 10)] public int core_rad;
    [Header("dev game stats ---NOT CONFIG---")]
    public float power = 0f; // in watts


    

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
