using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : BasicWindow
{
    [SerializeField]
    private Text _nameText;
    [SerializeField]
    private Transform _productionLayout;
    [SerializeField]
    private Transform _upkeepLayout;
    [SerializeField]
    private Transform _priceLayout;
    [SerializeField]
    private Button _upgradeButton;
    [SerializeField]
    private Button _destroyButton;
    [SerializeField]
    private Text _levelText;
    [SerializeField]
    private Text _upgradeText;
    [SerializeField]
    private Image _buildingImage;

    private Dictionary<ResourceType, int> _price;
    private Dictionary<ResourceType, int> _production;
    private HexCell _cellForUpgrade;

    public override void Start()
    {
        base.Start();
        SetLevelText();
        _upgradeButton.onClick.AddListener(Upgrade);
        _destroyButton.onClick.AddListener(DestroyBuilding);
    }

    private void SetLevelText()
    {
        _levelText.text = _cellForUpgrade.level + "LVL";
    }

    private void Upgrade()
    {
        if (CellsManager.Instance.CanBuy(_price,_cellForUpgrade.level))
        {
            AudioManager.Instance.PlaySound(Sounds.Upgrade);
            CellsManager.Instance.BuySomething(_price, _cellForUpgrade.level);
            for (int i = 0; i < _cellForUpgrade.production.Values.Count; i++)
            {
                _cellForUpgrade.production[(ResourceType)i] *= 2;
            }
            _cellForUpgrade.UpgradeBuilding();
            _cellForUpgrade.level++;
            SetLevelText();
            CloseWindow();
        }
    }

    private void DestroyBuilding()
    {
        _cellForUpgrade.RemoveBuilding();
        CloseWindow();
    }

    public override void OpenWindow()
    {
        transform.localScale = Vector3.zero;
        base.OpenWindow();
        transform.DOScale(Vector3.one, 0.5f);
        _cellForUpgrade = CellsManager.Instance.GetCurrentHex();
        SetUpView();
        SetData(_cellForUpgrade.GetBuildingType());
        SetUpResources();
      
    }

    public override void CloseWindow()
    {
        ClearData();
        base.CloseWindow();
    }

    private void ClearData()
    {
        PriceElement[] prices = GetComponentsInChildren<PriceElement>();
        for (int i = 0; i < prices.Length; i++)
        {
            Destroy(prices[i].gameObject);
        }
    }

    private void SetUpView()
    {
        _nameText.text = _cellForUpgrade.GetBuildingType().ToString();
        _upgradeButton.interactable = true;
        _upgradeText.text = "Upgrade Cost";
        //set image
    }

    private void SetData(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Castle:
                _price = BuildingsInfo.castleUpgradePrice;
                _production = BuildingsInfo.castleProduction;
                SetImage(ResourceType.Gold);
                break;
            case BuildingType.Farm:
                _price = BuildingsInfo.farmUpgradePrice;
                _production = BuildingsInfo.farmProduction;
                SetImage(ResourceType.Food);
                break;
            case BuildingType.Lambermill:
                _price = BuildingsInfo.lambermillUpgradePrice;
                _production = BuildingsInfo.lambermillProduction;
                SetImage(ResourceType.Wood);
                break;
            case BuildingType.Smith:
                _price = BuildingsInfo.smithUpgradePrice;
                _production = BuildingsInfo.smithProduction;
                SetImage(ResourceType.Ore);
                break;
            case BuildingType.Mine:
                _price = BuildingsInfo.mineUpgradePrice;
                _production = BuildingsInfo.mineProduction;
                SetImage(ResourceType.Gold);
                break;
        }
    }

    private void SetImage(ResourceType resourceType)
    {
        _buildingImage.sprite = Resources.Load<Sprite>(resourceType.ToString());
    }

    private void SetUpResources()
    {
        WindowManager.Instance.SetUpSeparatedLayout(_production, _productionLayout,_upkeepLayout);
        if (_cellForUpgrade.level < 3)
        {
            WindowManager.Instance.SetUpLayout(_price, _priceLayout, _cellForUpgrade.level);
        }
        else
        {
            _upgradeButton.interactable = false;
            _upgradeText.text = "Max Upgrade";
        }
    }
}