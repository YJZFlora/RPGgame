using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
  public GameObject theMenu;
  private CharStats[] playerStats;
  public GameObject[] windows;

  // to control names show in the menu UI
  public Text[] nameText, hpText, mpText, lvlText, expText;
  public Slider[] expSlider;
  public Image[] charImage;
  public GameObject[] charStatHolder;
  public GameObject[] statusButtons;
  public Text statusName, statusHP, statusMP, statusStr, statusDef, statusWpnE, statusWpnPwr, statusArmr, statusArmrP, statusExp;
  public Image statusImage;

  public ItemButton[] itemButtons;
  public string selectedItem;
  public Item activeItem;
  public Text itemName, itemDescription, useButtonText;

  public static GameMenu instance;

  public GameObject itemCharChoiceMenu;
  public Text[] itemCharChoiceNames;

  public Text goldText;
  
  public string mainMenuName;


    // Start is called before the first frame update
    void Start()
    {
      instance = this;
      theMenu.SetActive(false);
      GameManager.instance.gameMenuOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetButtonDown("Fire2")) // "X" button on controller; right click on mouse
      {
        if(theMenu.activeInHierarchy) {
          CloseMenu();
        } else {
          theMenu.SetActive(true);
          updataMainSats();
          GameManager.instance.gameMenuOpen = true;
        }

        // 5 is the menu music we set up in Unity
        AudioManager.instance.PlaySFX(5);
      }

    }

    public void updataMainSats()
    {
      Debug.Log("_________updateing playerStats________");
      playerStats = GameManager.instance.playerStats;

      if(playerStats == null) {
        Debug.Log("playerStats is null!!!!!!!!!");
      }
      for(int i = 0; i < playerStats.Length; i++)
      {
        if(playerStats[i].gameObject.activeInHierarchy)
        {
          charStatHolder[i].SetActive(true);
          nameText[i].text = playerStats[i].charName;
          hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
          mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
          lvlText[i].text =  "Level: " + playerStats[i].playerLevel;
          expText[i].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
          expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
          expSlider[i].value = playerStats[i].currentEXP;
          charImage[i].sprite = playerStats[i].charImage;
        } else {
          charStatHolder[i].SetActive(false);
        }

        goldText.text = GameManager.instance.currentGold + "g";
      }
    }

    public void ToggleWindow(int windowIndex)
    {
      updataMainSats();
      for(int i = 0; i < windows.Length; i++) {
        if(i == windowIndex) {
          windows[i].SetActive(!windows[i].activeInHierarchy);
        } else {
          windows[i].SetActive(false);
        }
      }
      itemCharChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
      for(int i = 0; i < windows.Length; i++)
      {
        windows[i].SetActive(false);
      }
      theMenu.SetActive(false);
      GameManager.instance.gameMenuOpen = false;

      itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
      updataMainSats();
      StatusChar(0);
      // update information that is shown
      for(int i = 0; i < statusButtons.Length; i++) {
        statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
        statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
      }
    }

    public void StatusChar(int selected)
    {

      CharStats curPStat = playerStats[selected];
      statusName.text = playerStats[selected].charName;
      statusHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
      statusMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
      statusStr.text = playerStats[selected].strength.ToString();
      statusDef.text = playerStats[selected].defence.ToString();
      if(playerStats[selected].equippedWpn != "")
      {
        statusWpnE.text = playerStats[selected].equippedWpn;
      }
      statusWpnPwr.text = playerStats[selected].wpnPwr.ToString();
      if(playerStats[selected].equippedArmr != "")
      {
        statusArmr.text = playerStats[selected].equippedArmr;
      }
      statusArmrP.text = playerStats[selected].armPwr.ToString();
      statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();
      statusImage.sprite = curPStat.charImage;
    }

    public void ShowItems()
    {
      GameManager.instance.SortItems(GameManager.instance.itemsHeld);
      for(int i = 0; i < itemButtons.Length; i++)
      {
        ItemButton curButton = itemButtons[i];
        curButton.buttonValue = i;

        if(GameManager.instance.itemsHeld[i] != "")
        {
          curButton.buttonImage.gameObject.SetActive(true);
          curButton.buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
          curButton.amountText.text = GameManager.instance.numItems[i].ToString();
        } else
        {
          curButton.buttonImage.gameObject.SetActive(false);
          curButton.amountText.text = "";
        }
      }
    }

    /*
      Select an item and set it active
    */
    public void SelectItem(Item newItem)
    {
        Debug.Log("Selecting item...");
      Debug.Log(GameMenu.instance.gameObject.name);
      activeItem = newItem;
      if(activeItem.isItem){
        useButtonText.text = "Use";
      } else if(activeItem.isWeapon || activeItem.isArmor) {
        useButtonText.text = "Equip";
      }

      itemName.text = activeItem.itemName;

      itemDescription.text = activeItem.description;
    }

    /*
      discard item that player wants to discard
    */
    public void DiscardItem()
    {
        if(activeItem != null)
        {
          Debug.Log("Removing....");
          Debug.Log(activeItem.itemName);
          GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    /*
      for "Use" or "Equip" button to trigger "For who" menu
    */
    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for(int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }

    /*
    */
    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    /*
      "For who" menu to choose who use the item
    */
    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }

    public void PlayButtonSound()
    {
      // 4 is the idx of button sound we set up in Unity
        AudioManager.instance.PlaySFX(4);
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void QuitGame()
        {
            SceneManager.LoadScene(mainMenuName);

            Destroy(GameManager.instance.gameObject);
            Destroy(PlayerController.instance.gameObject);
            Destroy(AudioManager.instance.gameObject);
            Destroy(gameObject);
        }




}
