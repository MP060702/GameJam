using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Blinking : MonoBehaviour
{
    public bool isBliking = false;

    private SpriteRenderer _sprite;
    private Tween _fadeTween;
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        
        _fadeTween = _sprite.DOFade(0f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    void Update()
    {
        if (isBliking)
        {
            if(_fadeTween.IsPlaying() == false)
                _fadeTween.Play();
        }
        else
        {
            _fadeTween.Pause();
        }
    }
}
