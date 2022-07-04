using System.Collections.Generic;

public class BuildingsInfo
{
    #region Zero
    public static Dictionary<ResourceType, int> zeroProduction = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,0},
        { ResourceType.Wood,0},
        { ResourceType.Ore,0},
        { ResourceType.Food,0}
    };
    #endregion

    #region Castle
    public static Dictionary<ResourceType, int> castleBuildingPrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,1000},
        { ResourceType.Wood,500},
        { ResourceType.Ore,800},
        { ResourceType.Food,0}
    };
    public static Dictionary<ResourceType, int> castleUpgradePrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,500},
        { ResourceType.Wood,300},
        { ResourceType.Ore,400},
        { ResourceType.Food,0}
    };
    public static Dictionary<ResourceType, int> castleProduction = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,100},
        { ResourceType.Wood,0},
        { ResourceType.Ore,0},
        { ResourceType.Food,0}
    };
    #endregion

    #region Farm
    public static Dictionary<ResourceType, int> farmBuildingPrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,200},
        { ResourceType.Wood,0},
        { ResourceType.Ore,0},
        { ResourceType.Food,0}
    };
    public static Dictionary<ResourceType, int> farmUpgradePrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,200},
        { ResourceType.Wood,300},
        { ResourceType.Ore,0},
        { ResourceType.Food,0}
    };
    public static Dictionary<ResourceType, int> farmProduction = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,-20},
        { ResourceType.Wood,0},
        { ResourceType.Ore,0},
        { ResourceType.Food,100}
    };
    #endregion

    #region Lambermill
    public static Dictionary<ResourceType, int> lambermillBuildingPrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,200},
        { ResourceType.Wood,0},
        { ResourceType.Ore,0},
        { ResourceType.Food,100}
    };
    public static Dictionary<ResourceType, int> lambermillUpgradePrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,0},
        { ResourceType.Wood,200},
        { ResourceType.Ore,200},
        { ResourceType.Food,0}
    };
    public static Dictionary<ResourceType, int> lambermillProduction = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,0},
        { ResourceType.Wood,100},
        { ResourceType.Ore,0},
        { ResourceType.Food,-20}
    };
    #endregion

    #region Farm
    public static Dictionary<ResourceType, int> smithBuildingPrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,200},
        { ResourceType.Wood,100},
        { ResourceType.Ore,0},
        { ResourceType.Food,0}
    };
    public static Dictionary<ResourceType, int> smithUpgradePrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,100},
        { ResourceType.Wood,300},
        { ResourceType.Ore,0},
        { ResourceType.Food,100}
    };
    public static Dictionary<ResourceType, int> smithProduction = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,0},
        { ResourceType.Wood,-20},
        { ResourceType.Ore,100},
        { ResourceType.Food,0}
    };
    #endregion

    #region Mine

    #endregion

    #region Church
    public static Dictionary<ResourceType, int> mineBuildingPrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,200},
        { ResourceType.Wood,200},
        { ResourceType.Ore,100},
        { ResourceType.Food,0}
    };
    public static Dictionary<ResourceType, int> mineUpgradePrice = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,100},
        { ResourceType.Wood,200},
        { ResourceType.Ore,200},
        { ResourceType.Food,0}
    };
    public static Dictionary<ResourceType, int> mineProduction = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Gold,100},
        { ResourceType.Wood,0},
        { ResourceType.Ore, 0},
        { ResourceType.Food,-20}
    };
    #endregion
}
