using CodeBase.Enums;
using CodeBase.Services.Notification;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class EquipmentItemWindow : WindowBase
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _rarity;
        [SerializeField] private TMP_Text _bonusesText;

        [SerializeField] private Image _equipmentIcon;
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _unEquipButton;

        private IPersistentProgressService _progressService;
        private IPopupMessageService _messageService;
        private IStaticDataService _staticData;
        private EquipmentStaticData _data;
        private EquipmentItem _item;

        [Inject] 
        private void Construct(IPersistentProgressService progressService, IPopupMessageService messageService, IStaticDataService staticData)
        {
            _progressService = progressService;
            _messageService = messageService;
            _staticData = staticData;
        }

        public void SetEquipment(EquipmentItem item, EquipmentStaticData data)
        {
            _item = item;
            _data = data;
        }

        private void Start()
        {
            SetEquipmentItemData();
            UpdateButtons();
        }

        private void OnEnable()
        {
            _equipButton.onClick.AddListener(OnEquipButtonClick);
            _unEquipButton.onClick.AddListener(OnUnequipButtonClick);
        }

        private void OnDisable()
        {
            _equipButton.onClick.RemoveListener(OnEquipButtonClick);
            _unEquipButton.onClick.RemoveListener(OnUnequipButtonClick);
        }

        private void SetEquipmentItemData()
        {
            _title.text = _data.Title;
            _equipmentIcon.sprite = _data.Icon;
            _level.text = string.Format(_level.text, _item.Level, 100);
            _description.text = _data.Description;
            _rarity.text = string.Format(_rarity.text, ColorUtility.ToHtmlStringRGBA(Color.green), _data.Rarity.ToString());
            _bonusesText.text = GetBonusesText();
        }

        private string GetBonusesText()
        {
            StringBuilder bonusesBuilder = new StringBuilder();

            foreach (var bonus in (_data.Bonuses))
            {
                bonusesBuilder.AppendLine($"{bonus.Type}: +{bonus.Value}");
            }

            return bonusesBuilder.ToString();
        }

        private void UpdateButtons()
        {
            bool isEquipped = _progressService.Equipments.IsItemEquipped(_progressService.Progress.SelectedHero, _item, _data.Category);

            _unEquipButton.gameObject.SetActive(isEquipped);
            _equipButton.gameObject.SetActive(!isEquipped);
        }

        private void OnUnequipButtonClick()
        {
            _progressService.Equipments.UnequipHero(_progressService.Progress.SelectedHero, _item, _data.Category);
            Destroy(gameObject);
           
        }

        private void OnEquipButtonClick()
        {

            TryEquip(_progressService.Progress.SelectedHero, _data.TypeId);

        }

        private void TryEquip(HeroTypeId hero, EquipmentTypeId equipment)
        {
            if (_staticData.ForHero(hero).HeroClass == _staticData.ForEquipment(equipment).HeroClass)
            {
                _progressService.Equipments.EquipHero(_progressService.Progress.SelectedHero, _item, _data.Category);
                Destroy(gameObject);
            }
            else
            {
                _messageService.ShowMessage("Unable to equip, mismatch of hero and weapon classes", Color.red);
            }
        }
    }
}
