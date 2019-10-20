using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using TMPro;
[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{

#if UNITY_ANDROID
    private string gameId = "3332721";
#endif

    Button myButton;
    public string myPlacementId = "rewardedVideo";
    [SerializeField] GemsAndGoldSystem gemsAndGoldSystem;
    [SerializeField] GameObject gemsRewardHolder;
    [SerializeField] MessagePopup messagePopup;
    [SerializeField] TextMeshProUGUI adsRemainingText;
    [SerializeField] Player player;
    [SerializeField] TimeManager timeManager;
    [SerializeField] int adsPerDay = 3;
    
    int adsRemainingForToday = 3;

    public int AdsRemainingForToday { get => adsRemainingForToday; set => adsRemainingForToday = value; }
    public int AdsPerDay { get => adsPerDay;}

    void Start()
    {
        myButton = GetComponent<Button>();

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);       
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        AdsRemainingForToday = (timeManager.IsDailyAdsRefreshable()) ? AdsPerDay : AdsRemainingForToday;
        if(AdsRemainingForToday > 0)
        {
            Advertisement.Show(myPlacementId);
        }
        else
        {            
            messagePopup.ReceiveStringAndShowPopup("You watched all your ads for today. \n Thank you.");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            AdsRemainingForToday--;           
            timeManager.SaveCompletionDate();
            adsRemainingText.text = (AdsRemainingForToday + " ads remaining today.");
            gemsRewardHolder.SetActive(true);
            gemsAndGoldSystem.AddGems(20);
            player.SavePlayer();

        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            messagePopup.ReceiveStringAndShowPopup("An error occured while loading the ads. \n Please verify connection");

        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}