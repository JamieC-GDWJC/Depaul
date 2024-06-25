using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private GameObject tutorialBlockingPanel;
    public NarList progList;
    public List<ExternalCommunication> assetsToActivate = new List<ExternalCommunication>();
    private int _index = 0;

    private NarrativeController _narrativeController;
    private Coroutine changeStageCoroutine;
    
    // Start is called before the first frame update
    private void Start()
    {
        _narrativeController = GetComponent<NarrativeController>();
        
        SetStage(_index);

        //changeStageCoroutine = StartCoroutine(ChangeTutorialStage());
         InvokeRepeating("ChangeTutorialStage", .5f,.5f);
    }

    private void SetStage(int stage)
    {
        if (stage >= progList.Stage.Count-1)
        {
            CancelInvoke();
        }
        print("stage: " + stage);
        _index = stage;
        
        assetsToActivate.Clear();
        assetsToActivate = progList.Stage[_index].activeObjects;

        _narrativeController.textList.Clear();
        _narrativeController.textList = progList.Stage[_index].Lines;

        _narrativeController.Title = progList.Stage[_index].Title;
        setPanel(true);
        _narrativeController.LoadLine(0);
    }

    void setPanel(bool activate)
    {
        tutorialBlockingPanel.SetActive(activate);
    }

    bool CheckIfStageIsComplete()
    {
        foreach (ExternalCommunication asset in assetsToActivate)
        {
            if (!asset.active)
                return false;
        }

        return true;
    }

    void ChangeTutorialStage()
    {

        print("check if stage is complete");
        if (CheckIfStageIsComplete())
        {
            print("next Stage!");
            SetStage(_index+1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_narrativeController.isAllTextRead() && tutorialBlockingPanel.activeSelf)
        {
            setPanel(false);
        }
    }
}
