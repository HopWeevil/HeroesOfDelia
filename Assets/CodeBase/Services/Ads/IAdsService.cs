using System;

namespace CodeBase.Services.Ads
{
    public interface IAdsService
    {
        event Action RewardedVideoReady;
        event Action RewardedVideoFinished;
        int Reward { get; }
        void Initialize();

        void ShowRewardedVideo();
    }
}