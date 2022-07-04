using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FactionsWindow : BasicWindow
{
    [SerializeField]
    private DropdownScript _player1Dropdown;
    [SerializeField]
    private DropdownScript _player2Dropdown;
    [SerializeField]
    private Button _playButton;

    public override void Start()
    {
        base.Start();
        AddListeners(_player1Dropdown);
        AddListeners(_player2Dropdown);
        CheckPlayStatus();
        _playButton.onClick.AddListener(Play);
    }

    private void AddListeners(DropdownScript dropdown)
    {
        dropdown.DropdownOpened += OnDropdownOpened;
        dropdown.DropdownChange += CheckPlayStatus;
    }

    private void OnDropdownOpened(Dropdown dropdown)
    {
        if (dropdown.Equals(_player1Dropdown.dropdown))
        {
            SetOptionsAvaliable(dropdown, _player2Dropdown.GetValue());
        }
        else
        {
            SetOptionsAvaliable(dropdown, _player1Dropdown.GetValue());
        }
        
    }
    private void SetOptionsAvaliable(Dropdown dropdown, int indexToDisable)
    {
        Canvas canvas = dropdown.GetComponentInChildren<Canvas>();
        if (!canvas) 
        {
            return;
        }
        var toggles = canvas.GetComponentsInChildren<Toggle>(true);
        for (int i = 0; i < toggles.Length; i++)
        {
            if (i != indexToDisable)
            {
                toggles[i].interactable = true;
            }
            else
            {
                toggles[i].interactable = false;
            }
        }
    }

    private void CheckPlayStatus()
    {
        bool interactable = _player1Dropdown.dropdown.value.Equals(_player2Dropdown.dropdown.value);
        _playButton.interactable = !interactable;
    }

    private void Play()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);

    }

    private void OnDestroy()
    {
        RemoveListeners(_player1Dropdown);
        RemoveListeners(_player2Dropdown);
    }

    private void RemoveListeners(DropdownScript dropdown)
    {
        dropdown.DropdownOpened -= OnDropdownOpened;
        dropdown.DropdownChange -= CheckPlayStatus;
    }
}

public enum Factions
{
    Humans,
    Elves,
    Undead,
    Orcs
}