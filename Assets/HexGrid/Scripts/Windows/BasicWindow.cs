using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicWindow : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    [SerializeField]
    private Button _closeButton;

    public virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        _closeButton.onClick.AddListener(Close);
    }

    private void Close()
    {
        CloseWindow();
    }

    public void Update()
    {
        if (!WindowManager.Instance.isHiring && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWindow();
        }
    }

    public virtual void OpenWindow()
    {
        gameObject.SetActive(true);
        //AudioManager.Instance.PlaySound(Sounds.OpenWindow);
    }

    public virtual void CloseWindow()
    {
        WindowManager.Instance.CloseWindow();
        AudioManager.Instance.PlaySound(Sounds.CloseWindow);
        // gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // rectTransform.anchoredPosition += eventData.delta;
    }
}
