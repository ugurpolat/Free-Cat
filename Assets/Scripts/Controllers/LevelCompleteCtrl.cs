using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Handles the level Complte AI
/// </summary>
public class LevelCompleteCtrl : MonoBehaviour
{
    public Button btnNext;
    public Sprite goldenStar;
    public Image Star1;
    public Image Star2;
    public Image Star3;
    public Text txtScore;
    public int levelNumber;                         //Which level is this
    [HideInInspector]
    public int score;
    public int ScoreForThreeStars;
    public int ScoreForTwoStars;
    public int ScoreForOneStar;
    public int ScoreForNextLevel;
    public float animStartDelay;
    public float animDelay;

    bool showTwoStars, showThreeStars;

    // Start is called before the first frame update
    void Start()
    {
        //enable when deploying/beta testing
        score = GameCtrl.instance.GetScore();

        //update the score text
        txtScore.text = "" + score;

        //determine the number of stars to be awarded
        if (score >= ScoreForThreeStars)
        {
            showThreeStars = true;
            GameCtrl.instance.SetStarsAwarded(levelNumber, 3);
            Invoke("ShowGoldenStars", animStartDelay);
        }
        if (score >= ScoreForTwoStars && score < ScoreForThreeStars)
        {
            showTwoStars = true;
            GameCtrl.instance.SetStarsAwarded(levelNumber, 2);
            Invoke("ShowGoldenStars", animStartDelay);
        }
        if (score <= ScoreForOneStar && score != 0)
        {
            GameCtrl.instance.SetStarsAwarded(levelNumber, 1);
            Invoke("ShowGoldenStars", animStartDelay);
        }

    }
    void ShowGoldenStars()
    {
        StartCoroutine("HandleFirstStarAnim", Star1);
    }
    IEnumerator HandleFirstStarAnim(Image starImg)
    {
        DoAnim(starImg);

        // cause a delay before showing the next star
        yield return new WaitForSeconds(animDelay);

        if (showTwoStars || showThreeStars)
            StartCoroutine("HandleSecondStarAnim", Star2);
        else
            Invoke("CheckLevelStatus", 1.2f);

    }
    IEnumerator HandleSecondStarAnim(Image starImg)
    {
        DoAnim(starImg);

        // cause a delay before showing the next star
        yield return new WaitForSeconds(animDelay);

        showTwoStars = false;

        if (showThreeStars)
            StartCoroutine("HandleThirdStarAnim", Star3);
        else
            Invoke("CheckLevelStatus", 1.2f);


    }
    IEnumerator HandleThirdStarAnim(Image starImg)
    {
        DoAnim(starImg);

        // cause a delay before showing the next star
        yield return new WaitForSeconds(animDelay);

        showThreeStars = false;

        Invoke("CheckLevelStatus", 1.2f);
    }

    void CheckLevelStatus()
    {
        //----------------------unlock the next level if a certain score is reached----------------------
        if (score >= ScoreForNextLevel)
        {
            btnNext.interactable = true;

            //show a nice partickle effect
            SFXCtrl.instance.ShowBulletSparkle(btnNext.gameObject.transform.position);

            //play some audio
            AudioCtrl.instance.KeyFound(btnNext.gameObject.transform.position);

            //unlock the next level
            GameCtrl.instance.UnlockLevel(levelNumber);
        }
        else
        {
            btnNext.interactable = false;
        }
        //------------------------------------------------------------------------------------------------
    }
    void DoAnim(Image starImg)
    {

        //increase the star size
        starImg.rectTransform.sizeDelta = new Vector2(150f, 150f);

        //show the golden star
        starImg.sprite = goldenStar;

        //reduce the star size to normal using DoTween animation
        RectTransform t = starImg.rectTransform;
        t.DOSizeDelta(new Vector2(100f, 100f), 0.5f, false);

        //play an audio effect
        AudioCtrl.instance.KeyFound(starImg.gameObject.transform.position);

        //show a sparkle effect
        SFXCtrl.instance.ShowBulletSparkle(starImg.gameObject.transform.position);


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
