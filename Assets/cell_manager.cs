using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class cell_manager : MonoBehaviour
{
    
    private float kounter = 0f;
    public Dictionary<Vector2, cell> cells = new Dictionary<Vector2, cell>();
    public List<LPRM> l = new List<LPRM>();

    // Start is called before the first frame update
    void Awake()
    {
        generate_core(stats_manager.inst.core_rad);
    }

    // Update is called once per frame
    void Update()
    {
        print("   ");
        kounter += Time.deltaTime;
        if(kounter >= 1)
        {
            foreach (cell i in cells.Values)
            {
                i.tick();
                spread_cell_n(i.pos);
                print(i.pos);
            }
        }
        
    }

    #region // utility

    /// <summary>
    /// gets a cell if it exists inside the core
    /// </summary>
    public cell get_cell(Vector2 pos)
    {
        if(cells.ContainsKey(pos)) {return cells[pos];}
        return null;

    }
    cell get_cell((int,int) pos) {return get_cell(new Vector2(pos.Item1,pos.Item2));} 
    cell get_cell(int x,int y) {return get_cell((x,y));} 

    /// <summary>
    /// gets the neighbors of a cell
    /// ......... in a list of the N,W,S,E format
    /// </summary>
    /// <param name="rad"></param>
    public List<cell> get_cell_neighborhood(Vector2 pos)
    {
        return new List<cell>(){get_cell(pos + Vector2.up),get_cell(pos + Vector2.right),get_cell(pos - Vector2.up),get_cell(pos - Vector2.right)};
    }
    public List<cell> get_cell_neighborhood(int x,int y) {return get_cell_neighborhood(new Vector2(x,y));}    
    public List<cell> get_cell_neighborhood((int,int) pos) {return get_cell_neighborhood(new Vector2(pos.Item1,pos.Item2));}    

    private void spread_cell_n(Vector2 pos)
    {
        List<float> n = new List<float>(){get_cell(pos).n_amount};
        foreach(cell c in get_cell_neighborhood(pos))
        {
            if(c != null) {n.Add(c.n_amount);}
        }
        float mean = Mathf.Floor(n.Average());
        float diff = get_cell(pos).n_amount - mean;
        int i = 0;
        while(diff > 0f)
        {
            if(get_cell_neighborhood(pos)[i] != null)
            {
                get_cell_neighborhood(pos)[i].n_amount += 1;
                get_cell(pos).n_amount -= 1;
                diff -= 1;
            }
            i = (i + 1) % 4;
        }
        
        
    }
    
    
    #endregion

    void generate_core(int rad)
    {
        cells.Clear();
        l.Clear();
        /*       _____    rad = 2 essientially square with no corners
              __|__|__|__
             |__|__|__|__|
             |__|__|__|__|
                |__|__|
        */

        if(cells.Count > 0 )
        {
            cells.Clear();
        }
        for (var i = 0; i <= rad+1; i++)
        {
            for (var j = 0; j <= rad+1; j++)
            {
                var pos_new = new Vector2(i, j);
                cells.Add(pos_new, new cell(pos_new));
            }
        }

        cells.Remove(new Vector2(0, 0));
        cells.Remove(new Vector2(rad + 1, 0));
        cells.Remove(new Vector2(0, rad + 1));
        cells.Remove(new Vector2(rad + 1, rad + 1));

        // time to make LPRMS
        if(rad <= 1)
        {
            l.Add(new LPRM(cells.Values.ToList()));
        }
        else 
        {
            if(rad % 2 == 0) // even
            {
                // we use the same quardent numbering as the cartesian plane
                LPRM q1 = new LPRM();
                LPRM q2 = new LPRM();
                LPRM q3 = new LPRM();
                LPRM q4 = new LPRM();
                l.Add(q1); l.Add(q2); l.Add(q3); l.Add(q4);
                

            }
        }
    }



}
