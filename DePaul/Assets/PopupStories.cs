using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PopupStories : MonoBehaviour
{
    public List<Scenario> scenarios = new List<Scenario>();
    private GameObject focusPanel;

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
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            _narrativeController.LoadStory(scenarios[0]);
        }
    }
    
}
