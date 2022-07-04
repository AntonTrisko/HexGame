using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Player _player;
    public Dropdown dropdown;
    public Action<Dropdown> DropdownOpened;
    public Action DropdownChange;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
        SetUpDropdown(dropdown);
        SetFaction();
    }

    private void SetUpDropdown(Dropdown dropdown)
    {
        Factions factions = new Factions();
        Type type = factions.GetType();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        dropdown.ClearOptions();
        for (int i = 0; i < Enum.GetNames(type).Length; i++)
        {
            options.Add(new Dropdown.OptionData(Enum.GetName(type, i)));
        }
        dropdown.AddOptions(options);
        dropdown.onValueChanged.AddListener(delegate { DropdownChanged(dropdown); });
    }

    public int GetValue()
    {
        return dropdown.value + 1;
    }

    private void DropdownChanged(Dropdown dropdown)
    {
        DropdownChange?.Invoke();
        SetFaction();
    }

    private void SetFaction()
    {
        PlayerPrefs.SetInt(_player.ToString(), dropdown.value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DropdownOpened?.Invoke(dropdown);
    }
}