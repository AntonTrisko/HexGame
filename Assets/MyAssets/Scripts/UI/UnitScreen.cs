using UnityEngine;
using UnityEngine.UI;

public class UnitScreen : MonoBehaviour
{
    [SerializeField]
    private Text _nameText;
    [SerializeField]
    private Text _strengthText;
    [SerializeField]
    private Text _healthText;
    [SerializeField]
    private Text _movementText;

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    public void SetHealth(int health)
    {
        _healthText.text = health.ToString();
    }

    public void SetStrength(int strength)
    {
        _strengthText.text = strength.ToString();
    }

    public void SetMovement(int currentMovement, int maxMovement)
    {
        _movementText.text = currentMovement + "/" + maxMovement;
    }
}
