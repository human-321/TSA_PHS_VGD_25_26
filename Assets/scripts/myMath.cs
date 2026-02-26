using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public struct myMath
{

    public static Vector3    toVec3(Vector2 v)            {return new Vector3(v.x,v.y,0f);}
    public static Vector3    toVec2(Vector3 v)            {return new Vector2(v.x,v.y);}
    public static Vector2Int toVec2Int(Vector3 v)         {return new Vector2Int((int)v.x,(int)v.y);}
    public static float      glamFloatDisplay(float x)    {return Mathf.Floor(x * 100);}
    public static string     glamVec2Display(Vector2 vec) {return $"({vec.x},{vec.y})";}

}
