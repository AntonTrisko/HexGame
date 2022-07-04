using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Player currentPlayer;
    public bool isMovingUnit;
    public UnitScreen unitScreen;

    private void OnEnable()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(Music.GameMusic);
    }

    public void ChangePlayer()
    {
        if (currentPlayer.Equals(Player.Player1))
        {
            currentPlayer = Player.Player2;
        }
        else 
        {
            currentPlayer = Player.Player1;
        }
        Debug.LogError(currentPlayer);
    }

    public void SetWinner(Player player)
    {
        WindowManager.Instance.OpenWindow(WindowType.VictoryWindow);
        AudioManager.Instance.PlaySound(Sounds.Victory);
    }
}

public enum Player
{
    Player1,
    Player2
}