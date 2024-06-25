using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public TMP_Text textBox;
    public TMP_Dropdown dropdown;
    public Button button;

    public TMP_Text _name;
    public string stor1;
    public string stor1Name;
    public string stor1Answer;
    public bool stor1Done;
    public string stor1Conclusion;
    
    public string stor2;
    public string stor2Name;
    public string stor2Answer;
    public bool stor2Done;
    public string stor2Conclusion;
    
    public bool storiesActive = false;
    public bool initiated = false;

    private GameManager GM;
    
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        dropdown.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
    }

    public void Answer()
    {
        string ans = dropdown.options[dropdown.value].text;
        if (ans == stor1Answer && !stor1Done)
        {
            stor1Done = true;
            dropdown.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
            textBox.text = stor1Conclusion + "\n+50 people helped";
            GM.AddImpact(50);
            StartCoroutine(Initiate());
        }
        else if (ans == stor2Answer && stor1Done)
        {
            stor2Done = true;
            dropdown.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
            textBox.text = stor2Conclusion+ "\n+50 people helped";
            GM.AddImpact(50);
        }
        else
        {
            StartCoroutine(TryAgain());
        }
    }
    

    void StoryOne()
    {
        dropdown.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        textBox.text = stor1;
        _name.text = stor1Name;
    }

    void StoryTwo()
    {
        dropdown.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        textBox.text = stor2;
        _name.text = stor2Name;
    }

    IEnumerator Initiate()
    {
        print("initialised");
        yield return new WaitForSeconds(0.2f + Random.Range(5, 30));
        if(!stor1Done)
            StoryOne();
        if (stor1Done)
        {
            StoryTwo();
        }
    }
    IEnumerator TryAgain()
    {
        string temp = textBox.text;
        textBox.text = "Thats a Good Service, but not the one thats best suited for this case!\nTry Again!";
        yield return new WaitForSeconds(2);
        textBox.text = temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initiated && storiesActive)
        {
            initiated = true;
            StartCoroutine(Initiate());
        }
    }
}
