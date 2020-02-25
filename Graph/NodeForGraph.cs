using System;
using System.Text;

namespace Graph
{
    public class NodeG<T>
    {
        public T NodeData;

        public List<Edge<T>> Edges;


        public NodeG(T data)
        {
            NodeData = data;
            Edges = new List<Edge<T>>();
        }

        public override string ToString()
        {
            return NodeData.ToString();
        }

        public bool IsNeighbour(T data)
        {
            var currentEdge = Edges.First;

            while (currentEdge != null)
            {
                if (currentEdge.Data.FirstNodeOfEdge.Equals(this))
                {
                    if (currentEdge.Data.SecondNodeOfEdge.NodeData.Equals(data))
                        return true;
                }

                else if (currentEdge.Data.FirstNodeOfEdge.NodeData.Equals(data))
                    return true;

                currentEdge = currentEdge.Next;
            }

            return false;
        }
    }
}
