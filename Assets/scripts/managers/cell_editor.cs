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
    }

    string make_cell_editor_header_text(cell c)
    {
        return $@"
cell editor
position: {c.pos}
fuel: {c.fuel*100}%
heat: {c.heat*100}%
water: {c.water*100}%
neutrons: {c.n_amount}
iodine: {c.i_amount}
xenon: {c.xe_amount}
        ";
    }
}
