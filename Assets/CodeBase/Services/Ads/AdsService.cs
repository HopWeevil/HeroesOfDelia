using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const string AndroidGameId = "5651057";
        private const string IOSGameId = "5651056";

        private const string UnityRewardedVideoIdAndroid = "Rewarded_Android";
        private const string UnityRewardedVideoIdIOS = "Rewarded_iOS";

        private string _gameId;
        private string _placementId;

        public event Action RewardedVideoReady;
        public event Action RewardedVideoFinished;
        public event Action RewardedVideoFailed;

        public void Initialize()
        {
            SetIdsForCurrentPlatform();
            Advertisement.Initialize(_gameId, testMode: true, this);
        }

        public void ShowRewardedVideo()
        {
            if (Advertisement.isSupported && !Advertisement.isShowing)
            {
                Advertisement.Show(_placementId, this);
            }
            else
            {
                Debug.LogWarning("Rewarded video is either not supported or already showing.");
            }
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"Ad Loaded: {placementId}");
            if (placementId == _placementId)
            {
                RewardedVideoReady?.Invoke();
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"Failed to load ad {placementId}: {error} - {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"Failed to show ad {placementId}: {error} - {message}");
            RewardedVideoFailed?.Invoke();
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"Ad Started: {placementId}");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"Ad Clicked: {placementId}");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId == _placementId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                RewardedVideoFinished?.Invoke();
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Ads initialization failed: {error} - {message}");
        }

        private void SetIdsForCurrentPlatform()
        {
            _gameId = Application.platform switch
            {
                RuntimePlatform.Android => AndroidGameId,
                RuntimePlatform.IPhonePlayer or RuntimePlatform.WindowsEditor => IOSGameId,
                _ => throw new PlatformNotSupportedException("Unsupported platform for ads.")
            };

            _placementId = Application.platform switch
            {
                RuntimePlatform.Android => UnityRewardedVideoIdAndroid,
                RuntimePlatform.IPhonePlayer or RuntimePlatform.WindowsEditor => UnityRewardedVideoIdIOS,
                _ => throw new PlatformNotSupportedException("Unsupported platform for ads.")
            };
        }
    }
}
