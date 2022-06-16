using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class MapBorder
    {
        public struct BorderPoint
        {
            public Vector3Int TilePos;
            public int verticeIndex;
        }

        public static HashSet<BorderPoint> FindSidePositions(IMapDirectional map, HashSet<Vector3Int> inside)
        {
            var directions = map.NeighboursDirection;
            Func<Vector3Int, int> leftVerticeCalc = (v) => (Array.IndexOf(directions, v) + directions.Length - 1) % directions.Length;
            Func<Vector3Int, int> rightVerticeCalc = (v) => Array.IndexOf(directions, v);
            var startPos = Vector3Int.zero;
            var direction = Vector3Int.zero;
            var border = new HashSet<BorderPoint>();

            var found = false;
            startPos = inside.Aggregate((v1, v2) => v1.y > v2.y ? v1 : v2);
            for (int i = 0; i < directions.Length; i++)
            {
                var neighPos = startPos + directions[i];
                if (!inside.Contains(neighPos))
                {
                    direction = directions[i] * -1; //came to startPos from this direction
                    found = true;
                    break;
                }
            }

            if (found)
            {
                border.Add(new BorderPoint { TilePos = startPos, verticeIndex = leftVerticeCalc(direction * -1) });
                border.Add(new BorderPoint { TilePos = startPos, verticeIndex = rightVerticeCalc(direction * -1) });

                var finishDirection = direction;
                var opFinishDirection = direction * -1;

                var previousPos = startPos;
                direction = map.TurnLeft(direction);
                var currentPos = startPos + direction;

                while (startPos != currentPos || (direction != finishDirection && direction != opFinishDirection))
                {
                    var leftVertice = leftVerticeCalc(direction);
                    var rightVertice = rightVerticeCalc(direction);
                    if (inside.Contains(currentPos))
                    {
                        direction = map.TurnLeft(direction);
                    }
                    else
                    {
                        direction = map.TurnRight(direction);
                    }
                    var goInside = inside.Contains(previousPos) && !inside.Contains(currentPos);
                    var goOutside = !inside.Contains(previousPos) && inside.Contains(currentPos);
                    if (goOutside)
                    {
                        border.Add(new BorderPoint { TilePos = previousPos, verticeIndex = leftVertice });
                    }
                    if (goInside)
                    {
                        border.Add(new BorderPoint { TilePos = previousPos, verticeIndex = rightVertice });
                    }

                    previousPos = currentPos;
                    currentPos = currentPos + direction;
                }
                border.Add(new BorderPoint { TilePos = startPos, verticeIndex = rightVerticeCalc(direction * -1) });
            }

            return border;
        }
    }
}
