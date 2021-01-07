using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the Bomber Bee when the player comes near it
/// </summary>
public class BeeActivator : MonoBehaviour
{
    public GameObject bee;

    BomberBeeAI bbai;

    // Start is called before the first frame update
    void Start()
    {
        bbai = bee.GetComponent<BomberBeeAI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bbai.ActivateBee(other.gameObject.transform.position);
        }
    }
}
