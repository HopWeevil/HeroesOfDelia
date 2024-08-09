using CodeBase.Data;
using CodeBase.SO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _levelText;

    public UnityAction<EquipmentStaticData, InventoryItem> OnClick;

    private EquipmentStaticData _data;
    private InventoryItem _item;

    public void SetEquipmentdata(EquipmentStaticData data, InventoryItem item)
    {
        _data = data;
        _item = item;
    }

    private void Start()
    {
        _icon.sprite =_data.Icon;
        _levelText.text = "Lv. " + _item.Level.ToString();
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
        OnClick?.Invoke(_data, _item);
    }
}
