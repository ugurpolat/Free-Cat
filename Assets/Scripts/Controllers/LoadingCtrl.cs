using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows the loading screen
/// </summary>
public class LoadingCtrl : MonoBehaviour
{
    public GameObject panelLoading;                 // shown when the level is loading
    public static LoadingCtrl instance;             //the static instance of this class

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }    
    }

    public void ShowLoading()
    {
        panelLoading.SetActive(true);
    }
}
