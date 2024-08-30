using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    private GameObject focusPanel;
    public List<NarPoint> narationStages;
    public List<ExternalCommunication> assetsToActivate = new List<ExternalCommunication>();
    private int _index = 0;

    private NarrativeController _narrativeController;
    private Coroutine changeStageCoroutine;
    
    // Start is called before the first frame update
    private void Start()
    {
        focusPanel = GameObject.Find("Focus Panel");
        _narrativeController = GetComponent<NarrativeController>();
        
        SetStage(_index);

        //changeStageCoroutine = StartCoroutine(ChangeTutorialStage());
         InvokeRepeating("ChangeTutorialStage", .5f,.5f);
    }

    private void SetStage(int stage)
    {
        if (stage >= narationStages.Count-1)
        {
            CancelInvoke();
        }
        _index = stage;
        
        assetsToActivate.Clear(); 
        assetsToActivate = narationStages[_index].activeObjects;
        
        _narrativeController.LoadNarrative(narationStages[_index].Title,narationStages[_index].Lines);
        _narrativeController.setPanel(true);
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

        //print("check if stage is complete");
        if (CheckIfStageIsComplete())
        {
            print("next Stage!");
            SetStage(_index+1);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (assetsToActivate.Count == 0 && _narrativeController.isAllTextRead())
        {
            GetComponent<ProgressionManager>().enabled = true;
            this.enabled = false;
        }
    }
}
