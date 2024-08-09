using CodeBase.Services.PersistentProgress;
using CodeBase.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class EquipmentItemWindow : WindowBase
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Image _equipmentIcon;
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _unEquipButton;

        private IPersistentProgressService _progressService;
        private EquipmentStaticData _data;
        private InventoryItem _item;

        [Inject] 
        private void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void SetEquipmentData(EquipmentStaticData data)
        {
            _data = data;
        }

        public void SetItem(InventoryItem item)
        {
            _item = item;
        }

        private void Start()
        {
            _title.text = _data.Title;
            _equipmentIcon.sprite = _data.Icon;

            if (_progressService.Inventory.IsItemEquipped(_progressService.Progress.SelectedHero, _item, _data.EquipmentClass))
            {
                _unEquipButton.gameObject.SetActive(true);
                _equipButton.gameObject.SetActive(false);
            }
            else
            {               
                _unEquipButton.gameObject.SetActive(false);
                _equipButton.gameObject.SetActive(true);
            }
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
