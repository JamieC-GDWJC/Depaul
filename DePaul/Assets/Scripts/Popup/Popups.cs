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

    private int index = 0;

    [SerializeField] private Preset uiControllerPreset;

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
            StartCoroutine("Popup");
        }
    }

    IEnumerator Popup()
    {
        print("attemping popup");
        yield return new WaitUntil(() => scenarios[index].affectedAsset.active);

        setPanel(true);
        //_narrativeController.LoadStory(scenarios[index]);
        PopupScenario scenario = scenarios[index];
        
        currentActivePopup = scenario.affectedAsset.AddComponent<UIController>();
        currentActivePopup.isPopup = true;
        
        yield return new WaitUntil(() => currentActivePopup.instantiated);
        print("passed instantiation");
        currentActivePopup.AddInfoField("Buy", scenario.cost.ToString(),false);
        currentActivePopup.AddInfoField("Cooldown", scenario.time.ToString());
        
        if(scenario.peopleHelped != 0)
            currentActivePopup.AddInfoField("People Helped", scenario.peopleHelped.ToString());
        
        if(scenario.donationesEarned != 0)
            currentActivePopup.AddInfoField("Income", scenario.donationesEarned.ToString());
        
        currentActivePopup.ShowUI(true);
        currentActivePopup.Function = BuyPopup;
        
        index++;

        yield return new WaitUntil(() => scenario.complete);
        currentActivePopup.DestroyUI();
    }

    void BuyPopup()
    {
        
    }
    
    void setPanel(bool activate)
    {
        focusPanel.SetActive(activate);
    }
}
