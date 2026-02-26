using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cell_editor : MonoBehaviour
{
    public static cell_editor inst;
    
    [SerializeField] public GameObject cell_editor_header;
    [Header("non fields dev info")]
    [SerializeField] public GameObject current_cell_actor;
    private TextMeshProUGUI editor_header;

    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        editor_header = cell_editor_header.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(current_cell_actor != null)
        {
            editor_header.text = make_cell_editor_header_text(current_cell_actor.GetComponent<cell_actor_base>().myCell);
        }
        else
        {
            editor_header.text = cell_editor_header_text_null();
        }
    }

    string make_cell_editor_header_text(cell c)
    {
        return $@"
cell editor
position: {myMath.glamVec2Display(myMath.toVec2Int(c.pos))}
rod insertion: {myMath.glamFloatDisplay(c.rod_insertion)}%
fuel: {myMath.glamFloatDisplay(c.fuel)}%
heat: {myMath.glamFloatDisplay(c.heat)}%
water: {myMath.glamFloatDisplay(c.water)}%
neutrons: {c.n_amount}
iodine: {c.i_amount}
xenon: {c.xe_amount}
        ";
    }
    string cell_editor_header_text_null()
    {
        return $@"
cell editor
position: NA
rod insertion: NA
fuel: NA
heat: NA
water: NA
neutrons: NA
iodine: NA
xenon: NA
        ";
    }
}
