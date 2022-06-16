using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseWindow : BasicWindow
{
    [SerializeField]
    private Button _settingsButton;
    [SerializeField]
    private Button _menuButton;
    [SerializeField]
    private Button _quitButton;

    public override void Start()
    {
        base.Start();
        _settingsButton.onClick.AddListener(OpenSettings);
        _menuButton.onClick.AddListener(ToMenu);
        _quitButton.onClick.AddListener(Quit);
    }

    public override void OpenWindow()
    {
        transform.localScale = Vector3.zero;
        base.OpenWindow();
        transform.DOScale(Vector3.one, 0.5f);
    }

    private void OpenSettings()
    {
        AudioManager.Instance.PlaySound(Sounds.Button);
        WindowManager.Instance.OpenWindow(WindowType.SettingsWindow);
    }

    private void ToMenu()
    {
        AudioManager.Instance.PlaySound(Sounds.Button);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void Quit()
    {
        AudioManager.Instance.PlaySound(Sounds.Button);
        Application.Quit();
    }
}
