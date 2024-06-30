using System.Collections.Generic;
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
    private StoryScenario story;
    private bool deleteCurrentLine = false;

    private List<TMP_Text> buttonList = new List<TMP_Text>();

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

        TMP_Text button1 = buttonNest.Find($"Option 1").GetChild(0).GetComponent<TMP_Text>();
        buttonList.Add(button1);
        TMP_Text button2 = buttonNest.Find($"Option 2").GetChild(0).GetComponent<TMP_Text>();
        buttonList.Add(button2);
        TMP_Text button3 = buttonNest.Find($"Option 3").GetChild(0).GetComponent<TMP_Text>();
        buttonList.Add(button3);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStory(StoryScenario storyScenario)
    {
        story = storyScenario;
        ChangeState("Story");

        titleBox.text = storyScenario.title;
        textList = storyScenario.story;
        LoadLine(0);

        int one = Random.Range(0, 3);
        int two = one;
        int three = one;
        while (two == one)
        {
            two = Random.Range(0, 3);
        }
        while (three == one || three == two)
        {
            three = Random.Range(0, 3);
        }
        buttonList[one].text = storyScenario.o1.name;
        buttonList[two].text = storyScenario.o1.name;
        buttonList[three].text = storyScenario.o1.name;
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
        else if(buttonClicked.transform.GetChild(0).GetComponent<TMP_Text>().text == story.o1.name)
        {
            textList.Insert(0,story.o1Reason);
            LoadLine(0);
            deleteCurrentLine = true;
        }
        else if(buttonClicked.transform.GetChild(0).GetComponent<TMP_Text>().text == story.o2.name)
        {
            textList.Insert(0,story.o2Reason);
            LoadLine(0);
            deleteCurrentLine = true;
        }
    }
}
