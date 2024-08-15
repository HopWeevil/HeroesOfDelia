using TMPro;
using UnityEngine;
using DG.Tweening;

public class PopupMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 1.0f;

    [SerializeField] private CanvasGroup _canvasGroup;

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetColor(Color color)
    {
        _text.color = color;
    }

    public void Show()
    {
        _canvasGroup.alpha = 0f;

        _canvasGroup.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            DOVirtual.DelayedCall(displayDuration, Hide);
        });
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    public void HideImmediate()
    {
        Destroy(gameObject);
    }
}
