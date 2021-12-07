using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive;

  public string[] itemsHeld;
  public int[] numItems;
  public Item[] refItems;

  public CharStats[] playerStats;
  public int currentGold;

    // Start is called before the first frame update
    void Start()
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }

    /*
     Update is called once per frame
    */
    void Update()
    {
      // not let the player move if any of these cases happens
      if(gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive)
      {
        PlayerController.instance.canMove = false;
      }
      else
      {
        PlayerController.instance.canMove = true;
      }

      if (Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }
    }

    /*
     use item name string to return an Item Object
    */
    public Item GetItemDetails(string itemName)
    {
      for(int i = 0; i < refItems.Length; i++)
      {
        if(refItems[i].itemName == itemName)
        {
          return refItems[i];
        }
      }
      return null;
    }

    // public void SortItems()
    // {
    //   int idx = 0;
    //   for(int i = 0; i < itemsHeld.Length && idx < itemsHeld.Length; i++)
    //   {
    //     if(itemsHeld[i] == "")
    //     {
    //       if(idx < i) {
    //         idx = i;
    //       }
    //       while(idx < itemsHeld.Length && itemsHeld[idx] == "") {
    //         idx++;
    //       }
    //       if(idx < itemsHeld.Length) {
    //         itemsHeld[i] = itemsHeld[idx];
    //         numItems[i] = numItems[idx];
    //         itemsHeld[idx] = "";
    //         numItems[idx] = 0;
    //         idx++;
    //       }
    //     }
    //   }
    // }


    public void SortItems(string[] itemsArr)
    {
      int idx = 0;
      for(int i = 0; i < itemsArr.Length && idx < itemsArr.Length; i++)
      {
        if(itemsArr[i] == "")
        {
          if(idx < i) {
            idx = i;
          }
          while(idx < itemsArr.Length && itemsArr[idx] == "") {
            idx++;
          }
          if(idx < itemsArr.Length) {
            itemsArr[i] = itemsArr[idx];
            numItems[i] = numItems[idx];
            itemsArr[idx] = "";
            numItems[idx] = 0;
            idx++;
          }
        }
      }
    }

    public void AddItem(string itemToAdd)
    {
        // check valid item
        if(!CheckValid(itemToAdd)) {
          Debug.LogError(itemToAdd + " Does Not Exist!!");
          return;
        }
        bool added = false;
        // add to existing held item
        for(int i = 0; i < itemsHeld.Length; i++)
        {

            if(itemsHeld[i] == itemToAdd)
            {
              itemsHeld[i] = itemToAdd;
              numItems[i]++;
              added = true;
              break;
            }
        }
        // add to empty space
        if(!added) {
          for(int i = 0; i < itemsHeld.Length; i++)
          {
            if(itemsHeld[i] == "") {
              itemsHeld[i] = itemToAdd;
              numItems[i]++;
              break;
            }
          }
        }

        GameMenu.instance.ShowItems();
    }

    // remove one existing held item
    public void RemoveItem(string itemToRemove)
    {
      if(!CheckValid(itemToRemove)) {
        Debug.LogError(itemToRemove + " Does Not Exist!!");
        return;
      }

      bool removed = false;
      for(int i = 0; i < itemsHeld.Length; i++)
      {

          if(itemsHeld[i] == itemToRemove)
          {
            itemsHeld[i] = itemToRemove;
            numItems[i]--;
            removed = true;
            if(numItems[i] == 0) {
              itemsHeld[i] = "";
            }
            break;
          }
      }

      if(!removed) {
        Debug.LogError("Couldn't find " + itemToRemove);
      }
      GameMenu.instance.ShowItems();
    }


    /*
      Check the item is a valid item could be operated
    */
    private bool CheckValid(string itemName) {
      for(int i = 0; i < refItems.Length; i++)
      {
          if(refItems[i].itemName == itemName)
          {
              return true;
          }
      }
      return false;
    }

    /*
      Save the scene, player position, character info
    */
    public void SaveData()
        {
            PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
            PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
            PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

            //save character info for all players
            for(int i = 0; i < playerStats.Length; i++)
            {
                if(playerStats[i].gameObject.activeInHierarchy)
                {
                    PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1); // true
                } else
                {
                    PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0); // false
                }

                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentEXP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxMP);
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defence);
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].wpnPwr);
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armPwr);
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equippedWpn);
                PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equippedArmr);
            }

            //store inventory data
            for(int i = 0; i < itemsHeld.Length; i++)
            {
                PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
                PlayerPrefs.SetInt("ItemAmount_" + i, numItems[i]);
            }
        }

        public void LoadData()
        {
            PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"),
            PlayerPrefs.GetFloat("Player_Position_y"),
            PlayerPrefs.GetFloat("Player_Position_z"));

            for(int i = 0; i < playerStats.Length; i++)
            {
                if(PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0)
                {
                    playerStats[i].gameObject.SetActive(false);
                } else
                {
                    playerStats[i].gameObject.SetActive(true);
                }

                playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
                playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
                playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
                playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
                playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
                playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
                playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
                playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
                playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwr");
                playerStats[i].armPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
                playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWpn");
                playerStats[i].equippedArmr = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmr");
            }

            // inventory data
            for(int i = 0; i < itemsHeld.Length; i++)
            {
                itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
                numItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
            }
        }




}
