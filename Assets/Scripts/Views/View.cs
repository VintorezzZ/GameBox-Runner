using UnityEngine;

namespace Views
{
    public abstract class View : MonoBehaviour
    {
        public abstract void Initialize();
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
    }
}
