using CodeBase.Services.PersistentProgress;
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
        private EquipmentStaticData _data;
        private EquipmentItem _item;

        [Inject] 
        private void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
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
            bool isEquipped = _progressService.Inventory.IsItemEquipped(_progressService.Progress.SelectedHero, _item, _data.EquipmentClass);

            _unEquipButton.gameObject.SetActive(isEquipped);
            _equipButton.gameObject.SetActive(!isEquipped);
        }

        private void OnUnequipButtonClick()
        {
            _progressService.Inventory.UnequipHero(_progressService.Progress.SelectedHero, _item, _data.EquipmentClass);
            Destroy(gameObject);
        }

        private void OnEquipButtonClick()
        {
            _progressService.Inventory.EquipHero(_progressService.Progress.SelectedHero, _item, _data.EquipmentClass);
            Destroy(gameObject);
        }
    }
}
