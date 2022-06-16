using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class MapView : MonoBehaviour
    {
        GameObject Grid;

        public void Init(MapEntity map)
        {
            Grid = new GameObject("Grid");
            Grid.transform.SetParent(transform);
            Grid.transform.localPosition = Vector3.zero;
            map.CreateGrid(Grid.transform);
        }

        public void GridEnable(bool enable)
        {
            Grid.SetActive(enable);
        }

        public void GridToggle()
        {
            Grid.SetActive(!Grid.activeSelf);
        }
    }
}