using System.Collections.Generic;
using UnityEngine;

public class Stories : MonoBehaviour
{
    public bool active = false;
    public List<StoryScenario> scenarios = new List<StoryScenario>();
    private GameObject focusPanel;

    private int index = 0;

    [SerializeField] private int invokeTimer;

    private NarrativeController _narrativeController;
    // Start is called before the first frame update
    void Start()
    {
        _narrativeController = FindObjectOfType<NarrativeController>();
        focusPanel = GameObject.Find("Focus Panel");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivatePopupStories(bool state)
    {
        active = state;
        if (active)
        {
            InvokeRepeating("Story",invokeTimer,invokeTimer);
        }
    }

    void Story()
    {
        if(_narrativeController.State != "Default")
            return;
        
        if (scenarios[index].o1.active && scenarios[index].o2.active && scenarios[index].correctOption.active && active)
        {
            _narrativeController.setPanel(true);
            _narrativeController.LoadStory(scenarios[index]);
            index++;
        }
    }
    
}
