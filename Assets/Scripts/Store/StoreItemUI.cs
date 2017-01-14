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
    Placeable placement;
    Selectable selectable;
    ArrowPointer pointer;
    Color highlightColor;

    // Use this for initialization
    void Start () {
        gameStore = FindObjectOfType<Store>();
        outline = GetComponent<OutlineComponent>();
        outline.Enable();
		outline.setParticles(false);
        dragging = true;
        pointer = GetComponentInChildren<ArrowPointer>();
        placement = GetComponent<Placeable>();
        selectable = GetComponent<Selectable>();
    }

    public void startPurchaseProcess()
	{
        if(outline)
		    outline.Enable();	
	}

    public void SetHighlightColor(Color color)
    {
        outline.setColor(color);
        pointer.setColor(color);
        highlightColor = color;
    }

    public void Update()
    {
        if (dragging)
        {
            if (placement)
            {
                //select the proper current highlight color
                Color newHighlightColor;
                if (placement.CanPlace())
                {
                    if (selectable && selectable.IsSelected())
                    {
                        newHighlightColor = Color.yellow;
                    }
                    else
                    {
                        newHighlightColor = Color.green;
                    }
                } else
                {
                    newHighlightColor = Color.red;
                }
                if(newHighlightColor != highlightColor)
                {
                    SetHighlightColor(newHighlightColor);
                }

                if (pointer)
                {
                    if (placement.spawnCell)
                    {
                        pointer.target = placement.spawnCell.transform;
                    }
                    else
                    {
                        pointer.target = null;
                    }
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
        SetHighlightColor(Color.yellow);
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

    public void OnDrag()
    {
        outline.Enable();
        dragging = true;
        SendMessage("OnIdle");
    }
	
	public void OnPlace()
	{
        if (!(selectable && selectable.IsSelected()))
        {
            outline.Disable();
        }
        dragging = false;
        SendMessage("OnDeIdle");
	}
	
}
