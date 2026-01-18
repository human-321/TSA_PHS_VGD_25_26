using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class cell_actor_base : MonoBehaviour
{
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite selectedSprite;

    [NonSerialized] public cell myCell;
    private bool selected = false;


    private GameObject buttonObj;
    private Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        buttonObj = transform.GetChild(0).gameObject;
        button = buttonObj.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cell_editor.inst.current_cell_actor != gameObject)
        {
            selected = false;
        }

        button.image.sprite = selected ? selectedSprite : defaultSprite;
    }

    public void setCell(cell c) {myCell = c;}

    public void clicked()
    {
        cell_editor.inst.current_cell_actor = gameObject;
        selected = true;

    }
}
