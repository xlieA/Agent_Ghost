using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    Dictionary<string, Story> exitDictionary = new Dictionary<string, Story>();
    public Story currentPart;
    private GameController controller;
    [SerializeField] private TMP_InputField userInput;
    private string userName;
    private int missCount;

    void Awake()
    {
        controller = GetComponent<GameController>();
        //userInput = FindObjectOfType<TMP_InputField>();
    }

    public void UnpackExits()
    {
        for (int i = 0; i < currentPart.exits.Length; i++)
        {
            for (int j = 0; j < currentPart.exits[i].keyString.Count; j++)
            {
                exitDictionary.Add(currentPart.exits[i].keyString[j], currentPart.exits[i].valueStory);
            }
        }
    }

    public void AttemptToChangeParts(string key)
    {
        Debug.Log(currentPart.part);
        bool match = false;

        if (currentPart.part == "part1")
        {
            ChangeToPart2(key);
        }
        else
        {
            foreach (string k in exitDictionary.Keys)
            {
                if (key.Contains(k))
                {
                    if (currentPart.part == "part12")
                    {
                        ChangeToPart13(key);
                        match = true;
                        break;
                    }

                    currentPart = exitDictionary[k];
                    controller.DisplayStoryText();

                    match = true;

                    if (currentPart.part.Contains("ending"))
                    {
                        SetInputFieldInactive();
                    }

                    break;
                }
            }
        }

        if (missCount == 2 && currentPart.hint != "")
        {
            controller.DisplayHint();
        }


        if (!match)
        {
            missCount++;
        }
    }

    private void ChangeToPart2(string key)
    {
        userName = key;

        string dictKey = exitDictionary.Keys.First();
        exitDictionary[dictKey].story = "Hello " + userName + "! Do you <i>remember</i>, " + userName + "?";

        currentPart = exitDictionary.First().Value;
        controller.DisplayStoryText();
    }

    private void ChangeToPart13(string key)
    {
        exitDictionary[key].story = "You can’t do it. You aren’t strong enough. Ben is going to drag you both down. " +
            "Carefully you try to convert all of the weight into one hand. The additional ballast is hurting your shoulder. " +
            "With your free hand, you try to reach your pants. " +
            "Your hand slides into your pocket and you can feel the cold metal in your hand. You pause for a moment. " +
            "When you pull out your hand, it’s shaking. The knife feels heavy.\n" +
            "Should you really do this? You may be able to hold on for a little longer. But no one’s going to save you. " + 
            "There is no help, there will be no help. You are the only people on this mountain.\n" +
            "If you don’t cut the safety rope, Ben will kill you both. If you cut the safety rope at least one of you could live.\n" +
            "Ben has been a total jerk to you all day. He has been bragging about his money and his house all the time. " + 
            "And you tried your best to help him. But even now, he isn’t listening to you and won’t stop moving. " +
            "If he only listened to you, you could help him. But he won’t!\n" +
            "You feel your hand slowly losing grip. You have to cut the safety rope, " + userName + ".";

        currentPart = exitDictionary[key];
        controller.DisplayStoryText();
    }

    public void SetInputFieldInactive()
    {
        userInput.gameObject.SetActive(false);
    }

    public void SetInputFieldActive()
    {
        userInput.gameObject.SetActive(true);
        userInput.ActivateInputField();
        userInput.Select();
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
        missCount = 0;
    }
}
