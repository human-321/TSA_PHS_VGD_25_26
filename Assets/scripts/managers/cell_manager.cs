using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class cell_manager : MonoBehaviour
{
    public static cell_manager inst;
    private float kounter = 0f;
    public Dictionary<Vector2, cell> cells = new Dictionary<Vector2, cell>();
    public List<LPRM> lprms = new List<LPRM>();
    [SerializeField] private GameObject cell_actor_prefab;


    private bool tickLock = false;

    // private float last_pow = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        kounter += Time.deltaTime;

        if(stats_manager.inst.tps > 0)
        {
            if(kounter % (1f/stats_manager.inst.tps) < 0.02)
            {
                if(!tickLock) core_tick();
                tickLock = true;
            }
            else
            {
                tickLock = false;
            }
        }
        else
        {
            core_tick();
        }
        
    }

    void core_tick()
    {
        float a = 0f;
        foreach (LPRM i in lprms)
        {
            i.tick();
            a += i.avg_power();
        }
        foreach (cell i in cells.Values)
        {
            i.tick();
            spread_cell_n(i.pos);
        }
        float b = 0f;
        foreach (LPRM i in lprms)
        {
            b += i.avg_power();
        }
        if(b-a > 0f) {stats_manager.inst.power += b-a;}
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

    private List<int> get_cell_neighborhood_n(Vector2 pos)
    {
        List<int> output = new List<int>();
        foreach(cell c in get_cell_neighborhood(pos))
        {
            if(c != null) output.Add(c.n_amount);
        }

        return output;
    }

    private void spread_cell_n(Vector2 pos)
    {
        
        int i = 0;
        while(get_cell_neighborhood_n(pos).Min() < get_cell(pos).n_amount)
        {
            if(get_cell_neighborhood(pos)[i] != null)
            {
                if(get_cell_neighborhood(pos)[i].n_amount < get_cell(pos).n_amount)
                {
                    get_cell_neighborhood(pos)[i].n_amount += 1;
                    get_cell(pos).n_amount -= 1;
                }
            }
            i = (i + 1) % 4;
        }
        
        
    }
    
    
    #endregion

    public void generate_core(int rad)
    {
        #region // internal
        cells.Clear();
        lprms.Clear();
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
            lprms.Add(new LPRM(cells.Values.ToList()));
        }
        else 
        {
            // we use the same quardent numbering as the cartesian plane
            LPRM q1 = new LPRM();
            LPRM q2 = new LPRM();
            LPRM q3 = new LPRM();
            LPRM q4 = new LPRM();
            lprms.Add(q1); lprms.Add(q2); lprms.Add(q3); lprms.Add(q4);
            if(rad % 2 == 0) // even
            {


                int j =  rad / 2;
                foreach (cell c in cells.Values)
                {
                    /*
                    quadrant: 21
                              34
                    
                    */
                    //check if x is in left 2,3 or right 1,4
                    if(c.pos.x <= j)
                    {
                        //in the left 2 or 3
                        if(c.pos.y <= j) {lprms[2].add_cell(c);}
                        else {lprms[1].add_cell(c);}
                    }
                    else
                    {
                        //in the left 2 or 3
                        if(c.pos.y <= j) {lprms[3].add_cell(c);}
                        else {lprms[0].add_cell(c);}
                    }
                }
            }
            else // odd
            {

                // for odd the lprms will overlap
                float j = Mathf.FloorToInt((rad + 2) / 2f); // the middle
                foreach ( cell c in cells.Values)
                {
                    // not in the overlap
                    if(c.pos.x > j && c.pos.y > j) {lprms[0].add_cell(c);}
                    if(c.pos.x < j && c.pos.y > j) {lprms[1].add_cell(c);}
                    if(c.pos.x < j && c.pos.y < j) {lprms[2].add_cell(c);}
                    if(c.pos.x > j && c.pos.y < j) {lprms[3].add_cell(c);}

                    if(c.pos.x == j && c.pos.y == j) // center peice
                    {
                        lprms[0].add_cell(c);
                        lprms[1].add_cell(c);
                        lprms[2].add_cell(c);
                        lprms[3].add_cell(c);
                    }
                    else
                    {
                        if(c.pos.x == j)
                        {
                            if(c.pos.y < j) 
                            {
                                lprms[2].add_cell(c);
                                lprms[3].add_cell(c);
                            }
                            else 
                            {
                                lprms[1].add_cell(c);
                                lprms[0].add_cell(c);
                            }
                        }
                        else
                        {
                            if(c.pos.x < j) 
                            {
                                lprms[2].add_cell(c);
                                lprms[1].add_cell(c);
                            }
                            else 
                            {
                                lprms[0].add_cell(c);
                                lprms[3].add_cell(c);
                            }
                        }
                    }
                }
            }
        }
    
        #endregion 

        #region // other
        
        var center_pos = (rad + 1)/2f * Vector2.one;
        foreach(cell i in cells.Values)
        {
            var cam = GameObject.FindWithTag(stats_manager.tags.cam).GetComponent<Camera>();
            var temp = Instantiate(cell_actor_prefab,GameObject.FindWithTag(stats_manager.tags.coreHolder).transform);
            temp.transform.position = cam.WorldToScreenPoint(myMath.toVec3(cell_actor_prefab.GetComponent<RectTransform>().rect.width * (i.pos - center_pos)));
            temp.GetComponent<cell_actor_base>().setCell(i);
            
        }

        #endregion
    
    }



}
