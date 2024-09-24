using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    [HideInInspector] public Navigation navigation;
    [HideInInspector] public DynamicTextScroller scroller;
    private List<string> actionLog = new List<string>();
    [SerializeField] private float typingSpeed = 0.04f;
    private bool effectOn = false;
    private bool canContinueStory = false;
    private bool skip = false;
    private Coroutine displayEffectCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        DisplayStoryText();
        DisplayLoggedText();
    }

    void Awake()
    {
        navigation = GetComponent<Navigation>();
        scroller = GetComponent<DynamicTextScroller>();
    }

    void Update()
    {
        if (canContinueStory)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                skip = true;
            }
        }
    }

    public void DisplayLoggedText()
    {
        if (effectOn)
        {
            string logAsText = string.Join("\n", actionLog.Take(actionLog.Count - 1).ToArray());

            if (displayEffectCoroutine != null)
            {
                StopCoroutine(displayEffectCoroutine);
            }

            displayEffectCoroutine = StartCoroutine(TypewriterEffect(logAsText, "\n" + actionLog.Last()));
            effectOn = false;
        }
        else
        {
            string logAsText = string.Join("\n", actionLog.ToArray());
            displayText.text = logAsText;
        }
        
        scroller.UpdateContentSize();
    }

    public void DisplayStoryText()
    {
        ClearCollectionsForNewPart();
        Unpack();
        string combinedText = navigation.currentPart.story + "\n";
        effectOn= true;
        LogStringWithReturn(combinedText);
    }

    private void Unpack()
    {
        navigation.UnpackExits();
    }

    private void ClearCollectionsForNewPart()
    {
        navigation.ClearExits();
    }

    public void DisplayHint()
    {
        effectOn = true;
        LogStringWithReturn(navigation.currentPart.hint);
    }

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
        scroller.UpdateContentSize();
    }

    private IEnumerator TypewriterEffect(string oldPart, string nextPart)
    {
        displayText.text = oldPart;
        navigation.SetInputFieldInactive();

        bool isAddingRichTextTag = false;

        foreach (char letter in nextPart.ToCharArray())
        {
            canContinueStory = true;
            if (skip)
            {
                displayText.text = oldPart + nextPart;
                skip = false;
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                displayText.text += letter;

                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                displayText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        canContinueStory = false;
        
        if (!navigation.currentPart.part.Contains("ending"))
        {
            navigation.SetInputFieldActive();
        }
    }
}
