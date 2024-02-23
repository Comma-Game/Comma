using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System; // 필요

// ca-app-pub-3940256099942544/5224354917
public class GoogleAdMob : MonoBehaviour
{
 #if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3078473154541994/3359689681";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3078473154541994/3359689681";
#else
  private string _adUnitId = "unused";
#endif

    [SerializeField] private GameObject ADPanel;
    [SerializeField] private GameObject ADPanel2;
    private RewardedInterstitialAd _rewardedInterstitialAd;
    private int testCase = 0;
    private int getCoin = 0;
    private static GoogleAdMob instance;
    // singleton
    public static GoogleAdMob Instance
    {
        get
        {
            // 인스턴스가 없으면 생성
            if (instance == null)
            {
                instance = FindObjectOfType<GoogleAdMob>();

                // 만약 Scene에 GameManager가 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject("GoogleAdMob");
                    instance = obj.AddComponent<GoogleAdMob>();
                }
            }
            return instance;
        }
    }

        public void LoadAd(int testcase, int coin)
        {
            testCase = testcase;
            getCoin = coin;
            ADPanel.SetActive(true);
            if(ADPanel2 != null) ADPanel2.SetActive(true);
            // Clean up the old ad before loading a new one.
            if (_rewardedInterstitialAd != null)
            {
                DestroyAd();
            }

            Debug.Log("Loading rewarded interstitial ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // Send the request to load the ad.
            RewardedInterstitialAd.Load(_adUnitId, adRequest,
                (RewardedInterstitialAd ad, LoadAdError error) =>
                {
                    // If the operation failed with a reason.
                    if (error != null)
                    {
                        Debug.LogError("Rewarded interstitial ad failed to load an ad with error : "
                                        + error);
                        return;
                    }
                    // If the operation failed for unknown reasons.
                    // This is an unexpexted error, please report this bug if it happens.
                    if (ad == null)
                    {
                        Debug.LogError("Unexpected error: Rewarded interstitial load event fired with null ad and null error.");
                        return;
                    }

                    // The operation completed successfully.
                    Debug.Log("Rewarded interstitial ad loaded with response : "
                        + ad.GetResponseInfo());
                    _rewardedInterstitialAd = ad;

                    // Register to ad events to extend functionality.
                    RegisterEventHandlers(ad);
                    ShowAd();
                });
        }

        /// <summary>
        /// Shows the ad.
        /// </summary>
        public void ShowAd()
        {
            if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
            {
                _rewardedInterstitialAd.Show((Reward reward) =>
                {
                    Debug.Log("Rewarded interstitial ad rewarded : " + reward.Amount);
                    if(testCase == 0){ // 하트 풀 충전
                        HeartPanel.Instance.AddFullHearts();
                        //HeartPanel.Instance.AddHearts(1);
                        SaveLoadManager.Instance.WatchAds(); //광고 1회 시청
                    }else if(testCase == 1){ // 코인 2배
                        SceneManager.LoadScene("MainScene");
                        SaveLoadManager.Instance.PlusCoin(getCoin*2);
                        SaveLoadManager.Instance.SaveData();
                    }
                    ADPanel.SetActive(false);
                    if(ADPanel2 != null) ADPanel2.SetActive(false);
                });
            }
            else
            {
                Debug.LogError("Rewarded interstitial ad is not ready yet.");
            }
        }

        /// <summary>
        /// Destroys the ad.
        /// </summary>
        public void DestroyAd()
        {
            if (_rewardedInterstitialAd != null)
            {
                Debug.Log("Destroying rewarded interstitial ad.");
                _rewardedInterstitialAd.Destroy();
                _rewardedInterstitialAd = null;
            }
        }

        /// <summary>
        /// Logs the ResponseInfo.
        /// </summary>
        public void LogResponseInfo()
        {
            if (_rewardedInterstitialAd != null)
            {
                var responseInfo = _rewardedInterstitialAd.GetResponseInfo();
                UnityEngine.Debug.Log(responseInfo);
            }
        }

        protected void RegisterEventHandlers(RewardedInterstitialAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded interstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded interstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded interstitial ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                ADPanel.SetActive(false);
                if(ADPanel2 != null) ADPanel2.SetActive(false);
                Debug.Log("Rewarded interstitial ad full screen content closed.");
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                ADPanel.SetActive(false);
                if(ADPanel2 != null) ADPanel2.SetActive(false);
                Debug.LogError("Rewarded interstitial ad failed to open full screen content" +
                               " with error : " + error);
            };
        }
}

    //private RewardedAd rewardedAd;

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    // public void LoadRewardedAd(bool is2coin, int getCoin)
    // {
    //     is2Coin = is2coin;
    //     // Clean up the old ad before loading a new one.
    //     if (rewardedAd != null)
    //     {
    //         rewardedAd.Destroy();
    //         rewardedAd = null;
    //     }

    //     Debug.Log("Loading the rewarded ad.");

    //     // create our request used to load the ad.
    //     var adRequest = new AdRequest();

    //     // send the request to load the ad.
    //     RewardedAd.Load(_adUnitId, adRequest,
    //         (RewardedAd ad, LoadAdError error) =>
    //         {
    //             // if error is not null, the load request failed.
    //             if (error != null || ad == null)
    //             {
    //                 Debug.LogError("Rewarded ad failed to load an ad " +
    //                                "with error : " + error);
    //                 return;
    //             }

    //             Debug.Log("Rewarded ad loaded with response : "
    //                       + ad.GetResponseInfo());

    //             rewardedAd = ad;

    //             RegisterEventHandlers(rewardedAd);

    //             ShowRewardedAd();
    //         });
    // }

    // public void ShowRewardedAd()
    // {
    //     const string rewardMsg =
    //         "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

    //     if (rewardedAd != null && rewardedAd.CanShowAd())
    //     {
    //         rewardedAd.Show((Reward reward) =>
    //         {
    //             // TODO: Reward the user.
    //             Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
    //         });
    //     }
    // }

    // private void RegisterEventHandlers(RewardedAd ad)
    // {
    //     // Raised when the ad is estimated to have earned money.
    //     ad.OnAdPaid += (AdValue adValue) =>
    //     {
    //         Debug.Log(string.Format("Rewarded ad paid {0} {1}.",
    //             adValue.Value,
    //             adValue.CurrencyCode));
    //     };
    //     // Raised when an impression is recorded for an ad.
    //     ad.OnAdImpressionRecorded += () =>
    //     {
    //         Debug.Log("Rewarded ad recorded an impression.");
    //     };
    //     // Raised when a click is recorded for an ad.
    //     ad.OnAdClicked += () =>
    //     {
    //         Debug.Log("Rewarded ad was clicked.");
    //     };
    //     // Raised when an ad opened full screen content.
    //     ad.OnAdFullScreenContentOpened += () =>
    //     {
    //         Debug.Log("Rewarded ad full screen content opened.");
    //     };
    //     // Raised when the ad closed full screen content.
    //     ad.OnAdFullScreenContentClosed += () =>
    //     {
    //         Debug.Log("Rewarded ad full screen content closed.");
    //         //LoadRewardedAd();
    //         if(is2Coin){
    //             SceneManager.LoadScene("MainScene");
    //             SaveLoadManager.Instance.PlusCoin(getCoin);
    //             SaveLoadManager.Instance.SaveData();
    //         }
    //         is2Coin = false;
    //     };
    //     // Raised when the ad failed to open full screen content.
    //     ad.OnAdFullScreenContentFailed += (AdError error) =>
    //     {
    //         Debug.LogError("Rewarded ad failed to open full screen content " +
    //                        "with error : " + error);
    //         //LoadRewardedAd();
    //     };
    // }
