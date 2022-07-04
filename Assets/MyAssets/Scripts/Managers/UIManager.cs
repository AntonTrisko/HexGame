using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button _menuButton;
    [SerializeField]
    private Button _nextTurnButton;
    [SerializeField]
    private Button _hireButton;
    [SerializeField]
    private Text _turnText;
    [SerializeField]
    private Transform _anchor;
    private Vector3 _startPosition;
    private Transform _turnTransform;
    private bool _isAnimating;

    private void Start()
    {
        _menuButton.onClick.AddListener(ShowMenu);
        _nextTurnButton.onClick.AddListener(NextTurn);
        _hireButton.onClick.AddListener(OpenHiringWindow);
        _turnTransform = _turnText.transform.parent;
        _startPosition = _turnTransform.position;
        ShowWhichTurn();
    }

    private void ShowMenu()
    {
        if (!WindowManager.Instance.WindowIsOpen())
        {
            WindowManager.Instance.OpenWindow(WindowType.PauseWindow);
        }
    }

    private void NextTurn()
    {
        if (!_isAnimating)
        {
            EventManager.NextTurn?.Invoke();
            AudioManager.Instance.PlaySound(Sounds.NextTurn);
            ShowWhichTurn();
        }
    }

    private void ShowWhichTurn()
    {
        ChangeAnimatingStatus();
        //Vector3 pos = _turnTransform.position;
        _turnText.text = PlayerManager.Instance.currentPlayer + "`s Turn";
        Tween tween = _turnTransform.DOMoveY(_anchor.position.y, 1);
        tween.OnComplete(() => ResetTurnTextPosition());
        tween.SetEase(Ease.InOutBack);
        
    }

    private void ResetTurnTextPosition()
    {
        Tween tween = _turnTransform.DOMoveY(_startPosition.y, 1);
        tween.SetEase(Ease.InOutBack);
        tween.SetDelay(0.5f);
        tween.OnComplete(() => ChangeAnimatingStatus());
    }

    private void ChangeAnimatingStatus()
    {
        _isAnimating = !_isAnimating;
    }

    private void OpenHiringWindow()
    {
        if (!WindowManager.Instance.WindowIsOpen())
        {
            WindowManager.Instance.OpenWindow(WindowType.HiringWindow);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextTurn();
        }
    }
}