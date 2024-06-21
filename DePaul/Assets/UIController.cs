using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    
    public delegate void DelegateVoid();
    public DelegateVoid Function;

    private GameObject Bubble;
    private List<GameObject> InfoModules = new List<GameObject>();
    private Transform infoLocation;
    private Button buyLocation;
    private Transform nameLocation;
    private Slider progressSlider;
    
    private InfoModuleTypes moduleTypes;
    VerticalLayoutGroup contentLayoutGroup;

    private BuyAsset _buyAsset;

    private Camera mainCamera;

    private bool isShowing;
    // Start is called before the first frame update
    void Awake()
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
        Bubble.name = gameObject.name;
        
        infoLocation = Bubble.transform.Find("BubbleUI/Content/Info");
        
        buyLocation = Bubble.transform.Find("BubbleUI/Content/Buy").GetComponent<Button>();
        Function = _buyAsset.Buy;
        ResetButton();
        
        nameLocation = Bubble.transform.Find("BubbleUI/Content/Name");
        nameLocation.GetComponent<TMP_Text>().text = gameObject.name;
        
        contentLayoutGroup = Bubble.transform.Find("BubbleUI/Content").GetComponent<VerticalLayoutGroup>();
        
        progressSlider = Bubble.transform.Find("BubbleUI/Progress/Slider").GetComponent<Slider>();
    }
    
    public void ResetButton()
    {
        if (Function == null)
            return;
        
        buyLocation.onClick.RemoveAllListeners();
        buyLocation.onClick.AddListener(delegate { Function(); });
        
    }

    public void AddInfoField(string field, string content, bool isInfo = true)
    {
        Sprite icon = GetIcon(field);
        GameObject module;
        
        if(isInfo)
            module = Instantiate(InfoModule, infoLocation);
        else
        {
            module = Instantiate(InfoModule, buyLocation.transform);
            module.transform.localPosition = new Vector3(0,0,0);
        }
        
        module.name = field;
        module.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = icon;
        module.transform.Find("Text").GetComponent<TMP_Text>().text = content;
        InfoModules.Add(module);

        StartCoroutine(UpdateLayoutGroup());
    }

    public void ChangeField(string field, string content)
    {
        foreach (var module in InfoModules)
        {
            if (module.name == field)
            {
                module.transform.Find("Text").GetComponent<TMP_Text>().text = content;
            }
        }
        //StartCoroutine(UpdateLayoutGroup());
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

        timer += Time.deltaTime;
        if (timer >= cooldownTime)
        {
            //print("checked if mouse is over UI");
            if(!IsMouseOverUi)
                ShowUI(false);
            else
                timer = 0;
        }
    }

    public void ShowUI(bool show)
    {
        timer = 0;
        isShowing = show;
        //print("show UI: " + show);
        Bubble.SetActive(show);
    }
    

    public bool IsMouseOverUi
    {
        get
        {
            if (EventSystem.current == null)
            {
                return false;
            }
            RaycastResult lastRaycastResult = ((InputSystemUIInputModule)EventSystem.current.currentInputModule).GetLastRaycastResult(Mouse.current.deviceId);
            const int uiLayer = 5;


            if (lastRaycastResult.gameObject != null)
            {
                Transform parent = lastRaycastResult.gameObject.transform;
                int roof = 10;
                while (true)
                { 
                    if (parent.gameObject == Bubble)
                    {
                        return true;
                    }
                    else
                    {
                        parent = parent.parent;
                        roof--;
                    }

                    if (roof == 0 || parent == null)
                    {
                        return false;
                    }
                }
            }

            return lastRaycastResult.gameObject != null && lastRaycastResult.gameObject.layer == uiLayer;
        }
    }
    
    private void OnMouseEnter()
    {
        ShowUI(true);
    }

    private void OnMouseOver()
    {
        timer = 0;
    }

    public IEnumerator FillSliderOverTime(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            progressSlider.value = Mathf.Clamp01(elapsedTime / time);
            yield return null;
        }
        progressSlider.value = 1f; // Ensure the slider is set to 1 at the end
    }
}






