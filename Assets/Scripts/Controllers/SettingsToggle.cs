using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;                  // for DoTween animation
using UnityEngine.UI;


/// <summary>
/// Toggles the Social Buttons
/// </summary>
public class SettingsToggle : MonoBehaviour
{
    public RectTransform btnFB, btnT, btnG, btnR;
    public float moveFB, moveT, moveG, moveR;
    public float defaultPosY, defaultPosX;
    public float speed;

    bool expanded;

    // Start is called before the first frame update
    void Start()
    {
        expanded = false;           // the buttons will be hidden when game begins so we set expanded = false
    }

    public void Toogle()
    {
        if (!expanded)
        {
            // show the buttons 
            btnFB.DOAnchorPosY(moveFB, speed, false);
            btnT.DOAnchorPosY(moveT, speed, false);
            btnG.DOAnchorPosY(moveG, speed, false);
            btnR.DOAnchorPosY(moveR, speed, false);
            expanded = true;
        }
        else
        {
            btnFB.DOAnchorPosY(defaultPosY, speed, false);
            btnT.DOAnchorPosY(defaultPosY, speed, false);
            btnG.DOAnchorPosY(defaultPosY, speed, false);
            btnR.DOAnchorPosY(defaultPosY, speed, false);
            expanded = false;
        }


    }
   
}
