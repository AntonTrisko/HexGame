using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitScript : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _selectionParticles;
    private HexCell _currentCell;
    private bool _isSelected;
    [SerializeField]
    private int _strength;
    [SerializeField]
    private int _health;
    [SerializeField]
    private int _movement;
    [SerializeField]
    private Image _healthbar;
    private bool _isRecentlySelected;
    [SerializeField]
    private float _speed;
    private UnitScreen _unitScreen;
    private int _movementLeft;
    private bool _isMoving;
    private int _currentHealth;
    public Player owner;

    private void Start()
    {
        EventManager.NextTurn += NewTurn;
        EventManager.CellSelected += SetCurrentHex;
        SetUp();
    }

    private void SetUp()
    {
        _unitScreen = PlayerManager.Instance.unitScreen;
        _movementLeft = _movement;
        _currentHealth = _health;
        _healthbar.fillAmount = 1;
    }

    private void NewTurn()
    {
        if (_isSelected)
        {
            Deselect();
        }
        _movementLeft = _movement;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isSelected && !_isRecentlySelected)
        {
            Deselect();
        }
    }

    private void SetCurrentHex(HexCell hexCell)
    {
        if (!_isMoving && _isSelected && _movementLeft > 0)
        {
            HexDirection[] hexDirection = Enum.GetValues(typeof(HexDirection)).Cast<HexDirection>().ToArray();
            for(int i = 0;i < hexDirection.Length;i++)
            {
                if (hexCell.Equals(_currentCell.GetNeighbor(hexDirection[i])) && CellIsWalkable(hexCell))
                {
                    SetWalkableCells(false);
                    if (!hexCell.unitStanding)
                    {
                        GetNewCell(hexCell);
                    }
                    else if(hexCell.unitStanding.owner != owner)
                    {
                        Attack(hexCell);
                    }
                }
            }
        }
    }

    private void GetNewCell(HexCell hexCell)
    {
        _currentCell.unitStanding = null;
        hexCell.unitStanding = this;
        _currentCell = hexCell;
        _currentCell.unitStanding = this;
        StartCoroutine(Move());
        UpdateMovement(_movementLeft - 1);
    }

    IEnumerator Move()
    {
        float time = 0;
        _isMoving = true;
        while (Vector3.Distance(transform.position,AdjustedPosition()) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, AdjustedPosition(), time);
            Vector3 targetDirection = AdjustedPosition() - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetDirection),Time.deltaTime);
            time += _speed *3 * Time.deltaTime;
            yield return null;
        }
        SetWalkableCells(true);
        Conquer();
        _isMoving = false;
    }

    private void SetWalkableCells(bool isActive)
    {
        HexDirection[] hexDirection = Enum.GetValues(typeof(HexDirection)).Cast<HexDirection>().ToArray();
        for (int i = 0; i < hexDirection.Length; i++)
        {
            HexCell cell = _currentCell.GetNeighbor(hexDirection[i]);
            if (cell && CellIsWalkable(cell))
            {
                cell.ChangeWalkableSprite(isActive);
            }
        }
    }

    private void UpdateMovement(int newValue)
    {
        _movementLeft = newValue;
        _unitScreen.SetMovement(_movementLeft, _movement);
    }

    private Vector3 AdjustedPosition()
    {
        return _currentCell.transform.position + Vector3.up * 5;
    }

    private void Attack(HexCell cell)
    {
        StartCoroutine(AnimateAttack(cell));
    }

    IEnumerator AnimateAttack(HexCell cell)
    {
        _isMoving = true;
        float time = 0;
        Vector3 target = cell.unitStanding.transform.position;
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position,target, time);
            Vector3 targetDirection = target - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime);
            time += _speed * 3 * Time.deltaTime;
            yield return null;
        }
        _isMoving = false;
        cell.unitStanding.TakeDamage(_strength, this);
        if (!cell.unitStanding)
        {
            GetNewCell(cell);
        }
        _movementLeft = 0;
        if (_isSelected)
        {
            StartCoroutine(Move());
            _unitScreen.SetMovement(_movementLeft, _movement);
        }
    }

    private void OnMouseDown()
    {
        if (CanSelect())
        {
            Debug.LogError("Selected");
            Select();
        }
    }

    private bool CanSelect()
    {
        if (owner != PlayerManager.Instance.currentPlayer)
        {
            return false;
        }
        return true;
    }

    private void Select()
    {
        PlayerManager.Instance.isMovingUnit = true;
        _isSelected = true;
        _isRecentlySelected = true;
        _selectionParticles.gameObject.SetActive(true);
        SetUpScreen();
        Invoke(nameof(ResetSelection), 0.5f);
        SetWalkableCells(true);
    }

    private void ResetSelection()
    {
        _isRecentlySelected = false;
    }

    private void Deselect()
    {
        Debug.LogError("Deselected");
        _isSelected = false;
        _selectionParticles.gameObject.SetActive(false);
        PlayerManager.Instance.isMovingUnit = false;
        _unitScreen.gameObject.SetActive(false);
        SetWalkableCells(false);
    }

    private void SetUpScreen()
    {
        _unitScreen.SetName(gameObject.name);
        _unitScreen.SetStrength(_strength);
        _unitScreen.SetHealth(_currentHealth);
        _unitScreen.SetMovement(_movementLeft, _movement);
        _unitScreen.gameObject.SetActive(true);
    }

    private bool CellIsWalkable(HexCell cell)
    {
        if (cell.cellStatus.Equals(CellStatus.ClaimedPlayer1) || cell.cellStatus.Equals(CellStatus.ClaimedPlayer2))
        {
            return true;
        }
        return false;
    }

    private void Conquer()
    {
        if ((_currentCell.cellStatus.Equals(CellStatus.ClaimedPlayer2) && owner.Equals(Player.Player1)) ||
           (_currentCell.cellStatus.Equals(CellStatus.ClaimedPlayer1) && owner.Equals(Player.Player2)))
        {
            _currentCell.ClaimCell(owner);
            CellsManager.Instance.ChangeOwner(owner, _currentCell);
        }
    }

    public void TakeDamage(int amount, UnitScript attacker)
    {
        _currentHealth -= amount;
        if (attacker)
        {
            attacker.TakeDamage(_strength, null);
        }
        SetHealth();
    }

    private void SetHealth()
    {
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _health);
        _healthbar.fillAmount = (float)_currentHealth / _health;
        if (_currentHealth <= 0)
        {
            Die();
        }
        if (_isSelected)
        {
            _unitScreen.SetHealth(_currentHealth);
        }
    }

    private void Die()
    {
        if (_isSelected)
        {
            Deselect();
        }
        ParticlesManager.Instance.PlayParticle(Particles.Death, transform.position);
        gameObject.SetActive(false);
    }

    public void SetUpUnit(HexCell cell,UnitData data)
    {
        _currentCell = cell;
        cell.unitStanding = this;
        owner = PlayerManager.Instance.currentPlayer;
        transform.position = AdjustedPosition();
        _health = data.health;
        _strength = data.strength;
        _speed = data.movement;
        gameObject.name = data.unitName;
    }
}