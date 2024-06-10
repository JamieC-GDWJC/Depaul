using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Done : MonoBehaviour
{
    public TMP_Text text;

    private ExternalCommunication EC;
    // Start is called before the first frame update
    void Start()
    {
        EC = GetComponent<ExternalCommunication>();
        text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (EC.active && !text.gameObject.activeSelf)
            text.gameObject.SetActive(true);
    }
}
