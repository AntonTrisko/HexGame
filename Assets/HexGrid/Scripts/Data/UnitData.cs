using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    public string unitName;
    public int health;
    public int strength;
    public int movement;

    public int gold;
    public int wood;
    public int food;
    public int ore;
}
