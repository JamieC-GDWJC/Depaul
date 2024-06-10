using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeController : MonoBehaviour
{
    public List<string> text;

    public int index = 0;

    public TMP_Text textBox;

    public TMP_Text nextButtonText;
    public TMP_Text prevButtonText;

    public Button nextButton;
    public Button prevButton;

    public NarList narList = new NarList();

    public int stage = 0;
    public bool done = false;
    public int maxLines = 3;
    public ExternalCommunication[] externalCommunications;
    
    // Start is called before the first frame update
    void Start()
    {
        textBox.text = text[index];
        nextButtonText.text = "Next";
        prevButtonText.text = "Previous";
        prevButton.interactable = false;
        stage = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (stage >= narList.Stage.Count || done)
            return;
        
        foreach (ExternalCommunication EC in externalCommunications)
        {
            if(!EC.active)
                return;
        }
        ChangeStage();
    }
    
    void ChangeStage()
    {
        stage ++;
        if (stage >= narList.Stage.Count)
        {
            done = true;
            GetComponent<StoryManager>().storiesActive = true;
            return;
        }
        externalCommunications = narList.Stage[stage].activeObjects;
        maxLines = narList.Stage[stage].Lines;
        if (maxLines == 2)
        {
            return;
        }
        NextLine();
    }

    public void NextLine()
    {
        if(done)
            return;
        if (index >= maxLines)
        {
            return;
        }
        if (nextButtonText.text == "Close")
        {
            //DeactivateText();
            textBox.text = "";
            return;
        }
        index++;
        textBox.text = text[index];
        CheckNextButton();
        CheckPrevButton();
    }

    public void PreviousLine()
    {
        if(done)
            return;
        index--;
        textBox.text = text[index];
        CheckNextButton();
        CheckPrevButton();
    }

    void CheckNextButton()
    {
        
        if (index >= text.Count - 1)
        {
            nextButtonText.text = "Close";
        }
        else
        {
            nextButtonText.text = "Next";
        }
    }
    
    void CheckPrevButton()
    {
        
        if (index == 0)
        {
            prevButton.interactable = false;
        }
        else
        {
            prevButton.interactable = true;
        }
    }
    
    public void DeactivateText()
    {
        textBox.gameObject.SetActive(false);
    }
    public void ActivateText()
    {
        textBox.gameObject.SetActive(true);
    }
}
