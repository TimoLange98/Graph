using System;
using System.Text;
//using System.Linq;

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
                throw new Exception("There is at least still one connection refering to this Element");
            }

            else if (node.Edges == null)
            {
                NodesInGraph.Remove(node);
                return;
            }

            var currentEdge = node.Edges.First;

            while (currentEdge != null)
            {
                if (AllEdges.Exists(x => x.FirstNodeOfEdge.NodeData.Equals(currentEdge.Data.FirstNodeOfEdge.NodeData) && x.SecondNodeOfEdge.NodeData.Equals(currentEdge.Data.SecondNodeOfEdge.NodeData)))
                    RemoveEdge(currentEdge.Data.FirstNodeOfEdge.NodeData, currentEdge.Data.SecondNodeOfEdge.NodeData);
                else
                    RemoveEdge(currentEdge.Data.SecondNodeOfEdge.NodeData, currentEdge.Data.FirstNodeOfEdge.NodeData);

                currentEdge = currentEdge.Next;
            }
            NodesInGraph.Remove(node);
        }

        //-----------------------------------------------------------------------------------------
        public void AddNodes(params T[] datas)
        {
            foreach (var data in datas)
                AddSingleNode(data);
        }

        //-----------------------------------------------------------------------------------------
        //public void RemoveEdges(params T[] datas)
        //{
        //    foreach (var data in datas)
        //        RemoveEdge(data);
        //}
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

            var remove = GetEdge(firstLoc, secondLoc);

            AllEdges.Remove(remove);
            FirstLoc.Edges.Remove(remove);
            SecondLoc.Edges.Remove(remove);

            //throw new Exception("Theres no connection that could be removed!");
        }


        public (List<T>, int) FindConnection(T firstLoc, T secondLoc)
        {
            var ways = new List<List<NodeG<T>>>();
            var firstL = new List<NodeG<T>>();
            firstL.Add(FindNode(firstLoc));

            ways.Add(firstL);

            var newWays = ways;
            var foundNewWay = true;

            while (foundNewWay)
            {
                var currentWay = newWays.First;

                while (currentWay !=null)
                {
                    var tempNewWays = CheckForWays(currentWay.Data);
                        ways.AddRange(tempNewWays);

                    currentWay = currentWay.Next;
                }
                
            }
        }

        List<List<NodeG<T>>> CheckForWays(List<NodeG<T>> oldList)
        {
            var result = new List<List<NodeG<T>>>();

            var lastNode = oldList.Last;

            var edges = lastNode.Data.Edges;

            var allNodes = new List<NodeG<T>>();

            var currentEdge = edges.First;

            while (currentEdge != null)
            {
                if (!oldList.Exists(x => x.Equals(currentEdge.Data.FirstNodeOfEdge)) && !allNodes.Exists(x => x.Equals(currentEdge.Data.FirstNodeOfEdge)))
                    allNodes.Add(currentEdge.Data.FirstNodeOfEdge);

                if (!oldList.Exists(x => x.Equals(currentEdge.Data.SecondNodeOfEdge)) && !allNodes.Exists(x => x.Equals(currentEdge.Data.SecondNodeOfEdge)))
                    allNodes.Add(currentEdge.Data.SecondNodeOfEdge);

                currentEdge = currentEdge.Next;
            }

            var currentNode = allNodes.First;
            while (currentNode != null)
            {
                var temp = new List<NodeG<T>>();

                //------Liste in temp kopieren-------------
                var currentNodeInOldList = oldList.First;
                while (currentNodeInOldList != null)
                {
                    temp.Add(currentNodeInOldList.Data);
                    currentNodeInOldList = currentNodeInOldList.Next;
                }
                //-----------------------------------------
                temp.Add(currentNode.Data);
                result.Add(temp);
            }
            return result;
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
            return AllEdges;
        }

        Edge<T> GetEdge(T firstLoc, T secondLoc)
        {
            return AllEdges.Find(x => x.FirstNodeOfEdge.NodeData.Equals(firstLoc) && x.SecondNodeOfEdge.NodeData.Equals(secondLoc));
        }
    }
}
