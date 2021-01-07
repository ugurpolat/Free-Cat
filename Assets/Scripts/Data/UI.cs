using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class UI
{
    [Header("Text")]
    public Text txtCoinCount;
    public Text txtScore;
    public Text txtTimer;

    [Header("Images/Sprites")]
    public Image key0;
    public Image key1;
    public Image key2;
    public Sprite key0Full;
    public Sprite key1Full;
    public Sprite key2Full;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Slider bossHealth;

    [Header("Popup Menus/Panels")]
    public GameObject panelGameOver;
    public GameObject levelCompleteMenu;
    public GameObject panelMobileUI;
    public GameObject panelPause;
}
