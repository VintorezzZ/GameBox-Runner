using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class View : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public virtual void Initialize()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void Show()
    {
        canvasGroup.alpha = 1;
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}