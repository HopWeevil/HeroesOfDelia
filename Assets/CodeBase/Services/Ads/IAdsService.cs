using System;

namespace CodeBase.Services.Ads
{
    public interface IAdsService
    {
        event Action RewardedVideoReady;
        event Action RewardedVideoFinished;
        event Action RewardedVideoFailed;

        void Initialize();

        void ShowRewardedVideo();
    }
}