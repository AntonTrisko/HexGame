using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance;
    private BasicWindow _currentWindow;
    [SerializeField]
    private List<BasicWindow> _windows;
    [SerializeField]
    private PriceElement _priceElement;
    public bool isHiring;


    private void OnEnable()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_currentWindow)
            {
                OpenWindow(WindowType.PauseWindow);
            }
        }
    }

    public bool WindowIsOpen()
    {
        if (_currentWindow)
        {
            return true;
        }
        return false;
    }

    public void OpenWindow(WindowType windowType)
    {
        _currentWindow = _windows[(int)windowType];
        _currentWindow?.OpenWindow();
    }

    public void CloseWindow()
    {
        if (!_currentWindow)
        {
            BasicWindow pauseWindow = _windows[(int)WindowType.PauseWindow];
            if (pauseWindow.gameObject.activeInHierarchy)
            {
                _currentWindow = pauseWindow;
            }
        }
        if (_currentWindow)
        {
            _currentWindow.gameObject.SetActive(false);
            _currentWindow = null;
        }
    }

    public void SetUpLayout(Dictionary<ResourceType, int> dict, Transform layout, int multiplier)
    {
        foreach (KeyValuePair<ResourceType, int> pair in dict)
        {
            if (pair.Value != 0)
            {
                PriceElement priceElement = Instantiate(_priceElement, layout);
                priceElement.SetUpElement(pair.Key, pair.Value * multiplier);
            }
        }
    }

    public void SetUpSeparatedLayout(Dictionary<ResourceType, int> dict, Transform layout1, Transform layout2)
    {
        foreach (KeyValuePair<ResourceType, int> pair in dict)
        {
            if (pair.Value > 0)
            {
                PriceElement priceElement = Instantiate(_priceElement, layout1);
                priceElement.SetUpElement(pair.Key, pair.Value);
            }
            else if (pair.Value < 0)
            {
                PriceElement priceElement = Instantiate(_priceElement, layout2);
                priceElement.SetUpElement(pair.Key, pair.Value);
            }
        }
    }
}

public enum WindowType
{
    BuildingWindow,
    UpgradeWindow,
    PauseWindow,
    SettingsWindow,
    HiringWindow,
    StartWindow,
    VictoryWindow,
    FactionsWindow
}