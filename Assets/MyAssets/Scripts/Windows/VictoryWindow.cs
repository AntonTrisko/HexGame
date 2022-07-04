using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryWindow : BasicWindow
{
    [SerializeField]
    private Button _menuButton;
    [SerializeField]
    private Button _retryButton;
    [SerializeField]
    private Text _text;

    public override void OpenWindow()
    {
        transform.localScale = Vector3.zero;
        base.OpenWindow();
        transform.DOScale(Vector3.one, 0.5f);
    }

    public override void Start()
    {
        SetVictoryText();
        _menuButton.onClick.AddListener(ToMenu);
        _retryButton.onClick.AddListener(Retry);
    }

    private void SetVictoryText()
    {
        _text.text = PlayerManager.Instance.currentPlayer + " Wins!";
    }

    private void ToMenu()
    {
        AudioManager.Instance.PlaySound(Sounds.Button);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}