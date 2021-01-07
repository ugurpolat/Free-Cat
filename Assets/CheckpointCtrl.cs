using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves the current position of the cat in PlayerPrefs
/// </summary>
public class CheckpointCtrl : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("CPX", other.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("CPY", other.gameObject.transform.position.y);
        }
    }
}
