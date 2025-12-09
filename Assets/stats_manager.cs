using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stats_manager : MonoBehaviour
{
    public static stats_manager inst;
    [SerializeField, Range(1, 10)] public int core_rad;
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
