using System;
using System.Text;

namespace Graph
{
    public class Graph<T>
    {

        public List<NodeG<T>> NodesInGraph;
        List<Edge<T>> AllEdges;

        public Graph(params T[] datas)
        {
            AddNodes(datas);
            NodesInGraph = new List<NodeG<T>>();
            AllEdges = new List<Edge<T>>();
        }

        //-----------------------------------------------------------------------------------------
        void AddSingleNode(T data)
        {
            var help = NodesInGraph.First;

            while (help != null)
            {
                if (help.Data.Equals(data))
                {
                    return;
                }
                help = help.Next;
            }
            NodesInGraph.Add(new NodeG<T>(data));
        }

        //-----------------------------------------------------------------------------------------
        void RemoveSingleNode(T data, bool removeEdges = default)
        {
            var node = FindNode(data);

            if (node.Edges != null && removeEdges == false)
            {
                throw new Exception("There are still connection(s) refering to this Element");
            }

            else if (node.Edges == null)
            {
                NodesInGraph.Remove(node);
                return;
            }

            var currentEdge = node.Edges.First;

            while (currentEdge != null)
            {
                if (currentEdge.Data.FirstNodeOfEdge.NodeData.Equals(node.NodeData))
                    RemoveEdge(node.NodeData, currentEdge.Data.SecondNodeOfEdge.NodeData);

                RemoveEdge(node.NodeData, currentEdge.Data.FirstNodeOfEdge.NodeData);
            }
        }

        //-----------------------------------------------------------------------------------------
        public void AddNodes(params T[] datas)
        {
            foreach (var data in datas)
                AddSingleNode(data);
        }

        //-----------------------------------------------------------------------------------------
        public void RemoveEdges(params T[] datas)
        {
            foreach (var data in datas)
                RemoveSingleNode(data);
        }
        public void RemoveNodes(bool removeEdges, params T[] datas)
        {
            foreach (var data in datas)
                RemoveSingleNode(data, removeEdges);
        }

        //-----------------------------------------------------------------------------------------
        public void AddEdge(T firstLocData, int data, T secondLocData)
        {
            var (FirstLoc, SecondLoc) = CheckNodes(firstLocData, secondLocData);
            CheckIfEdgeAlreadyExist(FirstLoc, SecondLoc);

            var edge = new Edge<T>(FirstLoc, data, SecondLoc);
            FirstLoc.Edges.Add(edge);
            SecondLoc.Edges.Add(edge);

            AllEdges.Add(edge);
        }

        //-----------------------------------------------------------------------------------------
        public void RemoveEdge(T firstLoc, T secondLoc)
        {
            var (FirstLoc, SecondLoc) = CheckNodes(firstLoc, secondLoc);

            var pointerFirst = FirstLoc.Edges.First;

            while (pointerFirst != null)
            {
                var pointerSecond = SecondLoc.Edges.First;

                while (pointerSecond != null)
                {
                    if (pointerFirst.Data.Equals(pointerSecond.Data))
                    {
                        FirstLoc.Edges.Remove(pointerFirst.Data);
                        SecondLoc.Edges.Remove(pointerSecond.Data);
                        return;
                    }
                    pointerSecond = pointerSecond.Next;
                }
                pointerFirst = pointerFirst.Next;
            }

            throw new Exception("Theres no connection that could be removed!");
        }


        public (List<NodeG<T>>, T) FindConnection(T firstLoc, T secondLoc, int pathData = default)
        {
            List<NodeG<T>> paths;

            var Pathdata = pathData;

            var (FirstLoc, SecondLoc) = CheckNodes(firstLoc, secondLoc);

            //-----------------------------------------
            var path = new List<NodeG<T>>();

            var currentNode = FirstLoc;
            var temp = currentNode.Edges.First;

            while (temp != null)
            {
                if (currentNode.NodeData.Equals(temp.Data.SecondNodeOfEdge.NodeData))
                {
                    path.Add(currentNode);
                }
            }
        }


        //public List<List<T>> FindConnections(T firstLoc, T secondLoc)
        //{
        //    //Todo
        //}


        //-------------Checks if there are any nodes in the graph that match with the data given by the user and return the nodes if so--------------
        (NodeG<T>, NodeG<T>) CheckNodes(T firstData, T secondData)
        {
            NodeG<T> FirstLoc = null;
            NodeG<T> SecondLoc = null;

            var currentNode = NodesInGraph.First;

            while (currentNode != null)
            {
                if (FirstLoc == null && currentNode.Data.NodeData.Equals(firstData))
                    FirstLoc = currentNode.Data;

                if (SecondLoc == null && currentNode.Data.NodeData.Equals(secondData))
                    SecondLoc = currentNode.Data;

                currentNode = currentNode.Next;
            }
            if (FirstLoc == null || SecondLoc == null)
                throw new Exception("One or both values couldn't be found!");

            return (FirstLoc, SecondLoc);
        }

        //-----------------------------Checks if the edges already exist------------------------------------
        void CheckIfEdgeAlreadyExist(NodeG<T> firstLoc, NodeG<T> secondLoc)
        {
            var currentEdgeOfFirstLoc = firstLoc.Edges.First;

            while (currentEdgeOfFirstLoc != null)
            {
                var currentEdgeOfSecondLoc = secondLoc.Edges.First;

                while (currentEdgeOfSecondLoc != null)
                {
                    if (currentEdgeOfFirstLoc.Data.Equals(currentEdgeOfSecondLoc))
                    {
                        throw new Exception("Connection already exists!");
                    }
                    currentEdgeOfSecondLoc = currentEdgeOfSecondLoc.Next;
                }
                currentEdgeOfFirstLoc = currentEdgeOfFirstLoc.Next;
            }
        }

        //-----------------------------------------------------------------------------------------
        NodeG<T> FindNode(T inputData)
        {
            var node = NodesInGraph.Find(x => x.NodeData.Equals(inputData));

            if (node != null)
                return node;

            throw new Exception("Value doesn't exist!");
        }

        public List<Edge<T>> GetAllEdges()
        {
            //var allEdges = new List<Edge<T>>();

            //var currentNode = NodesInGraph.First;

            //while (currentNode != null && currentNode.Data.Edges != null)
            //{
            //    var currentEdge = currentNode.Data.Edges.First;

            //    while (currentEdge != null)
            //    {
            //        if (!allEdges.Exists(x => x.Equals(currentEdge.Data)))
            //        {
            //            allEdges.Add(currentEdge.Data);
            //        }
            //        currentEdge = currentEdge.Next;
            //    }
            //    currentNode = currentNode.Next;
            //}
            return AllEdges;
        }
    }
}
