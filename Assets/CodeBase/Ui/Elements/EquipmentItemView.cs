using CodeBase.Infrastructure.Factories;
using CodeBase.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class EquipmentItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _levelText;

        private EquipmentStaticData _data;
        private EquipmentItem _item;
        private IUIFactory _factory;

        [Inject]
        private void Construct(IUIFactory uIFactory)
        {
            _factory = uIFactory;
        }

        public void SetEquipmentData(EquipmentStaticData data, EquipmentItem item)
        {
            _data = data;
            _item = item;
        }

        private void Start()
        {
            _icon.sprite = _data.Icon;
            _levelText.text = string.Format(_levelText.text, _item.Level.ToString());
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleClick);
        }

        private void HandleClick()
        {
            _factory.CreateEquipmentInfoWindow(_item);
        }
    }
}