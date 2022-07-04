using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyUnitItem : MonoBehaviour
{
    [SerializeField]
    private UnitData _unitData;
    [SerializeField]
    private UnitScript _unit;
    [SerializeField]
    private PricesDropdown _pricesDropdown;
    [SerializeField]
    private Text _nameText;
    [SerializeField]
    private Text _healthText;
    [SerializeField]
    private Text _strengthText;
    [SerializeField]
    private Text _movementText;
    [SerializeField]
    private Button _showButton;
    [SerializeField]
    private Text _showText;
    private bool _isOpened;
    private bool _isHiring;
    private Dictionary<ResourceType, int> _price;

    void Start()
    {
        _showButton.onClick.AddListener(ShowPrices);
        _pricesDropdown.buildButton.onClick.AddListener(Hire);
        SetUp();
        EventManager.PlaceUnit += PlaceUnit;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.LogError("Change");
            ChangeHiringStatus();
        }
    }

    private void ShowPrices()
    {
        _isOpened = !_isOpened;
        _pricesDropdown.gameObject.SetActive(_isOpened);
        if (_isOpened)
        {
            _showText.text = "Hide";
        }
        else
        {
            _showText.text = "Show";
        }
    }

    private void Hire()
    {
        if (CellsManager.Instance.CanBuy(_price, 1))
        {
            ChangeHiringStatus();
        }
    }

    private void ChangeHiringStatus()
    {
        _isHiring = !_isHiring;
        CellsManager.Instance.HighlightCells(_isHiring);
        WindowManager.Instance.isHiring = _isHiring;
    }

    private void PlaceUnit(HexCell cell)
    {
        if (_isHiring)
        {
            ChangeHiringStatus();
            UnitScript unit = Instantiate(_unit);
            unit.SetUpUnit(cell,_unitData);
            AudioManager.Instance.PlaySound(Sounds.Hire);
            CellsManager.Instance.BuySomething(_price, 1);
            WindowManager.Instance.CloseWindow();
        }
    }

    private void SetUp()
    {
        _nameText.text = _unitData.unitName;
        _healthText.text = _unitData.health.ToString();
        _strengthText.text = _unitData.strength.ToString();
        _movementText.text = _unitData.movement.ToString();
         _price = new Dictionary<ResourceType, int>()
         {
              { ResourceType.Gold,_unitData.gold},
              { ResourceType.Wood,_unitData.wood},
              { ResourceType.Ore,_unitData.food},
              { ResourceType.Food,_unitData.ore}
         };
         _pricesDropdown.SetUp(_price, null);
    }
}
