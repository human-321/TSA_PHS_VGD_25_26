using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

public class cell
{
    #region  // lot ton of variables
    public Vector2 pos;                    // position in the reactor

    // main things , these are what differentiates the cells from each other
    public float fuel          = 1f; // percent
    public float heat          = .2f; // percent
    public float rod_insertion = 1f; // percent, istg no jokes
    public float water         = .5f; // percent, used to cool
    public int   n_amount      = 0;  // number of neutrons
    public int   xe_amount     = 10; // number of xenon atoms
    public int   i_amount      = 10; // number of iodine atoms

    //per second
    public readonly float i_to_xe_decay_rate  = 0.5f;   // percent, how fast iodine decays to xenon
    public readonly float xe_to_n_decay_rate  = 0.67f;  // percent, how fast xenon decays to neutrons
    public readonly float xe_burnoff_rate     = 0.213f; // percent, how much xenon naturally burns away
    public readonly float base_n_gen          = 0.9f;   // how much n gets made with fuel ignoring the i->xe->n thing
    public readonly float base_fuel_burn_rate = 0.05f;  // how much fuel gets burned per second
    public readonly float fuel_to_heat_rate = 0.01f;
    public readonly float water_damp_fire_rate = .8f;

    public readonly float n_to_power = 1f; // idk about this one champ
    
    // kounters
    public float kounter       = 0f;
    public float last_kounter  = 0f;
    private float fuel_to_n_kounter  = 0f;
    private float i_to_xe_kounter    = 0f;
    private float xe_to_n_kounter    = 0f;
    private float xe_burnoff_kounter = 0f;

    public float last_n_gen    = 0f; // how fast n was being made last frame
    public int   last_n_amount = 0;

    #endregion

    /// <summary>
    /// this is what you add to heat,........  like heat += this func
    /// </summary>
    /// <param name="heat"></param>
    /// <returns></returns>
    public float fuel_to_heat_affect(float heat)
    {
        return fuel_to_heat_rate * 1f/(2*heat + 1f);
    }

    public float water_affect(float percent)
    {
        return Mathf.Sin(Mathf.Clamp(percent/1.01f,0f,1f) * Mathf.PI);
    }

    // in n/s calculates the current production rate via a formula
    public float current_n_gen()
    {
        return base_n_gen * water_affect(water) * rod_insertion * 1f/(stats_manager.inst.tps > 0? stats_manager.inst.tps : 1);
    }

    public float calc_power_gen()
    {
        return Mathf.Abs(n_amount - last_n_amount) * n_to_power; // takes the change in n
    }

    private void convert_fuel_to_n()
    {

        // the thing below is modulo %
        if (fuel_to_n_kounter >= 1f/current_n_gen())
        {
            fuel_to_n_kounter = 0f;
            if (fuel > 0f)
            {
                n_amount += 1;
                fuel = Mathf.Max(0f,fuel - (base_fuel_burn_rate * current_n_gen()/base_n_gen * Time.deltaTime)); // take fuel away
                heat += fuel_to_heat_affect(heat);
            }

        }

    }

    private void perform_i_xe_loop()
    {
        if(i_to_xe_kounter >= 1f/i_to_xe_decay_rate)
        {
            i_to_xe_kounter = 0f;
            if(i_amount > 0)
            {
                i_amount -= 1;
                xe_amount += 1;
            }
        }
        if(xe_to_n_kounter >= 1f/xe_to_n_decay_rate)
        {
            xe_to_n_kounter = 0f;
            if(xe_amount > 0)
            {
                xe_amount -= 1;
                n_amount += 1;
            }
        }
        if(xe_burnoff_kounter >= 1f/xe_burnoff_rate)
        {
            xe_burnoff_kounter = 0f;
            if(xe_amount > 0)
            {
                xe_amount -= 1;
            }
        }
    }

    /// <summary>
    /// the frame to frame operations of the cell
    /// </summary>
    public void tick()
    {
        last_n_amount = n_amount;
        last_kounter = kounter;
        kounter += Time.deltaTime;
        fuel_to_n_kounter += Time.deltaTime;
        i_to_xe_kounter += Time.deltaTime;
        xe_to_n_kounter += Time.deltaTime;
        xe_burnoff_kounter += Time.deltaTime;
        last_n_gen = current_n_gen();
        convert_fuel_to_n(); 
        perform_i_xe_loop();
        
        
    }


    public cell() { }
    public cell(Vector2 position)
    {
        pos = position;
    }
    public cell((int,int) position) // just for literal cuz for some reason vector literals dont exist
    {
        pos = new Vector2(position.Item1,position.Item2);
    }

}

public class LPRM
{
    public List<cell> cells = new List<cell>();
    // private float last_pow;

    public void tick()
    {
        // last_pow = avg_power();
    }
    public float avg_power()
    {
        float pow = 0f;
        foreach(cell i in cells)
        {
            if(i != null) {pow += i.calc_power_gen();}
        }
        pow /= cells.Count;

        return pow;
    }
    // public float change_in_power()
    // {
    //     return avg_power() - last_pow;
    // }
    
    public void add_cell(cell c)
    {
        cells.Add(c);
    }
    
    public LPRM() {}
    public LPRM(List<cell> c) { cells = c;}
}