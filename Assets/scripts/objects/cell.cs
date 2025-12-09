using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class cell
{
    #region  // lot ton of variables
    public Vector2 pos;                    // position in the reactor

    public float fuel = 1f;                  // percent
    public float rod_insertion = 1f;         // percent, istg no jokes
    public float water = 0f;                 // percent, used to cool
    public int   n_amount = 0;               // number of neutrons
    public int   xe_amount = 0;              // number of xenon atoms
    public int   i_amount = 0;               // number of iodine atoms

    //per second
    public readonly float i_to_xe_decay_rate = 0f;    // percent, how fast iodine decays to xenon
    public readonly float xe_to_n_decay_rate = 0f;    // percent, how fast xenon decays to neutrons
    public readonly float xe_burnoff_rate = 0f;       // percent, how much xenon naturally burns away
    public readonly float base_n_gen = .5f;           // how much n gets made with fuel ignoring the i->xe->n thing
    public readonly float base_fuel_burn_rate = .05f; // how much fuel gets burned per second

    public readonly float n_to_power = 1f; // idk about this one champ
    
    public float kounter = 0f;
    public float last_kounter = 0f;
    public float last_n_gen = 0f; // how fast n was being made last frame

    // couldn't think of a better way
    private float fuel_to_n_kounter = 0f;

    #endregion

    // in n/s calculates the current production rate via a formula
    public float current_n_gen()
    {
        return base_n_gen * rod_insertion;
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
                fuel -= .1f;
            }

        }

    }

    public float calc_power_gen()
    {
        return (current_n_gen() - last_n_gen) * n_to_power; // takes the change in n
    }
    /// <summary>
    /// the frame to frame operations of the cell
    /// </summary>
    public void tick()
    {
        last_kounter = kounter;
        kounter += Time.deltaTime;
        fuel_to_n_kounter += Time.deltaTime;
        last_n_gen = current_n_gen();
        convert_fuel_to_n(); 

        fuel = Mathf.Max(0f,fuel - (base_fuel_burn_rate * current_n_gen()/base_n_gen * Time.deltaTime)); // take fuel away
        
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
    public List<cell> cells;

    public float avg_power()
    {
        float pow = 0f;
        foreach(cell i in cells)
        {
            pow += i.calc_power_gen();
        }
        pow /= cells.Count;

        return pow;
    }

    public LPRM() {}
    public LPRM(List<cell> c) { cells = c;}
}