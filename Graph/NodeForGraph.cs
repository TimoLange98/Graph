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

        public void RemoveEdge()
        {
            //todo
        }
    }
}
