using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeController : MonoBehaviour
{
    public List<string> textList;
    public string Title;
    
    [SerializeField] private int index = 0;

    [SerializeField] private TMP_Text titleBox;
    

    [SerializeField] private RectTransform buttonNest;
    [SerializeField] private RectTransform narrativeNest; 
    
    private TMP_Text textBox;
    private Button nextButton;
    private Button prevButton;

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
        this.index++;
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
}
