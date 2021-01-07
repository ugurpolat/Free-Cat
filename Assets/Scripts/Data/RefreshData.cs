using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates DataCtrl so that it provides the most recent data
/// </summary>
public class RefreshData : MonoBehaviour
{
    void Start()
    {
        DataCtrl.instance.RefreshData();    
    }
}
