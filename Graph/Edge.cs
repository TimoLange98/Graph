using System;
using System.Text;

namespace Graph
{
    public class Edge<T>
    {
        public NodeG<T> FirstNodeOfEdge;
        public int EdgeData;
        public NodeG<T> SecondNodeOfEdge;

        public Edge(NodeG<T> firstLoc, int data, NodeG<T> secondLoc)
        {
            FirstNodeOfEdge = firstLoc;
            EdgeData = data;
            SecondNodeOfEdge = secondLoc;
        }
    }
}
