using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Algo
{
    public static class Dijkstra
    {
        //NOTE: Maxes set lower than real max to ignore/avoid overflow...
        public static readonly int max = Int16.MaxValue;
        public static readonly int min = Int16.MinValue;

        public static void Main()
        {
            //Weighted node matrix (max sentinel values)
            DijkstraAlgo<int, WeightedNode> algo = new DijkstraAlgo<int, WeightedNode>(
                new List<List<int>>()
                {
                    new List<int>(){ max,   1,   6, max },
                    new List<int>(){   1, max,   3,   5 },
                    new List<int>(){   6,   3, max,   1 },
                    new List<int>(){ max,   5,   1, max }
                });
            //Build initial traversal list
            foreach(WeightedNode item in algo.UnvisitedNodes)
            {
                //TODO: Copy instead of referencing item here?
                //var newItem = new WeightedNode(item);
                //newItem.Weight = max;
                //algo.Traversal.Add(newItem);
                algo.Traversal.Add(item);
            }
            //First iteration (start node)
            var nodeId = 0;
            algo.Traversal.Find(n => n.Id==nodeId)!.Weight = 0;
            //Compare each traversal weight to existing path weights in matrix
            for(int i=0; i < algo.Traversal.Count; i++)
            {
                int pathWeight = algo.Vertices.Data[nodeId][i];
                //If the weight is less than what's stored, set it in traversal data
                if (pathWeight < algo.Traversal[i].Weight)
                {
                    algo.Traversal[i].Weight = pathWeight;
                }
            }
            //Move 1st processed node to Visited list, remove from Unvisited
            var firstNode = algo.UnvisitedNodes.Find(n => n.Id == nodeId);
            if(firstNode == null)
            {
                throw new NullReferenceException(Node.NodeError);
            }

            algo.UnvisitedNodes.Remove(firstNode);
            algo.VisitedNodes.Add(firstNode);

            //We will consume all unvisited nodes traversing the graph
            while(algo.UnvisitedNodes.Count > 0)
            {
                //Find next smallest unvisited node
                var minWeightNode = algo.UnvisitedNodes.Find(x => x.Weight == algo.UnvisitedNodes.Min(n => n.Weight));
                if(minWeightNode == null)
                {
                    throw new NullReferenceException(Node.NodeError);
                }

                //Check path weights between this node and any other unvisited nodes
                foreach (WeightedNode unvisited in algo.UnvisitedNodes)
                {
                    //Path from current node to potential unvisited
                    int pathToUnvisitedWeight = algo.Vertices.Data[minWeightNode.Id][unvisited.Id];

                    //If the weight of this traversal between node->node is lower, save new
                    var unvisitedTraversal = algo.Traversal.Find(x => x.Id == unvisited.Id);
                    var minWeightTraversal = algo.Traversal.Find(x => x.Id == minWeightNode.Id);

                    //Current traversal weight plus the weight from potential path
                    var newWeight = pathToUnvisitedWeight + (minWeightTraversal != null ? minWeightTraversal.Weight : max);
                    
                    //Check whether the new potential path weight is better than existing
                    if (newWeight < (unvisitedTraversal != null ? unvisitedTraversal.Weight : min))
                    {
                        unvisitedTraversal!.Weight = newWeight;
                    }
                    else
                    {
                        ; // Nothing to do, existing path weight is lower
                    }
                }

                algo.UnvisitedNodes.Remove(minWeightNode);
                algo.VisitedNodes.Add(minWeightNode);
            }

            algo.Log();
        }
    }

    public class Matrix<T>
    {
        public List<List<T>> Data {  get; set; }

        public Matrix(List<List<T>> data)
        {
            Data = data;
        }
    }

    public class DijkstraAlgo<TData, TNode> where TNode : Node, new()
    {
        public List<TNode> UnvisitedNodes { get; set; } = new List<TNode>();
        public List<TNode> VisitedNodes { get; set; } = new List<TNode>();
        public List<TNode> Traversal { get; set; } = new List<TNode>();

        public Matrix<TData> Vertices { get; set; }

        public DijkstraAlgo(Matrix<TData> vertices)
        {
            Vertices = vertices;
            int index = 0;
            foreach (var v in Vertices.Data)
            {
                UnvisitedNodes.Add(NodeFromInt(index));
                var numOfNodes = Vertices.Data.Count;
                index++;
            }
        }

        public TNode NodeFromInt(int input)
        {
            var node = new TNode();
            node.Id = input;
            return node;
        }

        public DijkstraAlgo(List<List<TData>> datas) : this(new Matrix<TData>(datas)) { }

        public void Log()
        {
            string jsonString = JsonSerializer.Serialize(this);
            Debug.WriteLine(jsonString);
        }
    }

    public class WeightedNode : Node
    {
        public WeightedNode() { }

        public WeightedNode(WeightedNode item): base(item.Id)
        {
            this.Weight = item.Weight;
        }

        public int Weight { get; set; } = Dijkstra.max;
    }

    public class Node
    {
        public static readonly string NodeError = "Node could not be found, Id math wrong...";
        public int Id { get; set; }
        
        public Node() { }
        public Node(int id) { Id = id; }
    }

    public interface INode
    {
        public int Id {  get; set; }
    }
}
