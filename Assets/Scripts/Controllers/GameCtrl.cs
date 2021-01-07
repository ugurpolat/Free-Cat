using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using DG.Tweening;


public class GameCtrl : MonoBehaviour
{
    public static GameCtrl instance;
    public float restartDelay;
    [HideInInspector]
    public GameData data;                       // to work with game data in the inspector
    public UI ui;                               // for neatly arranging UI elements
    public GameObject bigCoin;                  // reward the cat gets on killing the enemy
    public GameObject player;
    public GameObject lever;
    public GameObject enemySpawner;
    public GameObject signPlatform;
    
    public int coinValue;                       // value of one small coin
    public int bigCoinValue;                    // value of one big coin
    public int enemyValue;                      // value of one enemy
    public float maxTime;                       // max time allowed to compelete the level
    float timeLeft;
    bool timerOn;
    bool isPaused;

    
    string dataFilePath;        // path to store the data file
    BinaryFormatter bf;         // help in saving/loading to binary files

    public enum Item
    {
        Coin,
        BigCoin,
        Enemy
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        bf = new BinaryFormatter();

        dataFilePath = Application.persistentDataPath + "/game.dat";

        //Debug.Log(dataFilePath);
    }
    // Start is called before the first frame update
    void Start()
    {
        DataCtrl.instance.RefreshData();
        data = DataCtrl.instance.data;
        RefreshUI();

        //LevelComplete();

        timeLeft = maxTime;
        timerOn = true;
        isPaused = false;
        HandleFirstBoot();

        UpdateHearts();
        ui.bossHealth.gameObject.SetActive(false);
    }
    void Update()
    {
        if (isPaused)
        {
            //set Time.timeScale = 0
            Time.timeScale = 0;

        }
        else
        {
            //set Time.timeScale = 1
            Time.timeScale = 1;
        }

        if (timeLeft > 0 && timerOn)
        {
            UpdateTimer();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeleteCheckPoints();
        }
    }
    
    void DeleteCheckPoints()
    {
        PlayerPrefs.DeleteKey("CPX");
        PlayerPrefs.DeleteKey("CPY");
    }

    public void RefreshUI()
    {
        ui.txtCoinCount.text = " X " + data.coinCount;
        ui.txtScore.text = "Score " + data.score;
    }
   
    /// <summary>
    /// Saves the stars awarded for a level
    /// </summary>
    /// <param name="levelNumber"></param>
    /// <param name="numOfStars"></param>
    public void SetStarsAwarded(int levelNumber,int numOfStars)
    {
        data.levelData[levelNumber].starsAwarded = numOfStars;

        //print star count in console for testing
        //Debug.Log("Number of Stars Awarded = " + data.levelData[levelNumber].starsAwarded);
    }
    /// <summary>
    /// Unlocks the specified level
    /// </summary>
    /// <param name="levelNumber"></param>
    public void UnlockLevel(int levelNumber)
    {
        data.levelData[levelNumber].isUnlocked = true;
    }

    /// <summary>
    /// Gets the current score for level complete Menu
    /// </summary>
    /// <returns>The Score</returns>
    public int GetScore()
    {
        return data.score;
    }
    void OnEnable()
    {
        //Debug.Log("Data Loaded");
        RefreshUI();
    }
    void OnDisable()
    {
       // Debug.Log("Data Saved");
        DataCtrl.instance.SaveData(data);
        Time.timeScale = 1;

        // Hide the AdMob banner
        AdsCtrl.instance.HideBanner();
    }   
    public void PlayerDied(GameObject player)
    {
        player.SetActive(false);
        CheckLives();

        //Invoke("RestartLevel", restartDelay);
    }
    public void PlayerDiedAnimation(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-150f,400f));

        player.transform.Rotate(new Vector3(0,0, 45f));

        player.GetComponent<PlayerCtrl>().enabled = false;
        //disable the colliders attached to the player so that the player can fall through the ground
        foreach (Collider2D c2d in player.transform.GetComponents<Collider2D>())
        {
            c2d.enabled = false;
        }
        //disable the child gameobjets attached to the player cat
        foreach (Transform child in player.transform)
        {
            child.gameObject.SetActive(false);
        }

        //disable the camera attached wih the player cat
        Camera.main.GetComponent<CameraCtrl>().enabled = false;

        rb.velocity = Vector2.zero;

        //restart level
        StartCoroutine("PauseBeforeReload",player);
    }
    public void PlayerStompsEnemy(GameObject enemy)
    {
        //change the enemy's tag
        enemy.tag = "Untagged";
        //destroy the enemy
        Destroy(enemy);
        //update the score
        UpdateScore(Item.BigCoin);
    }
    IEnumerator PauseBeforeReload(GameObject player)
    {
        yield return new WaitForSeconds(1.5f);
        PlayerDied(player);
    }
    public void PlayerDrowned(GameObject player)
    {
        //Debug.Log("Collision detected");

        //Invoke("RestartLevel", restartDelay);
        CheckLives();
    }
    public void UpdateCoinCount()
    {
        data.coinCount += 1;

        ui.txtCoinCount.text = " X " + data.coinCount.ToString();
        

    }
    public void BulletHitEnemy(Transform enemy)
    {
        
        //sfx
        Vector3 pos = enemy.position;
        pos.z = 20f;
        //hit sound
        AudioCtrl.instance.EnemyExplosion(pos);
        SFXCtrl.instance.EnemyExplosion(pos);
        //show big coin
        Instantiate(bigCoin, pos, Quaternion.identity);
        //destroy enemy
        Destroy(enemy.gameObject);
        //update score

    }
    public void UpdateScore(Item item)
    {
        int itemValue = 0;
        switch (item)
        {
            case Item.BigCoin:
                itemValue = bigCoinValue;
                break;
            case Item.Coin:
                itemValue = coinValue;
                break;
            case Item.Enemy:
                itemValue = enemyValue;
                break;
            default:
                break;

        }
        data.score += itemValue;
        ui.txtScore.text = "Score: " + data.score;
    }
    public void UpdateKeyCount(int keyNumber)
    {
        data.keyFound[keyNumber] = true;

        if (keyNumber == 0)
            ui.key0.sprite = ui.key0Full;
        else if (keyNumber == 1)
            ui.key1.sprite = ui.key1Full;
        else if (keyNumber == 2)
            ui.key2.sprite = ui.key2Full;

        if (data.keyFound[0] && data.keyFound[1])
            ShowSignPlatform();
    }
    void ShowSignPlatform()
    {
        signPlatform.SetActive(true);
        ui.bossHealth.gameObject.SetActive(true);
        SFXCtrl.instance.ShowPlayerLanding(signPlatform.transform.position);
        timerOn = false;
    }
    public void LevelComplete()
    {
        if (timerOn)
        {
            timerOn = false;
        }

        ui.panelMobileUI.SetActive(false);
        ui.levelCompleteMenu.SetActive(true);
    }
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void UpdateTimer()
    {
        timeLeft -= Time.deltaTime;

        ui.txtTimer.text = "Timer: " + (int)timeLeft;

        if (timeLeft <= 0)
        {
            ui.txtTimer.text = "Timer: 0";
            GameObject player = GameObject.FindGameObjectWithTag("Player") as GameObject;
            PlayerDied(player);
        }
    }
    private void HandleFirstBoot()
    {
        if (data.isFirstBoot)
        {
            //set lives to 3
            data.lives = 3;
            //set number of coins to 0
            data.coinCount = 0;
            //set keys collected to 0
            data.keyFound[0] = false;
            data.keyFound[1] = false;
            data.keyFound[2] = false;
            //set score to 0
            data.score = 0;
            //set isFirstBoot to false
            data.isFirstBoot = false;
        }

    }
    private void UpdateHearts()
    {
        if (data.lives == 3)
        {
            ui.heart1.sprite = ui.fullHeart;
            ui.heart2.sprite = ui.fullHeart;
            ui.heart3.sprite = ui.fullHeart;
        }
        if (data.lives == 2)
        {
            ui.heart1.sprite = ui.emptyHeart;
        }
        if (data.lives == 1)
        {
            ui.heart1.sprite = ui.emptyHeart;
            ui.heart2.sprite = ui.emptyHeart;
        }
        if (data.lives == 0)
        {
            ui.heart1.sprite = ui.emptyHeart;
            ui.heart2.sprite = ui.emptyHeart;
            ui.heart3.sprite = ui.emptyHeart;
        }
    }
    void CheckLives()
    {
        int updatedLives = data.lives;
        updatedLives -= 1;
        data.lives = updatedLives;

        if (data.lives == 0)
        {
            data.lives = 3;
            DataCtrl.instance.SaveData(data);
            Invoke("GameOver", restartDelay);
        }
        else
        {
            DataCtrl.instance.SaveData(data);
            Invoke("RestartLevel", restartDelay);
        }
    }
    public void StopCameraFollow()
    {
        Camera.main.GetComponent<CameraCtrl>().enabled = false;
        player.GetComponent<PlayerCtrl>().isStuck = true;                   //stop parallax
        player.transform.Find("Left_Check").gameObject.SetActive(false);
        player.transform.Find("Right_Check").gameObject.SetActive(false);

    }
    public void ShowLever()
    {
        lever.SetActive(true);
        DeactivateEnemySpawner();
        SFXCtrl.instance.ShowPlayerLanding(lever.gameObject.transform.position);
        AudioCtrl.instance.EnemyExplosion(lever.gameObject.transform.position);
    }
    public void ActivateEnemySpawner()
    {
        enemySpawner.SetActive(true);
    }
    public void DeactivateEnemySpawner()
    {
        enemySpawner.SetActive(false);
    }
    
    void GameOver()
    {
        //ui.panelGameOver.SetActive(true);
        if (timerOn)
            timerOn = false;
        // show Game Over menu with sliding animation
        ui.panelGameOver.gameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 0.7f, false);

        //
        //AdsCtrl.instance.ShowBanner();

        DeleteCheckPoints();
    }

    /// <summary>
    /// Shows the pause panel
    /// </summary>
    public void ShowPausePanel()
    {
        if (ui.panelMobileUI.activeInHierarchy)
            ui.panelMobileUI.SetActive(false);

        
        ui.panelPause.SetActive(true);
        
        // animate the pause panel
        ui.panelPause.gameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 0.7f, false);

        // Show the admob banner
        AdsCtrl.instance.ShowBanner();

        // show an interstitial Ad
        AdsCtrl.instance.ShowInterstitial();

        Invoke("SetPause", 1.1f);
    }

    void SetPause()
    {
        // set the bool
        isPaused = true;
    }

    /// <summary>
    /// Hides the pause panel
    /// </summary>
    public void HidePausePanel()
    {
        isPaused = false;

        if (!ui.panelMobileUI.activeInHierarchy)
            ui.panelMobileUI.SetActive(true);


        //ui.panelPause.SetActive(false);

        // animate the pause panel
        ui.panelPause.gameObject.GetComponent<RectTransform>().DOAnchorPosY(600f, 0.7f, false);

        //Hide the AdMob banner
        AdsCtrl.instance.HideBanner();
         
    }

}
