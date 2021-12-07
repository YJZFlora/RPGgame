using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public string[] questMarkerNames;
    public bool[] questMarkersComplete;

    public static QuestManager instance;

	// Use this for initialization
	void Start () {
        instance = this;

        questMarkersComplete = new bool[questMarkerNames.Length];
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(CheckIfComplete("quest test"));
            MarkQuestComplete("quest test");
            MarkQuestIncomplete("fight the demon");
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            SaveQuestData();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            LoadQuestData();
        }
	}

    private int GetQuestNumber(string questToFind)
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            if(questMarkerNames[i] == questToFind)
            {
                return i;
            }
        }

        Debug.LogError("Quest " + questToFind + " does not exist");
        return -1;
    }

    public bool CheckIfComplete(string questToCheck)
    {
       int questIdx = GetQuestNumber(questToCheck);
        if(questIdx != -1)
        {
            return questMarkersComplete[questIdx];
        }

        return false;
    }

    public void MarkQuestComplete(string questToMark)
    {
      int questIdx = GetQuestNumber(questToMark);
      questMarkersComplete[questIdx] = true;

        UpdateLocalQuestObjects();
    }

    public void MarkQuestIncomplete(string questToMark)
    {
        int questIdx = GetQuestNumber(questToMark);
        questMarkersComplete[questIdx] = false;

        UpdateLocalQuestObjects();
    }

    public void UpdateLocalQuestObjects()
    {
      // find objects in the scene that has the script QuestObjectActivator attached
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if(questObjects.Length > 0)
        {
            for(int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
    }

    /*
      Save data by saving all quests and their status (complete or not)
    */
    public void SaveQuestData()
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            if(questMarkersComplete[i])
            {
              // key - value
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 1); //true
            } else
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 0); // false
            }
        }
    }

    /*
      Load data by reading saved quests
    */
    public void LoadQuestData()
    {
        for(int i = 0; i < questMarkerNames.Length; i++)
        {
            int valueToSet = 0;
            if(PlayerPrefs.HasKey("QuestMarker_" + questMarkerNames[i]))
            {
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + questMarkerNames[i]);
            }

            if(valueToSet == 0)
            {
                questMarkersComplete[i] = false;
            } else
            {
                questMarkersComplete[i] = true;
            }
        }
    }


}
