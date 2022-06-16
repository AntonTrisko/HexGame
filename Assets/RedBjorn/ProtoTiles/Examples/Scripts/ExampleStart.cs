using UnityEngine;

namespace RedBjorn.ProtoTiles.Example
{
    public class ExampleStart : MonoBehaviour
    {
        public MapSettings Map;
        public KeyCode GridToggle = KeyCode.G;
        MapEntity MapEntity;

        void Start()
        {
            var view = GameObject.FindObjectOfType<MapView>();
            MapEntity = Map.CreateEntity(view);
            if (view)
            {
                view.Init(MapEntity);
            }
            var unit = GameObject.FindObjectOfType<UnitMove>();
            if (unit)
            {
                unit.Init(MapEntity);
            }
        }

        void Update()
        {
            if (Input.GetKeyUp(GridToggle))
            {
                MapEntity.GridToggle();
            }
        }
    }
}
