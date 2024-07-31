using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class IncomeGenerator : MonoBehaviour
{
    public float waitTime = 5f; // Time to wait in seconds
    public bool isAutomatic = false; // Toggle for automatic collection
    public int collectionAmount;

    [Header("Audio")] 
    public List<AudioClipWithKey> clipList = new List<AudioClipWithKey>();

    [Header("Upgrades \nadd duplicate of the last one if you want to assign manager")] 
    public IncomeUpgrades upgrades = new IncomeUpgrades();
    
    private Coroutine automaticCoroutine;
    private bool canCollect = false;

    private bool unlocked = false;

    private GameManager GM;
    private AudioManager AM;
    private UIController UI;
    private BuyAsset _buyAsset;
    private OutlineCommunication _outline;
    private ExternalCommunication _externalCommunication;
    void Start()
    {
        GameObject manager = GameObject.FindWithTag("Manager"); 
        GM = manager.GetComponent<GameManager>();
        AM = GM.GetComponent<AudioManager>();
        UI = GetComponent<UIController>();
        _buyAsset = GetComponent<BuyAsset>();
        _outline = GetComponent<OutlineCommunication>();
        _externalCommunication = GetComponent<ExternalCommunication>();
        SetUpBuyUI();
    }

    void SetUpBuyUI()
    {
        UI.AddInfoField("Cooldown", waitTime + "s");
        UI.AddInfoField("Income",  "€" + collectionAmount);
        
        UI.AddInfoField("Buy", "€" + _buyAsset.cost, false);
    }

    public void SetMode(bool isAutomatic)
    {
        this.isAutomatic = isAutomatic;

        if (isAutomatic)
        {
            if (automaticCoroutine == null)
            {
                automaticCoroutine = StartCoroutine(GenerateIncomeAutomatically());
            }
        }
        else
        {
            if (automaticCoroutine != null)
            {
                StopCoroutine(automaticCoroutine);
                automaticCoroutine = null;
            }
            StartCoroutine(ManualIncomeCooldown());
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

    IEnumerator GenerateIncomeAutomatically()
    {
        while (true)
        {
            StartCoroutine(UI.FillSliderOverTime(waitTime));
            yield return new WaitForSeconds(waitTime); // Wait for specified seconds
            CollectMoney();
        }
    }

    void OnMouseDown()
    {
        if (!isAutomatic && canCollect && !IsPointerOverUIElement())
        {
            CollectMoney();
            StartCoroutine(ManualIncomeCooldown());
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

    IEnumerator ManualIncomeCooldown()
    {
        canCollect = false;
        StartCoroutine(UI.FillSliderOverTime(waitTime));
        yield return new WaitForSeconds(waitTime); // Wait for specified seconds
        canCollect = true;
    }

    void CollectMoney()
    {
        GM.AddDono(collectionAmount); // Add Dono
        playAudio("CollectMoney");
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
            playAudio("Error");
            return;
        }
        
        
        playAudio("Upgrade");
        GM.SpendDono(upgrades.upgradesInOrder[0].cost);
        
        
        waitTime = upgrades.upgradesInOrder[0].waitTime;
        collectionAmount = upgrades.upgradesInOrder[0].collectionAmount;
        UI.ChangeField("Cooldown", waitTime + "s");
        UI.ChangeField("Income",  "€" + collectionAmount);

        upgrades.upgradesInOrder.RemoveAt(0);
        
        if (upgrades.upgradesInOrder.Count == 0)
        {
            ToggleAutomaticMode(true);
        }
        
        SetUpgradeButtonText();
        
    }

    void SetUpgradeButtonText()
    {
        if(upgrades.upgradesInOrder.Count == 0)
            UI.ChangeField("Buy", "Fully Upgraded");
        else  if(upgrades.upgradesInOrder.Count == 1 && !isAutomatic)
            UI.ChangeField("Buy", "Assign Manager\n€" + upgrades.upgradesInOrder[0].cost);
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