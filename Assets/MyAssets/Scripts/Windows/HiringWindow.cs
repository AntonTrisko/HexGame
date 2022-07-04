using DG.Tweening;
using UnityEngine;

public class HiringWindow : BasicWindow
{
    private Vector3 _startPos;
    private Transform _player1Units;
    private Transform _player2Units;

    public override void OpenWindow()
    {
        _startPos = transform.position;
        transform.position += Vector3.right * 100;
        base.OpenWindow();
        SetUnits();
        OpenPlayerUnits(PlayerManager.Instance.currentPlayer);
        transform.DOMoveX(_startPos.x, 1);
    }

    private void SetUnits()
    {
        int player1Units = PlayerPrefs.GetInt(Player.Player1.ToString());
        int player2Units = PlayerPrefs.GetInt(Player.Player2.ToString());
        _player1Units = transform.GetChild(player1Units);
        _player2Units = transform.GetChild(player2Units);
    }

    private void OpenPlayerUnits(Player player)
    {
        if (player.Equals(Player.Player1))
        {
            _player1Units.gameObject.SetActive(true);
            _player2Units.gameObject.SetActive(false);
        }
        else
        {
            _player1Units.gameObject.SetActive(false);
            _player2Units.gameObject.SetActive(true);
        }
    }
}
