using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCONEvent : MonoBehaviour
{
    [Header("PC UI ELEMENTS")]
    [SerializeField] private Animator monitor;
    [SerializeField] private Animator fader;
    [SerializeField] private GameObject controler;
    [SerializeField] private GameObject interButton;

    private bool screenOpen;
    
    private void Start()
    {
        screenOpen = false;
    }
    void Update()
    {
        string OpenScreen = ((Ink.Runtime.StringValue)DialogMannager.GetInstance().GetVarState("computer_on")).value;
        
        if (OpenScreen == "true" && !DialogMannager.GetInstance().playingDialog && !screenOpen)
        {
            fader.SetTrigger("OpenScreen");
            monitor.SetTrigger("OpenScreen");
            screenOpen = true;
            controler.SetActive(false);
            interButton.SetActive(false);
        }

        else if (OpenScreen == "false" && screenOpen) {
            fader.SetTrigger("CloseScreen");
            monitor.SetTrigger("CloseScreen");
            controler.SetActive(true);
            interButton.SetActive(true);
            screenOpen = false;
        }
    }
}
