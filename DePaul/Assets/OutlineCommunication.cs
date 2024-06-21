using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineCommunication : MonoBehaviour
{
    public bool needsInteraction = true;
    public float speed = 10;
    private Outline _outline;
    private bool inrease;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (needsInteraction)
        {
            if(inrease)
                _outline.OutlineWidth += Time.deltaTime * speed;
            else
                _outline.OutlineWidth -= Time.deltaTime * speed;

            if (_outline.OutlineWidth <= 1)
                inrease = true;
            else if (_outline.OutlineWidth >= 10)
                inrease = false;
        }
        else if (_outline.OutlineWidth < 10)
        {
            _outline.OutlineWidth += Time.deltaTime * speed*speed;
        }
    }
}
