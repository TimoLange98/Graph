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
            //var (FirstLoc, SecondLoc) = CheckNodes(firstLocData, secondLocData);
            var FirstLoc = FindNode(firstLocData);
            var SecondLoc = FindNode(secondLocData);
            CheckIfEdgeAlreadyExist(FirstLoc, SecondLoc);

            var edge = new Edge<T>(FirstLoc, data, SecondLoc);
            FirstLoc.Edges.Add(edge);
            SecondLoc.Edges.Add(edge);

            AllEdges.Add(edge);
        }

        //-----------------------------------------------------------------------------------------
        public void RemoveEdge(T firstLoc, T secondLoc)
        {
            //var (FirstLoc, SecondLoc) = CheckNodes(firstLoc, secondLoc);

            var FirstLoc = FindNode(firstLoc);
            var SecondLoc = FindNode(secondLoc);

            var toRemove = GetEdge(firstLoc, secondLoc);

            AllEdges.Remove(toRemove);
            FirstLoc.Edges.Remove(toRemove);
            SecondLoc.Edges.Remove(toRemove);

            //throw new Exception("Theres no connection that could be removed!");
        }


        public List<List<NodeG<T>>> FindConnections(T firstLoc, T dest)
        {
            var allWays = new List<List<NodeG<T>>>();

            var firstListOnlyFirstLoc = new List<NodeG<T>>();
            firstListOnlyFirstLoc.Add(FindNode(firstLoc));
            allWays.Add(firstListOnlyFirstLoc);

            var currentWay = allWays.First;

            while (currentWay != null)
            {
                var tempNewWays = CheckForWays(currentWay.Data, dest);

                if (tempNewWays.First != null)
                {
                    allWays.AddRange(tempNewWays);
                    currentWay = currentWay.Next;
                }
                else if (tempNewWays.First == null && currentWay.Next != null)
                    currentWay = currentWay.Next;

                else break;

            }
            var allWaysWithDest = allWays.FindAll(x => x.Exists(y => y.NodeData.Equals(dest)));

            return allWaysWithDest;
        }

        List<List<NodeG<T>>> CheckForWays(List<NodeG<T>> oldList, T dest)
        {
            var resultList = new List<List<NodeG<T>>>();
            var allNewNodes = new List<NodeG<T>>();

            var oldListLast = oldList.Last.Data;

            var currentEdge = oldListLast.Edges.First;

            while (currentEdge != null)
            {
                if (oldListLast.Equals(currentEdge.Data.SecondNodeOfEdge))
                {
                    if (!(oldList.Exists(x => x.Equals(currentEdge.Data.FirstNodeOfEdge)) || oldList.Exists(x => x.NodeData.Equals(dest))))
                        allNewNodes.Add(currentEdge.Data.FirstNodeOfEdge);
                }
                else
                {
                    if (!(oldList.Exists(x => x.Equals(currentEdge.Data.SecondNodeOfEdge)) || oldList.Exists(x => x.NodeData.Equals(dest))))
                        allNewNodes.Add(currentEdge.Data.SecondNodeOfEdge);
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

            return resultList;
        }


        public (List<NodeG<T>>, int) FindBestConnection(T firstLoc, T secondLoc)
        {
            var allWays = FindConnections(firstLoc, secondLoc);

            var minCosts = int.MaxValue;
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

                if (costs < minCosts)
                {
                    minCosts = costs;
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
            return (currentWay.Data, minCosts);
        }


        ////-------------Checks if there are any nodes in the graph that match with the data given by the user and returns the nodes if so--------------
        //(NodeG<T>, NodeG<T>) CheckNodes(T firstData, T secondData)
        //{
        //    NodeG<T> FirstLoc = null;
        //    NodeG<T> SecondLoc = null;

        //    var currentNode = NodesInGraph.First;

        //    while (currentNode != null)
        //    {
        //        if (FirstLoc == null && currentNode.Data.NodeData.Equals(firstData))
        //            FirstLoc = currentNode.Data;

        //        if (SecondLoc == null && currentNode.Data.NodeData.Equals(secondData))
        //            SecondLoc = currentNode.Data;

        //        currentNode = currentNode.Next;
        //    }
        //    if (FirstLoc == null || SecondLoc == null)
        //        throw new Exception("One or both values couldn't be found!");

        //    return (FirstLoc, SecondLoc);
        //}

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
