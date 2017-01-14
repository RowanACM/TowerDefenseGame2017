using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragAndDropPurchase : MonoBehaviour
{


    public StoreItemUI item;


    // Use this for initialization
    void Start()
    {
        GetComponent<Image>().sprite = item.GetComponent<Item>().getImage();
    }

    //on mouse down, we'll instantiate our object and set it inactive until we drag the mouse off
    public void OnMouseDown()
    {
        StoreItemUI spawnedItem = Instantiate(item);
        spawnedItem.transform.position = GameObject.Find("LoadingSlot").transform.position;
        spawnedItem.startPurchaseProcess();

    }
}
