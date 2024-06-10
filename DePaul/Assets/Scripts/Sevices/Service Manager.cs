using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ServiceManager : MonoBehaviour
{
    [Header("Global")]
    public int value;
    public int cost;
    public int costToRun;

    public string name;
    
    private bool clickable;
    
    public float cooldownTime = 5f;
    private float lastUsedTime = 0;

    [Header("Activate Conditions")]
    public bool active = false;
    public Material activeMat;

    [Header("Local")] 
    public bool autoClick = false;
    
    [SerializeField] private TMP_Text costT;
    [FormerlySerializedAs("incomeT")] [SerializeField] private TMP_Text costToRunT;
    [SerializeField] private TMP_Text timeT;
    [SerializeField] private TMP_Text info;
    [SerializeField] private TMP_Text peopleT;
    [SerializeField] private GameObject buyMenu;
    [SerializeField] private GameObject activeMenu;
    [SerializeField] private GameObject bubbleMenu;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Slider slider;
    
    
    [Header("Audio")] 
    [SerializeField] private AudioSource buyAudioClip;
    [SerializeField] private AudioSource spendMoneyAudioSource;
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
            pos.y -= 210;
            bubbleMenu.transform.rotation =  Quaternion.Euler(new Vector3(0,0,180));
            bubbleMenu.transform.position = pos + new Vector3(0,30,0);
        }
        else
        {
            bubbleMenu.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            bubbleMenu.transform.position = pos;
        }

        info.transform.position = pos + new Vector3(0, 50, 0);
        menu.transform.position = pos;
    }
    
    void CanClick()
    {
        if (Time.time > lastUsedTime + cooldownTime && GM.donationValue >= costToRun)
            clickable = true;
        else
            clickable = false;
    }
    
    void SetUpText()
    {
        info.text = gameObject.name;
        costT.text = cost.ToString();
        costToRunT.text = costToRun.ToString();
        timeT.text = cooldownTime.ToString();
        peopleT.text = value.ToString();
        _canvasGroup.alpha = 0;
        buyMenu.transform.localScale = new Vector3(0,0,0);
        activeMenu.transform.localScale = new Vector3(0,0,0);
        bubbleMenu.transform.localScale = new Vector3(0,0,0);
        UIActive = false;
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

    void StartCollection()
    {
        lastUsedTime = Time.time;
        GM.SpendDono(costToRun);
        GM.AddImpact(value);
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
        if (autoClick && clickable && active)
        {
            StartCollection();
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

    void Buy()
    {
        active = true;
        EC.active = active;
            
        MeshRenderer MR = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        MR.material = activeMat;
        
        buyMenu.SetActive(false);
        activeMenu.SetActive(true);
        GM.SpendDono(cost);
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
                spendMoneyAudioSource.Play();
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
