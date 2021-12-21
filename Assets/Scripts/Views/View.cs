using System.Collections;
using UnityEngine;
using Utils;

[RequireComponent(typeof(CanvasGroup))]
public class View : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Timer _fadeTimer = new Timer();

    public virtual void Initialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void Show()
    {
        _canvasGroup.alpha = 1;
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        _canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    public virtual void FadeIn(float duration = 1f)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeInCoroutine(duration));
    }

    public virtual void FadeOut(float duration = 1f)
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeInCoroutine(float duration)
    {
        _fadeTimer.Start();

        while (_fadeTimer.Time < duration)
        {
            _canvasGroup.alpha = 1 - _fadeTimer.Time / duration;
            yield return null;
        }

        _canvasGroup.alpha = 0;
        gameObject.SetActive(false);
        _fadeTimer.Stop();
    }
    
    private IEnumerator FadeOutCoroutine(float duration)
    {
        _fadeTimer.Start();
        while (_fadeTimer.Time < duration)
        {
            _canvasGroup.alpha = _fadeTimer.Time / duration;
            yield return null;
        }

        _canvasGroup.alpha = 1;
        _fadeTimer.Stop();
    }
}