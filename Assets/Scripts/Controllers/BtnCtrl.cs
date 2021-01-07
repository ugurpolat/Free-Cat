using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnCtrl : MonoBehaviour
{
    int levelNumber;                                // the level to check
    Button btn;                                     // the button to which this script is attached to
    Image btnImg;                                   // the image of this button
    Text btnText;                                   // the text element of this button
    Transform star1, star2, star3;                  // the 3 stars which are shown with the button

    public Sprite lockedBtn;                        // sprite shown when button is locked
    public Sprite unlockedBtn;                      // sprite shown when button is unlocked
    public string sceneName;


    // Start is called before the first frame update
    void Start()
    {
        //buttons are name according to numbers which represent level numbers
        levelNumber = int.Parse(transform.gameObject.name);

        btn = transform.gameObject.GetComponent<Button>();
        btnImg = btn.GetComponent<Image>();
        btnText = btn.gameObject.transform.GetChild(0).GetComponent<Text>();

        //getting references to the stars attached to the button
        star1 = btn.gameObject.transform.GetChild(1);
        star2 = btn.gameObject.transform.GetChild(2);
        star3 = btn.gameObject.transform.GetChild(3);

        BtnStatus();
    }

    /// <summary>
    /// Locks or unlocks a certain button.Also shows number of stars awarded
    /// </summary>
    void BtnStatus()
    {
        bool unlocked = DataCtrl.instance.isUnlocked(levelNumber);
        int starsAwarded = DataCtrl.instance.getStars(levelNumber);

        if (unlocked)
        {
            // show appropriate number of stars
            if (starsAwarded == 3)
            {
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(true);
                star3.gameObject.SetActive(true);
            }
            if (starsAwarded == 2)
            {
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(true);
                star3.gameObject.SetActive(false);
            }
            if (starsAwarded == 1)
            {
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(false);
                star3.gameObject.SetActive(false);
            }
            if (starsAwarded == 0)
            {
                star1.gameObject.SetActive(false);
                star2.gameObject.SetActive(false);
                star3.gameObject.SetActive(false);
            }
            btn.onClick.AddListener(LoadScene);
        }
        else
        {
            // show the locked button image
            btnImg.overrideSprite = lockedBtn;

            // don't show any text on the button
            btnText.text = "";
            //hide the 3 star
            star1.gameObject.SetActive(false);
            star2.gameObject.SetActive(false);
            star3.gameObject.SetActive(false);
        }

    }

    void LoadScene()
    {
        LoadingCtrl.instance.ShowLoading();

        SceneManager.LoadScene(sceneName);
    }
}
