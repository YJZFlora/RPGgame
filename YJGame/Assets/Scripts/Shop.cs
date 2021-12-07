using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
  public static Shop instance;
  public GameObject shopMenu;
  public GameObject buyMenu;
  public GameObject sellMenu;

  public Text goldText;
  public string[] itemsForSale;

  public ItemButton[] buyItemButtons;
  public ItemButton[] sellItemButtons;

  public Item selectedItem;
  public Text buyItemName, buyItemDescription, buyItemValue;
  public Text sellItemName, sellItemDescription, sellItemValue;

    // Start is called before the first frame update
    void Start()
    {
      instance = this;
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy)
      {
        OpenShop();

      }
    }

    public void OpenShop()
    {
      shopMenu.SetActive(true);
      OpenBuyMenu();
      GameManager.instance.shopActive = true;

      goldText.text = GameManager.instance.currentGold.ToString() + " g";
    }

    public void CloseShop()
    {
      shopMenu.SetActive(false);
      GameManager.instance.shopActive = false;
    }

    /*
      Attached to: buy MENE button
      Show shop keeper's items
    */
    public void OpenBuyMenu()
    {
      buyMenu.SetActive(true);
      sellMenu.SetActive(false);

       GameManager.instance.SortItems(itemsForSale);
      for (int i = 0; i < buyItemButtons.Length; i++)
        {
            ItemButton curButton = buyItemButtons[i];
            curButton.buttonValue = i;

            if (itemsForSale[i] != "")
            {
                curButton.buttonImage.gameObject.SetActive(true);
                curButton.buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                curButton.amountText.text = ""; // unlimit amount for items
            }
            else
            {
                curButton.buttonImage.gameObject.SetActive(false);
                curButton.amountText.text = "";
            }
        }

    }

    /*
      Attach: sell MENU button
      Sell player's items
    */
    public void OpenSellMenu()
    {
      buyMenu.SetActive(false);
      sellMenu.SetActive(true);
      string[] playerItems = GameManager.instance.itemsHeld;

      GameManager.instance.SortItems(playerItems);

      for(int i = 0; i < sellItemButtons.Length; i++)
      {
        ItemButton curButton = sellItemButtons[i];
        curButton.buttonValue = i;

        if(playerItems[i] != "")
        {
          curButton.buttonImage.gameObject.SetActive(true);
          curButton.buttonImage.sprite = GameManager.instance.GetItemDetails(playerItems[i]).itemSprite;
          curButton.amountText.text = GameManager.instance.numItems[i].ToString();
        } else
        {
          curButton.buttonImage.gameObject.SetActive(false);
          curButton.amountText.text = "";
        }
      }
    }

    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "Value: " + selectedItem.value + " g";
    }

    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;
        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        // half of the original price
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f).ToString() + " g";
    }

    /*
      Attached to Buy Button
    */
    public void BuyItem()
    {
        if (selectedItem != null)
        {
            if (GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;
                // put to player's items
                GameManager.instance.AddItem(selectedItem.itemName);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void SellItem()
    {
        if(selectedItem != null)
        {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);
            // remove from player's items
            GameManager.instance.RemoveItem(selectedItem.itemName);
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";

        OpenSellMenu();
    }

    private void ShowSellItems()
    {
        // GameManager.instance.SortItems();
        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            sellItemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = GameManager.instance.numItems[i].ToString();
            }
            else
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }

}
