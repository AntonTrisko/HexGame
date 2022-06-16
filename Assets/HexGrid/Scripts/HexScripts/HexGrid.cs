using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    public int _width;
    [SerializeField]
    public int _height;
    public HexCell cellPrefab;

    HexCell[] cells;
    public Text cellLabelPrefab;

    public Canvas gridCanvas;
    public HexMesh hexMesh;

    void Awake()
    {
        GetSize();
        cells = new HexCell[_height * _width];
        for (int z = 0, i = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
        CreateBase(Player.Player1);
        CreateBase(Player.Player2);
        TriangulateAll();
    }

    private void GetSize()
    {
        string path = "GameSize";
        if (PlayerPrefs.HasKey(path))
        {
            int size = PlayerPrefs.GetInt(path);
            _height = size;
            _width = size;
        }
    }

    private void CreateBase(Player player)
    {
        HexCell playerBase = new HexCell();
        if (player.Equals(Player.Player1))
        {
            playerBase = cells[0];
        }
        else
        {
            playerBase = cells[cells.Length - 1];
        }
        playerBase.ClaimCell(player);
        playerBase.Build(BuildingType.Castle);
        CellsManager.Instance.AddNewCell(playerBase,player);
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.cellStatus = CellStatus.Unavaliable;

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - _width]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - _width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - _width]);
                if (x < _width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - _width + 1]);
                }
            }
        }

        Text label = Instantiate(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            HandleInput(true);
        }
    }

    void HandleInput(bool rightButton)
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            if (WindowManager.Instance.WindowIsOpen() && !WindowManager.Instance.isHiring)
            {
                return;
            }
            HexCell cell = GetCellFromClick(hit.point);
            if (WindowManager.Instance.isHiring && CanPlaceUnit(cell))
            {
                EventManager.PlaceUnit?.Invoke(cell);
            }
            else if (!rightButton)
            {
                TouchCell(cell);
            }
            else
            {
                EventManager.CellSelected?.Invoke(cell);
            }
        }
    }

    private bool CanPlaceUnit(HexCell cell)
    {
        if (cell.cellStatus == CellStatus.Available || cell.cellStatus == CellStatus.Unavaliable)
        {
            return false;
        }
        else if (cell.cellStatus == CellStatus.ClaimedPlayer1 && PlayerManager.Instance.currentPlayer == Player.Player2)
        {
            return false;
        }
        else if (cell.cellStatus == CellStatus.ClaimedPlayer2 && PlayerManager.Instance.currentPlayer == Player.Player1)
        {
            return false;
        }
        return true;
    }

    private void TouchCell(HexCell cell)
    {
        CellsManager.Instance.SetCurrentHex(cell);
        if (!PlayerManager.Instance.isMovingUnit)
        {
            switch (cell.cellStatus)
            {
                case CellStatus.Unavaliable:
                    Debug.LogError("Unavaliable");
                    break;
                case CellStatus.Available:
                    if (TryBuyCell())
                    {
                        ClaimCell(cell);
                    }
                    break;
                case CellStatus.ClaimedPlayer1:
                    if (PlayerManager.Instance.currentPlayer.Equals(Player.Player1))
                    {
                        WorkWithSelectedCell(cell);
                    }
                    break;
                case CellStatus.ClaimedPlayer2:
                    if (PlayerManager.Instance.currentPlayer.Equals(Player.Player2))
                    {
                        WorkWithSelectedCell(cell);
                    }
                    break;
            }
        }
    }

    private void WorkWithSelectedCell(HexCell cell)
    {
        if (cell.GetBuildingType().Equals(BuildingType.None))
        {
            Debug.LogError("ShowBuildingOptions");

            WindowManager.Instance.OpenWindow(WindowType.BuildingWindow);
        }
        else
        {
            Debug.LogError("ShowBuilding");
            cell.ShowUpgrade();
        }
    }

    private HexCell GetCellFromClick(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * _width + coordinates.Z / 2;
        return cells[index];
    }

    private bool TryBuyCell()
    {
        int amountAvaliable = CellsManager.Instance.GetProductionAmount(ResourceType.Gold,PlayerManager.Instance.currentPlayer);
        if (amountAvaliable - CellsManager.Instance.cellPrice >= 0)
        {
            CellsManager.Instance.AddResource(ResourceType.Gold, - CellsManager.Instance.cellPrice, PlayerManager.Instance.currentPlayer);
            EventManager.UpdateResources?.Invoke();
            CellsManager.Instance.cellPrice += 10;
            return true;
        }
        Debug.LogError("Not enough gold");
        return false;
    }

    private void ClaimCell(HexCell cell)
    {
        AudioManager.Instance.PlaySound(Sounds.BuyCell);
        CellsManager.Instance.AddNewCell(cell,PlayerManager.Instance.currentPlayer);
        cell.ClaimCell(PlayerManager.Instance.currentPlayer);
        TriangulateAll();
    }

    public void TriangulateAll()
    {
        hexMesh.Triangulate(cells);
    }
}