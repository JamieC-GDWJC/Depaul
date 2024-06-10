using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAsset : MonoBehaviour
{
    public bool Unlocked = false;
    [SerializeField] private int cost;
    private GameManager GM;
    private AudioManager AM;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        AM = GM.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buy()
    {
        if (GM.donationValue >= cost)
        {
            Unlocked = true;
            GM.SpendDono(cost);
            AM.PlaySource();
        }
        else
        {
            
        }
    }
}
