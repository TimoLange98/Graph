using System;

namespace Graph
{
    public static class Program
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

            

            var ways = testGraph.FindConnections("Westend", "Friedrichstraße");

            var help = ways.First;
            while (help != null)
            {
                Print(help.Data);
                Console.WriteLine();
                help = help.Next;
            }
            //Console.Write(bestWay.Item2);
        }

        static void Print(List<NodeG<object>> list)
        {
            var help = list.First;
            while (help != null)
            {
                Console.Write(help.Data.NodeData + " -> ");
                help = help.Next;
            }
        }
    }
}
