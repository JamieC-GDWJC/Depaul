using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ServiceGenerator : MonoBehaviour
{
    public float waitTime = 5f; // Time to wait in seconds
    public bool isAutomatic = false; // Toggle for automatic collection
    public int PeopleHelped;
    public int CostToRun;

    [Header("Audio")] 
    public List<AudioClipWithKey> clipList = new List<AudioClipWithKey>();
    
    [Header("Upgrades")] 
    public ServiceUpgrades upgrades;
    
    private Coroutine automaticCoroutine;
    private bool canCollect = false;

    private bool unlocked = false;

    private GameManager GM;
    private AudioManager AM;
    private UIController UI;
    private BuyAsset _buyAsset;
    private OutlineCommunication _outline;
    
    void Start()
    {
        GameObject manager = GameObject.FindWithTag("Manager"); 
        GM = manager.GetComponent<GameManager>();
        AM = GM.GetComponent<AudioManager>();
        UI = GetComponent<UIController>();
        _buyAsset = GetComponent<BuyAsset>();
        _outline = GetComponent<OutlineCommunication>();
        SetUpBuyUI();
    }

    void SetUpBuyUI()
    {
        UI.AddInfoField("Cooldown", waitTime + "s");
        UI.AddInfoField("People Helped",PeopleHelped.ToString());
        UI.AddInfoField("Cost","€" + CostToRun);
        
        
        UI.AddInfoField("Buy", "€" + _buyAsset.cost, false);
    }

    public void SetMode(bool isAutomatic)
    {
        this.isAutomatic = isAutomatic;

        if (isAutomatic)
        {
            if (automaticCoroutine == null)
            {
                automaticCoroutine = StartCoroutine(GenerateServiceAutomatically());
            }
        }
        else
        {
            if (automaticCoroutine != null)
            {
                StopCoroutine(automaticCoroutine);
                automaticCoroutine = null;
            }
            StartCoroutine(ManualServiceCooldown());
        }
    }

    public void Update()
    {
        if (!unlocked && _buyAsset.Unlocked)
        {
            unlocked = true;
            SetMode(isAutomatic);
            SetBuyButton();
        }

        if (unlocked)
        {
            CommunicateWithOutline();
        }

    }

    void CommunicateWithOutline()
    {
        if (canCollect && !_outline.needsInteraction)
        {
            _outline.needsInteraction = true;
        }
        else if (!canCollect && _outline.needsInteraction)
        {
            _outline.needsInteraction = false;
        }
    }

    IEnumerator GenerateServiceAutomatically()
    {
        while (true)
        {
            StartCoroutine(UI.FillSliderOverTime(waitTime));
            yield return new WaitForSeconds(waitTime); // Wait for specified seconds
            while (!LaunchService())
            {
                //wait until can afford
                yield return new WaitForSeconds(0.1f);
            }
            
        }
    }

    void OnMouseDown()
    {
        if (!isAutomatic && canCollect && !IsPointerOverUIElement())
        {
            if(LaunchService())
                StartCoroutine(ManualServiceCooldown());
        }
    }
    
    public static bool IsPointerOverUIElement()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Where(r => r.gameObject.layer == 5).Count() > 0;
    }

    IEnumerator ManualServiceCooldown()
    {
        canCollect = false;
        StartCoroutine(UI.FillSliderOverTime(waitTime));
        yield return new WaitForSeconds(waitTime); // Wait for specified seconds
        canCollect = true;
    }

    bool LaunchService()
    {
        if (GM.donationValue < CostToRun)
            return false;
        
        GM.SpendDono(CostToRun);
        GM.AddImpact(PeopleHelped); // Add Dono
        playAudio("HelpedPeople");
        return true;
    }

    // Method to toggle the automatic mode via script
    public void ToggleAutomaticMode(bool isAutomatic)
    {
        this.isAutomatic = isAutomatic;
        SetMode(isAutomatic);
    }
    
    //audio

    void playAudio(string key)
    {
        foreach (AudioClipWithKey audioClipWithKey in clipList)
        {
            if (audioClipWithKey.key == key)
            {
                AM.PlaySource(clip: audioClipWithKey.clip);
            }
        }
    }
    
    public void BuyUpgrade()
    {
        if (upgrades.upgradesInOrder.Count == 0 || upgrades.upgradesInOrder[0].cost > GM.donationValue)
        {
            print("failed to buy Upgrade");
            return;
        }
        
        GM.SpendDono(upgrades.upgradesInOrder[0].cost);        
        waitTime = upgrades.upgradesInOrder[0].waitTime;
        PeopleHelped = upgrades.upgradesInOrder[0].peopleHelped;
        CostToRun = upgrades.upgradesInOrder[0].costToRun;
        UI.ChangeField("Cooldown", waitTime + "s");
        UI.ChangeField("Income",  "€" + PeopleHelped);
        UI.ChangeField("Cost", "€" + CostToRun);

        upgrades.upgradesInOrder.RemoveAt(0);
        
        SetUpgradeButtonText();
        
    }

    void SetUpgradeButtonText()
    {
        if(upgrades.upgradesInOrder.Count == 0)
            UI.ChangeField("Buy", "Fully Upgraded");
        else
            UI.ChangeField("Buy", "€" + upgrades.upgradesInOrder[0].cost);
    }

    void SetBuyButton()
    {
        UI.Function = BuyUpgrade;
        SetUpgradeButtonText();
        UI.ResetButton();
    }
    
}