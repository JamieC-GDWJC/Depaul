using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int donationValue = 0;
    public string donationPreText = "";
    
    public int impactValue = 0;
    public string impactPreText = "";
    
    [SerializeField] private TMP_Text donationTextBox;
    [SerializeField] private TMP_Text impactTextBox;

    [SerializeField] private GameObject Meeple;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        donationTextBox.text = donationPreText + " " + donationValue.ToString();
        impactTextBox.text = impactPreText + " " + impactValue.ToString();
        
    }

    public void AddDono(int value)
    {
        donationValue += value;
    }
    
    public void SpendDono(int value)
    {
        donationValue -= value;
    }

    public void AddImpact(int value)
    {
        impactValue += value;
        for (int i = 0; i <= value; i++)
        {
            Instantiate(Meeple);
        }
    }
}
