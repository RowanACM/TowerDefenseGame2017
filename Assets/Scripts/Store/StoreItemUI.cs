using UnityEngine;
using System.Collections;

[RequireComponent(typeof(OutlineComponent))]
[RequireComponent(typeof(Item))]
public class StoreItemUI : MonoBehaviour {

	private TextMesh costText;
	private TextMesh fundsAlertText;
    private bool dragging;
    Store gameStore;
    OutlineComponent outline;

    // Use this for initialization
    void Start () {
        gameStore = FindObjectOfType<Store>();
        outline = GetComponent<OutlineComponent>();
        outline.Enable();
		outline.setParticles(false);
        dragging = true;
	}

    public void startPurchaseProcess()
	{
        if(outline)
		    outline.Enable();	
	}

    public void Update()
    {
        if (dragging)
        {
            ArrowPointer pointer = GetComponentInChildren<ArrowPointer>();
            Placeable placement = GetComponent<Placeable>();
            if (placement)
            {
                if (pointer)
                {
                    if (placement.spawnCell)
                    {
                        pointer.target = placement.spawnCell.transform; 
                    }
                    if (placement.CanPlace())
                    {
                        pointer.setColor(Color.green);
                    }
                    else
                    {
                        pointer.setColor(Color.red);
                    }
                }
                if (placement.CanPlace())
                {
                    outline.setColor(Color.green);
                } else
                {
                    outline.setColor(Color.red);
                }
            }
        }
            Purchaseable purchase = this.GetComponent<Purchaseable>();
        if (!purchase.WasPurchased())
        {
            if (costText == null)
            {
                costText = GetComponentInChildren<TextDisplay>().Add(0);
                costText.text = "Cost: " + purchase.GetPrice();
            }
            if (purchase.CanBePurchased())
            {
                if (fundsAlertText)
                {
                    GetComponentInChildren<TextDisplay>().Remove(fundsAlertText);
                    fundsAlertText = null;
                }
                if (costText)
                    costText.color = Color.green;
            }
            else
            {
                if (!fundsAlertText)
                {
                    fundsAlertText = GetComponentInChildren<TextDisplay>().Add(1);
                    fundsAlertText.text = "INSUFFICIENT FUNDS";
                    fundsAlertText.color = Color.red;
                }
                if (costText)
                    costText.color = Color.red;
            }
        }
    }

    /**
	*	Called when this item was selected as the current Store item but becomes deselected.
	*/
    public void OnDeselect() {
			outline.Disable();
			outline.setParticles(false);
	}
	
	public void OnSelect(){
		outline.Enable();
        outline.setColor(Color.yellow);
		outline.setParticles(true);
	}

    public void OnPurchase()
    {
        if (costText)
        {
            GetComponentInChildren<TextDisplay>().Remove(costText);
            costText = null;
        }
    }

    public void OnInvalid()
    {

    }

    public void OnDrag()
    {
        outline.Enable();
        dragging = true;
        SendMessage("OnIdle");
    }
	
	public void OnPlace()
	{
        Selectable selectable = GetComponent<Selectable>();
        if (selectable && selectable.IsSelected())
        {
            outline.setColor(Color.white);
        }
        else
        {
            outline.Disable();
        }
        dragging = false;
        SendMessage("OnDeIdle");
	}
	
}
