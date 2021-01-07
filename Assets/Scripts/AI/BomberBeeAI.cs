using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// The AI Engine of the Bomber Bee
/// </summary>
public class BomberBeeAI : MonoBehaviour
{
    public float destroyBeeDelay;
    public float beeSpeed;

    public void ActivateBee(Vector3 playerPos)
    {
        transform.DOMove(playerPos, beeSpeed,false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Player"))
        {
            //Show explosion
            SFXCtrl.instance.EnemyExplosion(other.gameObject.transform.position);
            //destroy bee
            Destroy(gameObject, destroyBeeDelay);
        }
    }


}
