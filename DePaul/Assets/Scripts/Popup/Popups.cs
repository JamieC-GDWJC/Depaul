using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;

public class Popups : MonoBehaviour
{
    public bool active = false;
    public List<PopupScenario> scenarios = new List<PopupScenario>();
    private GameObject focusPanel;

    public int index = 0;

    [SerializeField] private int invokeTimer;

    private UIController currentActivePopup;
    private NarrativeController _narrativeController;
    void Start()
    {
        _narrativeController = FindObjectOfType<NarrativeController>();
        focusPanel = GameObject.Find("Focus Panel");
    }

    // Update is called once per frame
    void Update()
    {
        if (_narrativeController.isAllTextRead() && focusPanel.activeSelf)
        {
            setPanel(false);
        }
    }

    public void ActivatePopupStories(bool state)
    {
        active = state;
        if (active)
        {
            InvokeRepeating("ActivatePopup",invokeTimer,invokeTimer);
            print("popup activated");
        }
    }

    IEnumerator WhilePopupActive(PopupScenario scenario)
    {
        yield return new WaitUntil(() => scenario.complete == true);
        DeacrivatePopup();
        index++;
    }

    void ActivatePopup()
    {
        if(_narrativeController.State != "Default")
            return;
        
        PopupScenario scenario = scenarios[index];
        _narrativeController.LoadPopup(scenario);
        StartCoroutine(WhilePopupActive(scenario));
    }

    void DeacrivatePopup()
    {
        _narrativeController.ResetToDefault();
    }
    
    
    void setPanel(bool activate)
    {
        focusPanel.SetActive(activate);
    }
}
