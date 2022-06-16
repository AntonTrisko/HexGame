using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellsManager : MonoBehaviour
{
    public static CellsManager Instance;
    private HexCell _currentHex;
    [SerializeField]
    private List<HexCell> _player1HexCells;
    [SerializeField]
    private List<HexCell> _player2HexCells;
    private Dictionary<ResourceType, int> _player1Resources;
    private Dictionary<ResourceType, int> _player2Resources;
    private HexGrid _hexGrid;
    private int _player1CellPrice = 50;
    private int _player2CellPrice = 50;
    public int cellPrice = 50;

    private void OnEnable()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        SetResources();
        AddResource(ResourceType.Gold, 9999, Player.Player1);
        AddResource(ResourceType.Food, 9999, Player.Player1);
        AddResource(ResourceType.Ore, 9999, Player.Player1);
        AddResource(ResourceType.Wood, 9999, Player.Player1);

        AddResource(ResourceType.Gold, 9999, Player.Player2);
        AddResource(ResourceType.Food, 9999, Player.Player2);
        AddResource(ResourceType.Ore, 9999, Player.Player2);
        AddResource(ResourceType.Wood, 9999, Player.Player2);
        _hexGrid = FindObjectOfType<HexGrid>();
        cellPrice = _player1CellPrice;
    }

    private void SetResources()
    {
        _player1Resources =
            Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToDictionary(t => t, t => 0);
        _player2Resources =
            Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToDictionary(t => t, t => 0);
    }

    private void Start()
    {
        SetAvailablility(_player2HexCells, false);
        SetAvailablility(_player1HexCells, true);
        _hexGrid.TriangulateAll();
        EventManager.NextTurn += NextTurn;
    }

    private void NextTurn()
    {
        GetCellsProduction(PlayerManager.Instance.currentPlayer);
        PlayerManager.Instance.ChangePlayer();
        ChangePlayers();
        _hexGrid.TriangulateAll();
        EventManager.UpdateResources?.Invoke();
    }

    private void ChangePlayers()
    {
        if (PlayerManager.Instance.currentPlayer.Equals(Player.Player1))
        {
            SetAvailablility(_player2HexCells, false);
            SetAvailablility(_player1HexCells, true);
            _player1CellPrice = cellPrice;
            cellPrice = _player2CellPrice;
        }
        else
        {
            SetAvailablility(_player1HexCells, false);
            SetAvailablility(_player2HexCells, true);
            _player2CellPrice = cellPrice;
            cellPrice = _player1CellPrice;
        }
    }

    public void GetCellsProduction(Player player)
    {
        for (int i = 0; i < Enum.GetValues(typeof(ResourceType)).Length; i++)
        {
            if (player.Equals(Player.Player1))
            {
                _player1Resources[(ResourceType)i] += GetProductionIncome((ResourceType)i, PlayerManager.Instance.currentPlayer);
            }
            else
            {
                _player2Resources[(ResourceType)i] += GetProductionIncome((ResourceType)i, PlayerManager.Instance.currentPlayer);
            }
        }
        EventManager.UpdateResources?.Invoke();
    }

    public int GetProductionIncome(ResourceType resource, Player player)
    {
        int income = 0;
        if (player.Equals(Player.Player1))
        {
            for (int i = 0; i < _player1HexCells.Count; i++)
            {
                income += _player1HexCells[i].production[resource];
            }
        }
        else
        {
            for (int i = 0; i < _player2HexCells.Count; i++)
            {
                income += _player2HexCells[i].production[resource];
            }
        }
        return income;
    }

    public void AddResource(ResourceType resourceType, int amount, Player player)
    {
        if (player.Equals(Player.Player1))
        {
            _player1Resources[resourceType] += amount;
        }
        else
        {
            _player2Resources[resourceType] += amount;
        }
    }

    public int GetProductionAmount(ResourceType production, Player player)
    {
        if (player.Equals(Player.Player1))
        {
            return _player1Resources[production];
        }
        else
        {
            return _player2Resources[production];
        }
    }

    public void AddNewCell(HexCell hexCell, Player player)
    {
        if (player.Equals(Player.Player1))
        {
            _player1HexCells.Add(hexCell);
        }
        else
        {
            _player2HexCells.Add(hexCell);
        }
    }

    public void ChangeOwner(Player player, HexCell cell)
    {
        if (player.Equals(Player.Player1))
        {
            _player2HexCells.Remove(cell);
            _player1HexCells.Add(cell);
        }
        else
        {
            _player1HexCells.Remove(cell);
            _player2HexCells.Add(cell);
        }
        CheckForGameEnd();
    }

    private void CheckForGameEnd()
    {
        if (_player1HexCells.Count == 0)
        {
            PlayerManager.Instance.SetWinner(Player.Player2);
        }
        else if (_player2HexCells.Count == 0)
        {
            PlayerManager.Instance.SetWinner(Player.Player1);
        }
    }

    public void SetCurrentHex(HexCell hexCell)
    {
        _currentHex = hexCell;
    }

    public HexCell GetCurrentHex()
    {
        return _currentHex;
    }

    public bool CanBuy(Dictionary<ResourceType, int> price, int multiplier)
    {
        foreach (KeyValuePair<ResourceType, int> pair in price)
        {
            if (GetProductionAmount(pair.Key, PlayerManager.Instance.currentPlayer) < pair.Value * multiplier)
            {
                return false;
            }
        }
        return true;
    }

    public void BuySomething(Dictionary<ResourceType, int> price, int multiplier)
    {
        foreach (KeyValuePair<ResourceType, int> pair in price)
        {
            AddResource(pair.Key, -pair.Value * multiplier, PlayerManager.Instance.currentPlayer);
        }
        EventManager.UpdateResources?.Invoke();
    }

    public void SetAvailablility(List<HexCell> cells, bool availability)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].SetNeighborsAvailable(availability);
        }
    }

    public void HighlightCells(bool isActive)
    {
        List<HexCell> cells;
        if (PlayerManager.Instance.currentPlayer.Equals(Player.Player1))
        {
            cells = _player1HexCells;
        }
        else
        {
            cells = _player2HexCells;
        }
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].ChangeWalkableSprite(isActive);
        }
    }
}