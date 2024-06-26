using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NarrativeController : MonoBehaviour
{
    public List<string> textList;
    public string Title;
    
    [SerializeField] private int index = 0;

    [SerializeField] private TMP_Text titleBox;
    

    [SerializeField] private RectTransform buttonNest;
    [SerializeField] private RectTransform narrativeNest; 
    
    //narrative
    private TMP_Text textBox;
    private Button nextButton;
    private Button prevButton;
    
    //story
    private Scenario story;
    private bool deleteCurrentLine = false;

    public string State = "Default";
    
    // Start is called before the first frame update
    void Awake()
    {
        
        setUpVars();
        ChangeState(State);
        LoadLine(0);
    }

    private void setUpVars()
    {
        textBox = narrativeNest.Find("text box").GetComponent<TMP_Text>();
        nextButton = narrativeNest.Find("continue").GetComponent<Button>();
        prevButton = narrativeNest.Find("Back").GetComponent<Button>();
        prevButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStory(Scenario scenario)
    {
        story = scenario;
        ChangeState("Story");

        titleBox.text = scenario.title;
        textList = scenario.story;
        LoadLine(0);

        int one = Random.Range(1, 4);
        int two = one;
        int three = one;
        while (two == one)
        {
            two = Random.Range(1, 4);
        }
        while (three == one || three == two)
        {
            three = Random.Range(1, 4);
        }
        
        print($"{one},{two},{three}");
        buttonNest.Find($"Option {one}").GetChild(0).GetComponent<TMP_Text>().text = scenario.optionOne.name;
        buttonNest.Find($"Option {two}").GetChild(0).GetComponent<TMP_Text>().text = scenario.optionTwo.name;
        buttonNest.Find($"Option {three}").GetChild(0).GetComponent<TMP_Text>().text = scenario.correctOption.name;
    }
    
    public void LoadLine(int stage)
    {
        if(textList.Count-1 < stage)
            return;
        
        if (titleBox.text != Title)
            titleBox.text = Title;
        
        this.index = stage;
        textList[this.index] = textList[this.index].Replace("\\n", "\n");
        textBox.text = textList[this.index];
        
        CheckButtons();
    }

    public void NextLine()
    {
        if (deleteCurrentLine)
            textList.RemoveAt(0);
        else
            this.index++;

        deleteCurrentLine = false;
        LoadLine(this.index);
    }

    public void PreviousLine()
    {
        this.index--;
        LoadLine(this.index);
    }

    private void CheckButtons()
    {
        if (index >= textList.Count-1)
            nextButton.interactable = false;
        else
            nextButton.interactable = true;
        if (index == 0)
            prevButton.interactable = false;
        else
            prevButton.interactable = true;
    }
    
    private void DeactivateText()
    {
        textBox.gameObject.SetActive(false);
    }
    private void ActivateText()
    {
        textBox.gameObject.SetActive(true);
    }

    public bool isAllTextRead()
    {
        if (index >= textList.Count - 1)
            return true;
        return false;
    }

    public void ChangeState(string newState)
    {
        switch (newState)
        {
            case "Default":
                buttonNest.gameObject.SetActive(false);
                narrativeNest.offsetMin = new Vector2(0,0);
                break;
            case "Story":
                buttonNest.gameObject.SetActive(true);
                narrativeNest.offsetMin = new Vector2(0,300);
                break;
            
            default:
                return;
        }
        State = newState;
    }

    private void ResetToDefault()
    {
        ChangeState("Default");
        textList.Clear();
        Title = "";
        textBox.text = "";
        titleBox.text = "";
    }

    public void CheckStoryButton(Button buttonClicked)
    {
        if (deleteCurrentLine)
        {
            textList.RemoveAt(0);
        }
        if (buttonClicked.transform.GetChild(0).GetComponent<TMP_Text>().text == story.correctOption.name)
        {
            textList.Insert(0,story.correctOptionReason);
            Invoke(nameof(ResetToDefault),5);
            LoadLine(0);
        }
        else if(buttonClicked.transform.GetChild(0).GetComponent<TMP_Text>().text == story.optionOne.name)
        {
            textList.Insert(0,story.optionOneReason);
            LoadLine(0);
            deleteCurrentLine = true;
        }
        else if(buttonClicked.transform.GetChild(0).GetComponent<TMP_Text>().text == story.optionTwo.name)
        {
            textList.Insert(0,story.optionTwoReason);
            LoadLine(0);
            deleteCurrentLine = true;
        }
    }
}
