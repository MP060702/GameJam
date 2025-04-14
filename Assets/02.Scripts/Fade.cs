using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Fade : MonoBehaviour
{
    public Color ableColor;
    public Color unableColor;
    private Color _lastColor;

    private SpriteRenderer _sprite;
    private Tweener _fadeTween;
    private bool _isFading = false;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _lastColor = ableColor;
        _fadeTween = _sprite.DOColor(ableColor, 0.5f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => _isFading = false)
            .SetAutoKill(false)
            .Pause();
    }

    public void SetAbleColor(bool isAllowed)
    {
        if (isAllowed && _lastColor != ableColor)
        {
            _lastColor = ableColor;
            _fadeTween.ChangeEndValue(ableColor);
        }
        else if (!isAllowed && _lastColor != unableColor)
        {
            _lastColor = unableColor;
            _fadeTween.ChangeEndValue(unableColor);
        }
    }

    public void CheakAndPlayFading()
    {
        if (!_isFading)
        {
            _fadeTween.Restart();
            _isFading = true;
        }
    }
}