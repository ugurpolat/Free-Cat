using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignCtrl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GameCtrl.instance.StopCameraFollow();

            GameCtrl.instance.ActivateEnemySpawner();
        }
    }
}
