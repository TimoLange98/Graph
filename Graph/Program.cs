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
            testGraph.AddEdge("Schöneberg", 15, "Ostkreuz");

            //testGraph.NodesInGraph.PrintToConsole();
            //Console.WriteLine();


            //var edges = testGraph.GetAllEdges();

            //var currentEdge = edges.First;

            //while (currentEdge != null)
            //{
            //    Console.WriteLine(currentEdge.Data.FirstNodeOfEdge + "" + currentEdge.Data.EdgeData + currentEdge.Data.SecondNodeOfEdge);

            //    currentEdge = currentEdge.Next;
            //}
            //Console.WriteLine();
            //Console.WriteLine();

            //var List = testGraph.FindConnections("Hauptbahnhof", "Ostkreuz");

            //var help = List.First;

            //while (help != null)
            //{
            //    var help2 = help.Data.First;
            //    while (help2 != null)
            //    {
            //        Console.Write(help2.Data.NodeData + " -> ");
            //        help2 = help2.Next;
            //    }
            //    Console.WriteLine();
            //    help = help.Next;
            //}

            //Console.WriteLine();

            var bestWay = testGraph.FindBestConnection("Westend", "Friedrichstraße");

            var currentLoc = bestWay.Item1.First;
            while (currentLoc != null)
            {
                Console.Write(currentLoc.Data.NodeData + " -> ");
                currentLoc = currentLoc.Next;
            }

            Console.Write(bestWay.Item2);
        }
    }
}
