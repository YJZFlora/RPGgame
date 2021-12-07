using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{

  public string charName;
  public int playerLevel = 1;
  public int currentEXP; // current experience
  public int[] expToNextLevel;
  public int baseEXP = 100;
  public int maxLevel = 100;

  public int currentHP;
  public int maxHP = 100;
  public int currentMP;
  public int maxMP = 30;
  public int[] mpLvBonus;
  public int strength;
  public int defence;
  public int wpnPwr; // wapon power
  public int armPwr;
  public string equippedWpn;
  public string equippedArmr;
  public Sprite charImage;


    // Start is called before the first frame update
    void Start()
    {
      expToNextLevel = new int[maxLevel];
      expToNextLevel[1] = baseEXP;
      for(int i = 2; i < expToNextLevel.Length; i++)
      {
        // Debug.Log(i);
        expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i-1] * 1.05f); // curve
      }

    }

    // Update is called once per frame
    void Update()
    {
      // for test purpose
      if(Input.GetKey(KeyCode.K))
      {
        AddExp(50);
      }
    }

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if (playerLevel < maxLevel)
        {
            if (currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];

                playerLevel++;

                //determine whether to add to str or def based on odd or even
                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defence++;
                }

                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                if(playerLevel < mpLvBonus.Length) {
                  maxMP += mpLvBonus[playerLevel];
                }

                currentMP = maxMP;
            }
        }

        if(playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
