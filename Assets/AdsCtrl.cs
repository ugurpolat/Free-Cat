using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using JetBrains.Annotations;
/// <summary>
/// Handles Ads in the game
/// </summary>
public class AdsCtrl : MonoBehaviour
{
    public static AdsCtrl instance = null;
    public string Android_Admob_Banner_ID;         // ca-app-pub-3940256099942544/6300978111
    public string Android_Admob_Interstitial_ID;   // ca-app-pub-3940256099942544/1033173712

    public bool testMode;
    public bool showBaner;                          // to toggle banner ads
    public bool showInterstitial;                   // to toggle interstitial ads
    BannerView bannerView;                          // the container for the banner ad
    InterstitialAd interstitial;
    AdRequest request;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        MobileAds.Initialize(initStatus => { });
        
        
        //this.RequestBanner();
    }

    public void RequestBanner()
    {
        
        // Create a 320x50 banner at the top of the screen
        if (testMode)
        {
            this.bannerView = new BannerView(Android_Admob_Banner_ID, AdSize.Banner, AdPosition.Top);
        }
        else
        {
            // code for live ad
        }

        //Create an empty ad request
        AdRequest adRequest = new AdRequest.Builder().Build();

        // Load the banner with the request
        this.bannerView.LoadAd(adRequest);

        HideBanner();

    }
    public void ShowBanner()
    {
        if (showBaner)
        {
            bannerView.Show();
        }
        
    }
    
    public void HideBanner()
    {
        if (showBaner)
        {
            bannerView.Hide();
        }
        
    }
    public void HideBanner(float duration)
    {
        StartCoroutine("HideBannerRoutine", duration);
    }

    IEnumerator HideBannerRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        bannerView.Hide();
    }

    void RequestInterstitial()
    {
        // Initialize an InterstitialAd
        if (testMode)
        {
            interstitial = new InterstitialAd(Android_Admob_Interstitial_ID);
        }
        else
        {
            // code for live ad
        }
        

        // Create an empty ad Request
        request = new AdRequest.Builder().Build();

        //Load the interstitial with the request
        interstitial.LoadAd(request);

        interstitial.OnAdClosed += HandleOnAdClosed;

    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        interstitial.Destroy();
        RequestInterstitial();
    }

    public void ShowInterstitial()
    {
        if (showInterstitial)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
        }
        
    }
    void OnEnable()
    {
        if (showBaner)
            RequestBanner();

        if (showInterstitial)
            RequestInterstitial();





    }
    void OnDisable()
    {
        if (showBaner)
            bannerView.Destroy();

        if (showInterstitial)
            interstitial.Destroy();
    }
}
