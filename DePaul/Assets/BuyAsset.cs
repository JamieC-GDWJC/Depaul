using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAsset : MonoBehaviour
{
    public bool Unlocked = false;
    public int cost;
    private GameManager GM;
    private AudioManager AM;

    [SerializeField] private AudioClip boughtAudioClip;
    [SerializeField] private AudioClip errorAudioClip;

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
        if (!Unlocked && GM.donationValue >= cost)
        {
            Unlocked = true;
            GM.SpendDono(cost);
            AM.PlaySource(clip: boughtAudioClip);
        }
        else
        {
            AM.PlaySource(clip: errorAudioClip);
        }
    }

}
