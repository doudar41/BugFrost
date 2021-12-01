using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFMODEvent : MonoBehaviour
{
    [SerializeField]
    public string sceneName;

    [FMODUnity.BankRef]
    public List<string> banks;


    private void Awake()
    {
        foreach (string b in banks)
        {
            FMODUnity.RuntimeManager.LoadBank(b, true);
            Debug.Log("Loaded bank " + b);
        }
        /*
            For Chrome / Safari browsers / WebGL.  Reset audio on response to user interaction (LoadBanks is called from a button press), to allow audio to be heard.
        */
        FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
        FMODUnity.RuntimeManager.CoreSystem.mixerResume();

        StartCoroutine(CheckBanksLoaded());
    }

    public void LoadBanks()
    {

    }

    IEnumerator CheckBanksLoaded()
    {
        while (!FMODUnity.RuntimeManager.HasBanksLoaded )
        {
            yield return null;
        }


    }

}
