using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameObject canvasGameObject;
    [SerializeField] private GameObject UIBubble;

    [SerializeField] private GameObject InfoModule;
    [SerializeField] private float UIOffset;

    private GameObject Bubble;
    private List<GameObject> InfoModules = new List<GameObject>();
    private Transform infoLocation;
    private Transform buyLocation;
    
    private InfoModuleTypes moduleTypes;
    VerticalLayoutGroup contentLayoutGroup;

    private BuyAsset _buyAsset;

    private Camera mainCamera;

    private bool isShowing;
    // Start is called before the first frame update
    void Start()
    {
        canvasGameObject = GameObject.FindWithTag("Hud");
        moduleTypes = FindObjectOfType<InfoModuleTypes>();
        _buyAsset = GetComponent<BuyAsset>();
        
        mainCamera = Camera.main;
        
        InstantiateUI();
        ShowUI(false);
    }

    void InstantiateUI()
    {
        Bubble = Instantiate(UIBubble,canvasGameObject.transform);
        infoLocation = Bubble.transform.Find("BubbleUI/Content/Info");
        buyLocation = Bubble.transform.Find("BubbleUI/Content/Buy");
        buyLocation.GetComponent<Button>().onClick.AddListener(delegate { _buyAsset.Buy(); });
        contentLayoutGroup = Bubble.transform.Find("BubbleUI/Content").GetComponent<VerticalLayoutGroup>();
    }

    public void AddInfoField(string field, string content, bool isInfo = true)
    {
        Sprite icon = GetIcon(field);
        GameObject module;
        
        if(isInfo)
            module = Instantiate(InfoModule, infoLocation);
        else 
            module = Instantiate(InfoModule, buyLocation);
        
        module.name = field;
        module.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = icon;
        module.transform.Find("Text").GetComponent<TMP_Text>().text = content;
        InfoModules.Add(module);

        StartCoroutine(UpdateLayoutGroup());
    }
    
    IEnumerator UpdateLayoutGroup()
    {
        contentLayoutGroup.enabled = false;
        yield return new WaitForEndOfFrame();
        contentLayoutGroup.enabled = true;
    }
    
    Sprite GetIcon(string type)
    {
        foreach (InfoModuleType module in moduleTypes.InfoModules)
        {
            if (type == module.Type)
                return module.Icon;
        }

        return null;
    }

    private Vector3 pos;

    private float cooldownTime = 2;

    private float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if(!isShowing)
            return;
        
        pos = mainCamera.WorldToScreenPoint(transform.position + new Vector3(0, UIOffset, 0));
        if (Bubble.transform.position != pos)
        {
            Bubble.transform.position = pos;
        }

        if (!IsMouseOverUi)
        {
            timer += Time.deltaTime;
            if(timer >= cooldownTime)
                ShowUI(false);
        }
        else
        {
            timer = 0;
        }
    }

    public void ShowUI(bool show)
    {
        timer = 0;
        isShowing = show;
        print("show UI: " + show);
        Bubble.SetActive(show);
    }
    

    public static bool IsMouseOverUi
    {
        get
        {
            // [Only works well while there is not PhysicsRaycaster on the Camera)
            //EventSystem eventSystem = EventSystem.current;
            //return (eventSystem != null && eventSystem.IsPointerOverGameObject());
 
            // [Works with PhysicsRaycaster on the Camera. Requires New Input System. Assumes mouse.)
            if (EventSystem.current == null)
            {
                return false;
            }
            RaycastResult lastRaycastResult = ((InputSystemUIInputModule)EventSystem.current.currentInputModule).GetLastRaycastResult(Mouse.current.deviceId);
            const int uiLayer = 5;
            return lastRaycastResult.gameObject != null && lastRaycastResult.gameObject.layer == uiLayer;
        }
    }
    
}






