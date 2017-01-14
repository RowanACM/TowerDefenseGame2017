using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Store : MonoBehaviour {

	public int money;
	public GameObject gameCamera;
	public Selectable selectedObject;
	public Text upgradeCost;
	public Text sellAmount;
	public Text moneyAmount;
	public Button upgradeButton;
	public Text selectedItemText;
	public Image selectedItemImage;
	public GameObject selectedItemPanel;


	void Start(){
		selectedObject = null;
	}

	public void setSelectedItem(Selectable item)
	{
		if (selectedObject) {
            selectedObject.SendMessage("OnDeselect");
			selectedObject = null;
		}
		if (item) {
            selectedObject = item;
            selectedObject.SendMessage("OnSelect");
        }
	}

	void Update() {
		if (selectedObject) {
			selectedItemPanel.SetActive (true);
			Upgradable upgradeComponent = selectedObject.GetComponent<Upgradable> ();
			Item itemComponent = selectedObject.GetComponent<Item> ();
			if (upgradeComponent) {
				if (upgradeComponent.isUpgradable ()) {
					upgradeCost.text = upgradeComponent.getUpgradeCost ().ToString ();
					upgradeButton.gameObject.SetActive (true);
				} else {
					upgradeCost.text = "N/A";
					upgradeButton.gameObject.SetActive (false);
				}
				sellAmount.text = upgradeComponent.getSellValue ().ToString ();
				if (itemComponent) {
					selectedItemText.text = itemComponent.itemName + " (Level " + upgradeComponent.level + ")";
					selectedItemImage.sprite = itemComponent.getImage ();
				}
			}

		} else {
			selectedItemPanel.SetActive (false);
		}

		moneyAmount.text = money.ToString ();
	}
	
	public bool CanPurchase(Purchaseable purchaseItem) {
		return money >= purchaseItem.GetPrice();
	}

	public bool Purchase(Purchaseable purchaseItem)
	{
		if (money >= purchaseItem.GetPrice()) {
			money -= purchaseItem.GetPrice ();
			return true;
		}
		return false;
	}

	public void upgrade()
	{
		Upgradable upgrader = selectedObject.GetComponent<Upgradable>();
		if (upgrader.isUpgradable() && money >= upgrader.getUpgradeCost ()) {
			money -= upgrader.getUpgradeCost ();
			upgrader.Upgrade ();
		}
		
	}
	
	//sell a given item
	public void sell(GameObject item)
	{
		Upgradable upgrader = item.GetComponent<Upgradable>();
        Purchaseable purchase = item.GetComponent<Purchaseable>();
		if (purchase) {
            if (purchase.WasPurchased())   {
                if (upgrader)
                {
                    money += upgrader.getSellValue();
                }else
                {
                    money += purchase.GetPrice();
                }
            }
			Destroy (item);
			item = null;
		}
	}
	
	//sell the current selected item
	public void sell()
	{
        sell(selectedObject.gameObject);
        selectedObject = null;
	}

}
