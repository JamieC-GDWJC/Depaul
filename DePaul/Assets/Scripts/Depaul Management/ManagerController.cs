using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ManagerController : MonoBehaviour
{
    [Header("Global")]
    public int value;
    public int cost;
    public int costToRun;
    
    public string _name;
    
    private bool _clickable;

    private TMP_Text _info;

    [Header("Activate Conditions")]
    public bool active = false;
    public Material activeMat;

    [Header("Local")] 
    public bool autoClick = false;
    public string standType;

    private GameObject[] _stands;
    private GameManager _gm;

    private ExternalCommunication _ec;
    // Start is called before the first frame update
    void Awake()
    {
        _gm = FindObjectOfType<GameManager>();
        _info = transform.GetChild(2).Find("Info").GetComponent<TMP_Text>();
        _info.text = _name + "\nCost: " + cost.ToString();
        
        _stands = GameObject.FindGameObjectsWithTag(standType);
        _ec = GetComponent<ExternalCommunication>();
                
    }

    void OnMouseDown()
    {

        if (!active && _gm.donationValue >= cost)
        {
            Buy();
        }
    }
    
    public void Activate()
    {
        foreach (GameObject stand in _stands)
        {
            stand.GetComponent<ExternalCommunication>().AutoClickActive = true;
            stand.GetComponent<StandController>().getMoneyAudioClip.volume = .2f;
        }
        
    }

    void Buy()
    {
        active = true;
        _ec.active = active;
        _gm.SpendDono(cost);
        transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = activeMat;
        _info.text = _name + "is active";
        Activate();
        GetComponent<AudioSource>().Play();
    }
}
