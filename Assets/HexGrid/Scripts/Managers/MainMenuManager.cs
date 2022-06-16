using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private Button _settingsButton;
    [SerializeField]
    private Button _exitButton;

    private void Start()
    {
        Debug.LogError(Application.persistentDataPath);
        _playButton.onClick.AddListener(Play);
        _settingsButton.onClick.AddListener(OpenSettings);
        _exitButton.onClick.AddListener(ExitGame);
        AudioManager.Instance.PlayMusic(Music.MenuMusic);
    }

    private void Play()
    {
        Debug.LogError("Play");
        if (!WindowManager.Instance.WindowIsOpen())
        {
            AudioManager.Instance.PlaySound(Sounds.Button);
            WindowManager.Instance.OpenWindow(WindowType.StartWindow);
        }
    }

    private void OpenSettings()
    {
        if (!WindowManager.Instance.WindowIsOpen())
        {
            AudioManager.Instance.PlaySound(Sounds.Button);
            WindowManager.Instance.OpenWindow(WindowType.SettingsWindow);
        }
    }

    private void ExitGame()
    {
        AudioManager.Instance.PlaySound(Sounds.Button);
        Application.Quit();
    }
}