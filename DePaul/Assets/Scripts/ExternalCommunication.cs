using System;
using UnityEngine;

public class ExternalCommunication : MonoBehaviour
{
    public bool AutoClickActive = false;
    public bool active = false;

    public UIController uiController;

    private void Start()
    {
        uiController = GetComponent<UIController>();
    }
}
