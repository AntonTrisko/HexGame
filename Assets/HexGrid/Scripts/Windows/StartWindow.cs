using UnityEngine;
using UnityEngine.UI;

public class StartWindow : BasicWindow
{
    [SerializeField]
    private Button _smallGameSizeButton;
    [SerializeField]
    private Button _mediumGameSizeButton;
    [SerializeField]
    private Button _largeGameSizeButton;
    [SerializeField]
    private int _minSize;

    public override void Start()
    {
        base.Start();
        _smallGameSizeButton.onClick.AddListener(SmallGame);
        _mediumGameSizeButton.onClick.AddListener(MediumGame);
        _largeGameSizeButton.onClick.AddListener(LargeGame);
    }

    private void SmallGame()
    {
        StartGame(_minSize);
    }

    private void MediumGame()
    {
        StartGame(_minSize + 3);
    }

    private void LargeGame()
    {
        StartGame(_minSize + 6);
    }

    private void StartGame(int size)
    {
        AudioManager.Instance.PlaySound(Sounds.Button);
        PlayerPrefs.SetInt("GameSize", size);
        WindowManager.Instance.CloseWindow();
        WindowManager.Instance.OpenWindow(WindowType.FactionsWindow);
    }
}