using System;

namespace Graph
{
    public class Program
    {
        static void Main(string[] args)
        {
            Graph<object> testGraph = new Graph<object>();

            testGraph.AddNodes("Hauptbahnhof", "Ostkreuz", "Friedrichstraße", "Westend", "Schöneberg", "Wedding", "Alexanderplatz", "Gesundbrunnen");

            testGraph.AddEdge("Hauptbahnhof", 4, "Wedding");

            testGraph.AddEdge("Hauptbahnhof", 3, "Alexanderplatz");
            testGraph.AddEdge("Hauptbahnhof", 5, "Gesundbrunnen");
            testGraph.AddEdge("Hauptbahnhof", 6, "Westend");

            testGraph.AddEdge("Wedding", 2, "Gesundbrunnen");

            testGraph.AddEdge("Westend", 7, "Alexanderplatz");
            testGraph.AddEdge("Westend", 5, "Schöneberg");

            testGraph.AddEdge("Alexanderplatz", 4, "Friedrichstraße");

            testGraph.AddEdge("Schöneberg", 6, "Friedrichstraße");

            testGraph.AddEdge("Gesundbrunnen", 10, "Ostkreuz");

            testGraph.AddEdge("Ostkreuz", 15, "Schöneberg");

            testGraph.NodesInGraph.PrintToConsole();
            Console.WriteLine();


            var edges = testGraph.GetAllEdges();

            var currentEdge = edges.First;

            while (currentEdge != null)
            {
                Console.WriteLine($"{currentEdge.Data.FirstNodeOfEdge};  {currentEdge.Data.EdgeData};  {currentEdge.Data.SecondNodeOfEdge}");

                currentEdge = currentEdge.Next;
            }
            Console.WriteLine();

            //////-----------------------Removing a node...------------------------------------------------
            //testGraph.RemoveNodes(true, "Hauptbahnhof");

            //testGraph.NodesInGraph.PrintToConsole();

            //Console.WriteLine();

            //edges = testGraph.GetAllEdges();

            //currentEdge = edges.First;

            //while (currentEdge != null)
            //{
            //    Console.WriteLine($"{currentEdge.Data.FirstNodeOfEdge};  {currentEdge.Data.EdgeData};  {currentEdge.Data.SecondNodeOfEdge}");

            //    currentEdge = currentEdge.Next;
            //}



        }
    }
}
