using System;
using System.Text;

namespace Graph
{
    public class Edge<T>
    {
        public NodeG<T> FirstLocOfEdge;
        public T EdgeData;
        public NodeG<T> SecondLocOfEdge;

        public Edge(NodeG<T> firstLoc, T data, NodeG<T> secondLoc)
        {
            FirstLocOfEdge = firstLoc;
            EdgeData = data;
            SecondLocOfEdge = secondLoc;
        }
    }
}
