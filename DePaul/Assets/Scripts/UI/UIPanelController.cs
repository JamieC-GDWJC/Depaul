using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    private Animator _anim;

    private bool _active = false;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        Toggle();
    }

    public void Toggle()
    {
        if (_active)
        {
            _active = false;
            _anim.SetBool("SlideIn",true);
        }
        else
        {
            _active = true;
            _anim.SetBool("SlideIn",false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
