using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerAudio
{
    [Header("Part 1")]
    public AudioClip playerJump;
    public AudioClip coinPickup;
    public AudioClip fireBullets;
    public AudioClip enemyExplosion;
    public AudioClip breakCrates;

    [Header("Part 2")]
    public AudioClip waterSplash;
    public AudioClip powerUp;
    public AudioClip keyFound;
    public AudioClip enemyHit;
    public AudioClip playerDied;
}
