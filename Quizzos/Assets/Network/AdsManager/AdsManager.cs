using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{

    string gameId = "3332721";
    string myPlacementId = "rewardedVideo";
    bool testMode = true;
    [SerializeField] GemsAndGoldSystem gemsAndGoldSystem;
    
    void Start()
    {
        //Advertisement.AddListener(this);
        //Advertisement.Initialize(gameId, testMode);       
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            Advertisement.Show(myPlacementId);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }
}

