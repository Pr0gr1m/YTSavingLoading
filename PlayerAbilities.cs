using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.Playables;

public class PlayerAbilities : MonoBehaviour
{
    [SerializedDictionary("Ability","Level")]
    public SerializedDictionary<AbilityEnum, int> AbilityLevelsDictonary = new SerializedDictionary<AbilityEnum, int>();
    public List<AbilityEnum> RegisteredAbilityEnums;

    [SerializedDictionary("A", "E")]
    public SerializedDictionary<AbilityEnum, List<GameObject>> SortedDictionary = new SerializedDictionary<AbilityEnum, List<GameObject>>();

    public GameObject[] UICategories;

    public int Experience;
    public Text ExperienceText;

    public string path;

    private void Awake()
    {
        AbilityLevelsDictonary.Clear();

        foreach(AbilityEnum ability in RegisteredAbilityEnums)
        {
            AbilityLevelsDictonary.Add(ability, 0);
        }
    }

    private void Update()
    {
        ExperienceText.text = Experience.ToString();   
    }

    public void UpdradeAbility(AbilityEnum ability, int experience)
    {
        int lvl = AbilityLevelsDictonary[ability];
        AbilityLevelsDictonary.Remove(ability);
        AbilityLevelsDictonary.Add(ability, lvl + 1);

        Experience -= experience;
    }

    public void SaveAbilities()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream;

        if (!File.Exists(Application.persistentDataPath + path))
        {
            stream = File.Create(Application.persistentDataPath + path);
        }
        else
        {
            stream = File.Open(Application.persistentDataPath + path, FileMode.Open);
        }

        formatter.Serialize(stream, new SaveData
        {
            AbilityList = AbilityLevelsDictonary.Keys.ToList(),
            LevelList = AbilityLevelsDictonary.Values.ToList(),
            experience = Experience,
            BuyedButtonsIDs = GetBoughtAbilityButtonsIDs()
        });

        stream.Close();
    }

    public void LoadAbilities()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(Application.persistentDataPath + path, FileMode.Open);
        
        SaveData saveData = (SaveData)formatter.Deserialize(stream);
        Dictionary<AbilityEnum, int> dictionary = new Dictionary<AbilityEnum, int>();
        int i = 0;

        foreach(AbilityEnum ability in saveData.AbilityList)
        {
            dictionary.Add(ability, saveData.LevelList[i]);
            i += 1;
        }

        AbilityLevelsDictonary = new SerializedDictionary<AbilityEnum, int>(dictionary);
        Experience = saveData.experience;

        OnLoadChangeUI(saveData.BuyedButtonsIDs);
            
        stream.Close();
    }

    public void OnLoadChangeUI(List<string> buyedButtonsIDs)
    {
        List<AbilityButton> allAbilityButtons = GetAllAbilityButtons();

        List<AbilityButton> buyedAbilityButtons = new List<AbilityButton>();

        foreach (AbilityButton button in allAbilityButtons)
        {
            button.isBought = false;
            button.GetComponent<UnityEngine.UI.Image>().color = button.NotAvaibleButton;

            if(buyedButtonsIDs.Contains(button.ID))
            {
                buyedAbilityButtons.Add(button);
            }

            if(button.isFirstInList)
            {
                button.GetComponent<UnityEngine.UI.Image>().color = button.AvaibleColor;
            }
        }

        foreach (AbilityButton button in buyedAbilityButtons)
        {
            button.isBought = true;
            button.GetComponent<UnityEngine.UI.Image>().color = button.BoughtColor;
            if(button.nextButton) button.nextButton.GetComponent<UnityEngine.UI.Image>().color = button.AvaibleColor;
        }
    }

    public List<string> GetBoughtAbilityButtonsIDs()
    {
        List<string> AbilityButtons = new List<string>();

        foreach (GameObject category in UICategories)
        {
            foreach(AbilityButton button in category.GetComponentsInChildren<AbilityButton>())
            {
                if(button.isBought) AbilityButtons.Add(button.ID);
            }
        }

        return AbilityButtons;
    }

    public List<AbilityButton> GetAllAbilityButtons()
    {
        List<AbilityButton> AbilityButtons = new List<AbilityButton>();

        foreach (GameObject category in UICategories)
        {
            foreach (AbilityButton button in category.GetComponentsInChildren<AbilityButton>())
            {
                AbilityButtons.Add(button);
            }
        }

        return AbilityButtons;
    }
}

public enum AbilityEnum
{
    Speed, AttackDmg, AttackSpeed, Luck
}

[Serializable]
public struct SaveData
{
    public List<AbilityEnum> AbilityList;
    public List<int> LevelList;

    public List<string> BuyedButtonsIDs;
    public int experience;
}