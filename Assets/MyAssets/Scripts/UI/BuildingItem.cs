using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingItem : MonoBehaviour
{
    [SerializeField]
    private BuildingType _buildingType;
    [SerializeField]
    private Text _nameText;
    [SerializeField]
    private Button _showPricesButton;
    [SerializeField]
    private Text _showText;
    [SerializeField]
    private PricesDropdown _pricesDropdown;
    private Dictionary<ResourceType, int> _price;
    private Dictionary<ResourceType, int> _production;
    private bool _isOpened;

    private void Start()
    {
        _nameText.text = _buildingType.ToString();
        SetData();
        _showPricesButton.onClick.AddListener(ShowPrices);
        _pricesDropdown.buildButton.onClick.AddListener(BuyBuilding);
    }

    private void SetData()
    {
        switch (_buildingType)
        {
            case BuildingType.Castle:
                _price = BuildingsInfo.castleBuildingPrice;
                _production = BuildingsInfo.castleProduction;
                break;
            case BuildingType.Farm:
                _price = BuildingsInfo.farmBuildingPrice;
                _production = BuildingsInfo.farmProduction;
                break;
            case BuildingType.Lambermill:
                _price = BuildingsInfo.lambermillBuildingPrice;
                _production = BuildingsInfo.lambermillProduction;
                break;
            case BuildingType.Smith:
                _price = BuildingsInfo.smithBuildingPrice;
                _production = BuildingsInfo.smithProduction;
                break;
            case BuildingType.Mine:
                _price = BuildingsInfo.mineBuildingPrice;
                _production = BuildingsInfo.mineProduction;
                break;
        }
    }

    private void ShowPrices()
    {
        _isOpened = !_isOpened;
        _pricesDropdown.gameObject.SetActive(_isOpened);
        if (_isOpened)
        {
            _pricesDropdown.SetUp(_price, _production);
            _showText.text = "Hide";
        }
        else
        {
            _showText.text = "Show";
        }
    }

    private void BuyBuilding()
    {
        if (CellsManager.Instance.CanBuy(_price,1))
        {
            CellsManager.Instance.BuySomething(_price,1);
            Invoke(nameof(Build), 0.1f);
            WindowManager.Instance.CloseWindow();
        }
        else
        {
            Debug.LogError("Not enough ");
        }
    }

    private void Build()
    {
        AudioManager.Instance.PlaySound(Sounds.Build);
        HexCell cell = CellsManager.Instance.GetCurrentHex();
        cell.Build(_buildingType);
    }
}