using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StandController : MonoBehaviour
{
    [Header("Global")]
    public int value;
    public int cost;

    public string Name;
    
    private bool clickable;
    
    public float cooldownTime = 5f;
    private float lastUsedTime = 0;

    [Header("Activate Conditions")]
    public bool active = false;
    public Material activeMat;

    [Header("Local")] 
    public bool autoClick = false;

    [SerializeField] private TMP_Text costT;
    [SerializeField] private TMP_Text incomeT;
    [SerializeField] private TMP_Text timeT;
    [SerializeField] private TMP_Text info;
    [SerializeField] private GameObject buyMenu;
    [SerializeField] private GameObject activeMenu;
    [SerializeField] private GameObject bubbleMenu;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    public Slider slider;

    [Header("Audio")] 
    [SerializeField] private AudioSource buyAudioClip;
    public AudioSource getMoneyAudioClip;
    [SerializeField] private AudioSource interactAudioClip;
    [SerializeField] private AudioSource rejectAudioClip;
    
    
    private GameManager GM;
    private Camera _camera;
    private bool UIActive = false;
    private ExternalCommunication EC;
    // Start is called before the first frame update
    void Awake()
    {
        _camera = Camera.main;
        GM = FindObjectOfType<GameManager>();
        lastUsedTime = Time.time - cooldownTime;
        slider.gameObject.SetActive(false);
        EC = GetComponent<ExternalCommunication>();
        
        SetUpText();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            CanClick();
            UpdateSlider();
            AutoClick();
        }
        if (UIActive)
            UIPostion();
        
    }

    void UIPostion()
    {
        GameObject menu;
        if (buyMenu.activeSelf)
            menu = buyMenu;
        else
            menu = activeMenu;
        
        Vector3 pos = _camera.WorldToScreenPoint(transform.position);
        pos.y += 75;
        if (pos.y >= 400)
        {
            pos.y -= 200;
            bubbleMenu.transform.rotation =  Quaternion.Euler(new Vector3(0,0,180));
            bubbleMenu.transform.position = pos + new Vector3(0,40,0);
        }
        else
        {
            bubbleMenu.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            bubbleMenu.transform.position = pos;
        }

        info.transform.position = pos + new Vector3(0, 50, 0);
        menu.transform.position = pos;
        
    }
    void SetUpText()
    {
        info.text = gameObject.name;
        costT.text = cost.ToString();
        incomeT.text = value.ToString();
        timeT.text = cooldownTime.ToString();
        _canvasGroup.alpha = 0f;
        buyMenu.transform.localScale = new Vector3(0,0,0);
        activeMenu.transform.localScale = new Vector3(0,0,0);
        bubbleMenu.transform.localScale = new Vector3(0,0,0);
        UIActive = false;
    }
    void CanClick()
    {
        if (Time.time > lastUsedTime + cooldownTime)
            clickable = true;
        else
            clickable = false;
    }
    void OnMouseDown()
    {
        if (clickable && active)
            StartCollection();

        else if (!active && GM.donationValue >= cost)
        {
            Buy();
        }
        else
        {
            PlayAudioClip("reject");
        }
    }
    

    private void OnMouseEnter()
    {
        _canvasGroup.alpha = 1;
        buyMenu.transform.localScale = new Vector3(1.59f,1.59f,1.59f);
        activeMenu.transform.localScale = new Vector3(1.59f,1.59f,1.59f);
        bubbleMenu.transform.localScale = new Vector3(1.59f,1.59f,1.59f);
        UIActive = true;
        PlayAudioClip("interact");
    }

    private void OnMouseExit()
    {
        _canvasGroup.alpha = 0f;
        buyMenu.transform.localScale = new Vector3(0,0,0);
        activeMenu.transform.localScale = new Vector3(0,0,0);
        bubbleMenu.transform.localScale = new Vector3(0,0,0);
        UIActive = false;
    }

    void StartCollection()
    {
        lastUsedTime = Time.time;
        GM.AddDono(value);
        PlayAudioClip("collect");
    }

    void UpdateSlider()
    {
        if (!clickable)
            slider.value = (Time.time-lastUsedTime)/cooldownTime;
        if (clickable)
        {
            slider.value = 1;
        }
    }

    void AutoClick()
    {
        if (autoClick && clickable)
        {
            StartCollection();
        }
        else if(!autoClick)
        {
            if (EC.AutoClickActive)
                autoClick = true;
        }
    }

    void Buy()
    {
        active = true;
        EC.active = active;
        slider.gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = activeMat;
        GM.SpendDono(cost);
        
        buyMenu.SetActive(false);
        activeMenu.SetActive(true);
        lastUsedTime = Time.time;
        
        PlayAudioClip("buy");
    }

    void PlayAudioClip(string reason)
    {

        switch (reason)
        {
            case "buy":
                buyAudioClip.Play();
                break;
            case "collect":
                getMoneyAudioClip.Play();
                break;
            
            case "reject":
                rejectAudioClip.Play();
                break;
            
            case "interact":
                interactAudioClip.Play();
                break;
            
        }
    }
    
}
