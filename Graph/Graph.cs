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
                    return;

                help = help.Next;
            }
            NodesInGraph.Add(new NodeG<T>(data));
        }

        //-----------------------------------------------------------------------------------------
        void RemoveSingleNode(T data, bool removeEdges = default)
        {
            var node = FindNode(data);

            if (node.Edges != null && removeEdges == false)
                throw new Exception("There is at least still one connection refering to this Element");

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


        public List<List<NodeG<T>>> FindConnections(T firstLoc, T secondLoc)
        {
            var allWays = new List<List<NodeG<T>>>();
            var allEdges = new List<Edge<T>>();

            var firstListOnlyFirstLoc = new List<NodeG<T>>();
            firstListOnlyFirstLoc.Add(FindNode(firstLoc));
            allWays.Add(firstListOnlyFirstLoc);

            var currentWay = allWays.First;

            while (currentWay != null)
            {
                var tempNewWaysAndEdges = CheckForWays(currentWay.Data, allEdges);

                if (tempNewWaysAndEdges.Item1.First == null)
                    break;
                else
                {
                    allWays.AddRange(tempNewWaysAndEdges.Item1);
                    allEdges.AddRange(tempNewWaysAndEdges.Item2);
                    currentWay = currentWay.Next;
                }
            }
            var allWaysWithDest = allWays.FindAll(x => x.Exists(y => y.Equals(FindNode(secondLoc))));

            return allWays;
        }

        (List<List<NodeG<T>>>, List<Edge<T>>) CheckForWays(List<NodeG<T>> oldList, List<Edge<T>> oldEdges)
        {
            var resultList = new List<List<NodeG<T>>>();
            var resultEdges = new List<Edge<T>>();
            var tempEdge = oldEdges.First;
            var allNewNodes = new List<NodeG<T>>();

            var edges = oldList.Last.Data.Edges;

            var currentEdge = edges.First;

            while (currentEdge != null)
            {
                if (!(oldEdges.Exists(x => x.FirstNodeOfEdge.Equals(currentEdge.Data.FirstNodeOfEdge) && x.SecondNodeOfEdge.Equals(currentEdge.Data.SecondNodeOfEdge))))
                {
                    if (oldList.Last.Data.Equals(currentEdge.Data.SecondNodeOfEdge))
                    {
                        allNewNodes.Add(currentEdge.Data.FirstNodeOfEdge);
                        resultEdges.Add(GetEdge(currentEdge.Data.FirstNodeOfEdge.NodeData, currentEdge.Data.SecondNodeOfEdge.NodeData));
                    }
                    else
                    {
                        allNewNodes.Add(currentEdge.Data.SecondNodeOfEdge);
                        resultEdges.Add(GetEdge(currentEdge.Data.FirstNodeOfEdge.NodeData, currentEdge.Data.SecondNodeOfEdge.NodeData));
                    }
                }

                currentEdge = currentEdge.Next;
            }

            var currentNode = allNewNodes.First;

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
                resultList.Add(temp);

                currentNode = currentNode.Next;
            }

            return (resultList, resultEdges);
        }


        public (List<NodeG<T>>, int) FindBestConnection(T firstLoc, T secondLoc)
        {
            var allWays = FindConnections(firstLoc, secondLoc);

            var result = int.MaxValue;
            var currentWay = allWays.First;
            var rememberWay = 0;

            while (currentWay != null)
            {
                var costs = 0;
                var currentLoc = currentWay.Data.First;

                while (currentLoc.Next != null)
                {
                    costs += GetEdge(currentLoc.Data.NodeData, currentLoc.Next.Data.NodeData).EdgeData;
                    currentLoc = currentLoc.Next;
                }

                if (costs < result)
                {
                    result = costs;
                    rememberWay++;
                }
                currentWay = currentWay.Next;
            }

            currentWay = allWays.First;
            var i = 1;
            while (i < rememberWay)
            {
                currentWay = currentWay.Next;
                i++;
            }
            return (currentWay.Data, result);
        }


        //-------------Checks if there are any nodes in the graph that match with the data given by the user and returns the nodes if so--------------
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
            return AllEdges.Find(x => (x.FirstNodeOfEdge.NodeData.Equals(firstLoc) && x.SecondNodeOfEdge.NodeData.Equals(secondLoc)) || (x.FirstNodeOfEdge.NodeData.Equals(secondLoc) && x.SecondNodeOfEdge.NodeData.Equals(firstLoc)));
        }
    }
}
