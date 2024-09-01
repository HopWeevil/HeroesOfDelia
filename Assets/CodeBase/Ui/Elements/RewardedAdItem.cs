using CodeBase.Services.Ads;
using CodeBase.Services.Notification;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RewardedAdItem : MonoBehaviour
{
    [SerializeField] private Button _showAdButton;
    [SerializeField] private TMP_Text _amount;
    [SerializeField] private Image _icon;

    private IAdsService _adsService;
    private IPersistentProgressService _progressService;
    private IStaticDataService _staticData;
    private IPopupMessageService _messageService;

    private ResourceRewardStaticData _data;

    [Inject]
    private void Construct(IAdsService adsService, IPersistentProgressService progressService, IStaticDataService staticData, IPopupMessageService messageService)
    {
        _adsService = adsService;
        _progressService = progressService;
        _staticData = staticData;
        _messageService = messageService;
    }

    public void SetItemData(ResourceRewardStaticData data)
    {
       _data = data;
    }

    public void SetInfo()
    {
        _amount.text = string.Format(_amount.text, _data.Amount);
        _icon.sprite = _staticData.ForResource(_data.ResourceType).Icon;
    }

    public void Start()
    {
        _showAdButton.onClick.AddListener(OnShowAdClicked);
        _adsService.RewardedVideoFinished += OnVideoFinished;
        _adsService.RewardedVideoFailed += OnVideoFailed;
    }

    public void OnDestroy()
    {
        _showAdButton.onClick.RemoveListener(OnShowAdClicked);
        _adsService.RewardedVideoFinished -= OnVideoFinished;
        _adsService.RewardedVideoFailed -= OnVideoFailed;
    }


    private void OnShowAdClicked()
    {
        _adsService.ShowRewardedVideo();
    }

    private void OnVideoFailed()
    {
        _messageService.ShowMessage("Failed to show video", Color.red);
    }

    private void OnVideoFinished()
    {
        _progressService.Economy.IncreaseResourceAmount(_data.ResourceType, _data.Amount);
    }
}