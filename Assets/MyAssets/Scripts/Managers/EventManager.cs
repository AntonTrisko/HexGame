using System;

public class EventManager
{
    public static Action UpdateResources;
    public static Action NextTurn;
    public static Action<HexCell> CellSelected;
    public static Action<HexCell> PlaceUnit;
}