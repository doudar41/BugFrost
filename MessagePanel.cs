using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePanel : MonoBehaviour
{
    public Button Yes, No;
    [SerializeField]
    private TextMeshProUGUI MessageText;

    public GameObject Panel;
    private bool switchOnOff;


    public void ActivateTextPanel(bool switchPanel )
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ClickButtons");
        Panel.SetActive(switchPanel);
        if(Yes.gameObject.activeSelf )
        {
            Yes.gameObject.SetActive(false);
            No.gameObject.SetActive(false);
        }
    }

    public void PrintTextToPanel(string message, bool switchButtons)
    {
        MessageText.text = message;
        if (switchButtons)
        {
            Yes.gameObject.SetActive(true);
            No.gameObject.SetActive(true);
        }
    }



}
