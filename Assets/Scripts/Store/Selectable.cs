using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {

    private bool selected;

    Store store;

    public void Start()
    {
        store = FindObjectOfType<Store>();
    }

    /// <summary>
    /// On select marks this as the selected item in the store, unless this is already selected, in which case it deselects it
    /// </summary>
    public void OnClick()
    {
        if (!selected)
        {
            store.setSelectedItem(this);
        } else
        {
            store.setSelectedItem(null);
        }
    }

    public void OnSelect()
    {
        selected = true;
    }

    public bool IsSelected()
    {
        return selected;
    }
    /// <summary>
    /// On deselect listens for when the store marks this as unselected
    /// </summary>
    public void OnDeselect()
    {
        selected = false;
    }
}
