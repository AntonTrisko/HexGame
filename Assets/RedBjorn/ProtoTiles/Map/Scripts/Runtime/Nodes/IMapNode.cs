using System.Collections.Generic;

namespace RedBjorn.ProtoTiles
{
    public interface IMapNode
    {
        int Distance(INode x, INode y);
        IEnumerable<INode> Neighbours(INode node);
        IEnumerable<INode> NeighborsMovable(INode node);
        void Reset();
        void Reset(int range, INode startNode);
    }
}