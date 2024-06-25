using UnityEngine;

public class BuyAsset : MonoBehaviour
{
    public bool Unlocked = false;
    public int cost;
    private GameManager GM;
    private AudioManager AM;
    private ExternalCommunication _externalCommunication;

    [SerializeField] private AudioClip boughtAudioClip;
    [SerializeField] private AudioClip errorAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        AM = GM.GetComponent<AudioManager>();
        _externalCommunication = GetComponent<ExternalCommunication>();
    }

    public void Buy()
    {
        if (!Unlocked && GM.donationValue >= cost)
        {
            Unlocked = true;
            _externalCommunication.active = true;
            GM.SpendDono(cost);
            AM.PlaySource(clip: boughtAudioClip);
        }
        else
        {
            AM.PlaySource(clip: errorAudioClip);
        }
    }

}
