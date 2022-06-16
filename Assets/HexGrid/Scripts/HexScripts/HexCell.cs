using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public UnitScript unitStanding;
    public Dictionary<ResourceType, int> production;
    public int level = 1;
    [SerializeField]
    public GameObject _player1Hex;
    [SerializeField]
    public GameObject _player2Hex;
    [SerializeField]
    HexCell[] neighbors;
    public CellStatus cellStatus;
    private BuildingType _buildingType;
    [SerializeField]
    private Transform _buildingsParent;
    private Transform _player1Buildings;
    private Transform _player2Buildings;
    [SerializeField]
    private List<GameObject> _playerBuildings;
    [SerializeField]
    private SpriteRenderer _sprite;
    [SerializeField]
    private SpriteRenderer _walkableSprite;
    [SerializeField]
    private Text _priceText;

    private Color _color;
    [SerializeField]
    private Color _unavaliableColor;
    [SerializeField]
    private Color _avaliableColor;
    [SerializeField]
    private Color _claimedColor;

    private void OnEnable()
    {
        PlayerBuildings();
        production = BuildingsInfo.zeroProduction;
    }

    private void PlayerBuildings()
    {
        int player1Buildings = PlayerPrefs.GetInt(Player.Player1.ToString());
        int player2Buildings = PlayerPrefs.GetInt(Player.Player2.ToString());
        _player1Buildings = _buildingsParent.GetChild(player1Buildings);
        _player2Buildings = _buildingsParent.GetChild(player2Buildings);
    }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public Color GetCurrentColor()
    {
        switch (cellStatus)
        {
            case CellStatus.Unavaliable:
                _color = _unavaliableColor;
                break;
            case CellStatus.Available:
                _color = _avaliableColor;
                break;
            case CellStatus.ClaimedPlayer1:
                _color = _claimedColor;
                break;
        }
        _sprite.color = _color;
        return _color;
    }

    public void SetNeighborsAvailable(bool avaliable)
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i])
            {
                if (neighbors[i].cellStatus == CellStatus.Unavaliable && avaliable)
                {
                    neighbors[i].cellStatus = CellStatus.Available;
                }
                else if (neighbors[i].cellStatus == CellStatus.Available && !avaliable)
                {
                    neighbors[i].cellStatus = CellStatus.Unavaliable;
                }
                neighbors[i].ChangePriceText(avaliable);
            }
        }
    }

    public void ChangePriceText(bool isActive)
    {
        _priceText.gameObject.transform.parent.gameObject.SetActive(isActive);
    }

    public void SetPrice()
    {
        _priceText.text = CellsManager.Instance.cellPrice.ToString();
    }

    public BuildingType GetBuildingType()
    {
        return _buildingType;
    }

    public void SetProduction()
    {
        switch (_buildingType)
        {
            case BuildingType.Castle:
                production = BuildingsInfo.castleProduction;
                break;
            case BuildingType.Farm:
                production = BuildingsInfo.farmProduction;
                break;
            case BuildingType.Lambermill:
                production = BuildingsInfo.lambermillProduction;
                break;
            case BuildingType.Smith:
                production = BuildingsInfo.smithProduction;
                break;
            case BuildingType.Mine:
                production = BuildingsInfo.mineProduction;
                break;
        }
    }

    public void Build(BuildingType buildingType)
    {
        _buildingType = buildingType;
        GameObject building;
        GetOwnersBuildings();
        building = _playerBuildings[(int)_buildingType - 1];
        AnimateConstruction(building);
        ParticlesManager.Instance.PlayParticle(Particles.Build, transform.position);
        SetProduction();
    }

    private void AnimateConstruction(GameObject building)
    {
        Vector3 scale = building.transform.localScale;
        building.transform.localScale = Vector3.zero;
        building.SetActive(true);
        building.transform.DOScale(scale, 0.5f);
    }

    public void RemoveBuilding()
    {
        GetOwnersBuildings();
        _playerBuildings[(int)_buildingType - 1].SetActive(false);
        _buildingType = BuildingType.None;
        production = BuildingsInfo.zeroProduction;
    }

    public void ClaimCell(Player player)
    {
        if (cellStatus.Equals(CellStatus.Available))
        {
            AnimateCell(player);
        }
        if (player.Equals(Player.Player1))
        {
            _player1Hex.SetActive(true);
            _player2Hex.SetActive(false);
            cellStatus = CellStatus.ClaimedPlayer1;
        }
        else
        {
            _player1Hex.SetActive(false);
            _player2Hex.SetActive(true);
            cellStatus = CellStatus.ClaimedPlayer2;
        }
        if (!_buildingType.Equals(BuildingType.None))
        {
            ChangeBuildingStyle(player);
        }
        ChangePriceText(false);
        SetNeighborsAvailable(true);
    }

    private void AnimateCell(Player player)
    {
        GameObject hex = new GameObject();
        if (player.Equals(Player.Player1))
        {
            hex = _player1Hex;
        }
        else
        {
            hex = _player2Hex;
        }
        Vector3 scale = hex.transform.localScale;
        hex.transform.localScale = Vector3.zero;
        hex.SetActive(true);
        hex.transform.DOScale(scale, 0.5f).SetEase(Ease.OutBack);
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i])
            {
                if (!neighbors[i].cellStatus.Equals(CellStatus.Available) && !neighbors[i].cellStatus.Equals(CellStatus.Unavaliable))
                {
                    neighbors[i].transform.DOShakeScale(0.5f, 0.1f);//.DOPunchScale(Vector3.one * 0.1f, 0.5f)
                }
            }
        }
    }

    public void ChangeWalkableSprite(bool isActive)
    {
        _walkableSprite.gameObject.SetActive(isActive);
    }

    public void ShowUpgrade()
    {
        PlayBuildingSound();
        GameObject building = new GameObject();
        GetOwnersBuildings();
        building = _playerBuildings[(int)_buildingType - 1];
        building.transform.DOShakeScale(0.5f, 0.1f).OnComplete(() => WindowManager.Instance.OpenWindow(WindowType.UpgradeWindow));
    }

    public void UpgradeBuilding()
    {
        Invoke(nameof(AnimateUpgrade), 0.1f);
    }

    private void AnimateUpgrade()
    {
        _playerBuildings[(int)_buildingType - 1].SetActive(false);
        GetOwnersBuildings();
        GameObject building = _playerBuildings[(int)_buildingType - 1];
        building.SetActive(true);
        Vector3 scale = building.transform.localScale;
        building.transform.localScale = Vector3.zero;
        building.transform.DOScale(scale, 0.5f);
        ParticlesManager.Instance.PlayParticle(Particles.Build, transform.position);
    }

    private void PlayBuildingSound()
    {
        string name = _buildingType.ToString();
        Sounds sound = (Sounds)Enum.Parse(typeof(Sounds), name);
        AudioManager.Instance.PlaySound(sound);
    }

    private void GetOwnersBuildings()
    {
        if (cellStatus.Equals(CellStatus.ClaimedPlayer1))
        {
            GetBuildingsList(Player.Player1);

        }
        else if (cellStatus.Equals(CellStatus.ClaimedPlayer2))
        {
            GetBuildingsList(Player.Player2);
        }
    }

    private void GetBuildingsList(Player player)
    {
        List<GameObject> buildings = new List<GameObject>();
        Transform buildingsParent;
        if (player.Equals(Player.Player1))
        {
            buildingsParent = _player1Buildings.GetChild(level - 1);
        }
        else
        {
            buildingsParent = _player2Buildings.GetChild(level - 1);
        }
        for (int i = 0; i < buildingsParent.childCount; i++)
        {
            buildings.Add(buildingsParent.GetChild(i).gameObject);
        }
        _playerBuildings = buildings;
    }

    private void ChangeBuildingStyle(Player newOwner)
    {
        if (newOwner.Equals(Player.Player1))
        {
            GetBuildingsList(Player.Player2);
            _playerBuildings[(int)_buildingType - 1].SetActive(false);
            ParticlesManager.Instance.PlayParticle(Particles.Build, transform.position);
            GetBuildingsList(Player.Player1);
            _playerBuildings[(int)_buildingType - 1].SetActive(true);
        }
        else
        {
            GetBuildingsList(Player.Player1);
            _playerBuildings[(int)_buildingType - 1].SetActive(false);
            ParticlesManager.Instance.PlayParticle(Particles.Build, transform.position);
            GetBuildingsList(Player.Player2);
            _playerBuildings[(int)_buildingType - 1].SetActive(true);
        }
    }
}

public enum CellStatus
{
    Unavaliable,
    Available,
    ClaimedPlayer1,
    ClaimedPlayer2
}

public enum BuildingType
{
    None,
    Farm,
    Lambermill,
    Smith,
    Castle,
    Mine
}

public enum ResourceType
{
    Gold,
    Wood,
    Ore,
    Food
}