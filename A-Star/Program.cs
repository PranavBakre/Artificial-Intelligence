using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace A_Star
{
    class Program
    {
        static string[,] Goal { get; set; }
        static List<Node> Open { get; set; }
        static List<Node> Closed { get; set; }
        public class Node
        {
            public string[,] Value { get; set; }
            public int Level { get; set; }
            public int HeuristicValue
            {
                get {
                    var h = 0;
                    for (var i = 0; i < Value.GetLength(0); i++)
                    {
                        
                        for (var j = 0; j < Value.GetLength(1); j++)
                        {
                            if (Value[i, j] != "_" && Value[i, j] != Goal[i, j])
                            {
                                h++;
                            }
                        }
                    }
                    return h;
                }
            }

            public int FScore => Level + HeuristicValue;

            public Node Parent { get; set; }
            public Node(string[,] NodeState, int level)
            {
                Value= (string[,])NodeState.Clone();
                Level = level;
            }

            public void GetBlankPosition(out int? x, out int? y)
            {
                x = null;
                y = null;
                for (int i = 0; i < Value.GetLength(0); i++)
                {
                    for (int j = 0; j < Value.GetLength(1); j++)
                    {
                        if (Value[i, j] == "_")
                        {
                            x = i;
                            y = j;
                            return;
                        }
                    }
                }
            }

            public IEnumerable<Node> GenerateChildren()
            {
                int? x, y;
                GetBlankPosition(out x, out y);

                if (x.HasValue && y.HasValue)
                {
                    int X = x.Value;
                    int Y = y.Value;
                    var PossibleMoves = new List<Tuple<int, int>>
                    {
                        new Tuple<int, int>(X,Y-1),
                        new Tuple<int, int>(X,Y+1),
                        new Tuple<int, int>(X-1,Y),
                        new Tuple<int, int>(X+1,Y),
                        
                    };

                    foreach (var tuple in PossibleMoves)
                    {
                        var ValidX = tuple.Item1 >= 0 && tuple.Item1 < Value.GetLength(0);
                        var ValidY = tuple.Item2 >= 0 && tuple.Item2 < Value.GetLength(1);
                        if (ValidX && ValidY)
                        {
                            var node = new Node(Value, Level + 1);
                            node.Parent = this;
                            var temp = node.Value[X, Y];
                            node.Value[X, Y] = node.Value[tuple.Item1, tuple.Item2];
                            node.Value[tuple.Item1, tuple.Item2] = temp;
                            yield return node;
                        }
                    }
                }
            }

            public IEnumerable<Node> GetPath()
            {
                var temp = this;
                var path = new List<Node>();
                path.Add(temp);
                while(temp.Parent!=null)
                {
                    temp = temp.Parent;
                    path.Add(temp);
                }
                path.Reverse();
                return path;
            }

            public bool IsEqual(Node temp)
            {
                for(int i = 0;i<Value.GetLength(0);i++)
                {
                    for (int j = 0; j<Value.GetLength(1);j++)
                    {
                        if (Value[i,j]!=temp.Value[i,j])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

        }

        /*Function to accept the initial state and goal.
         * Currently hardcoded to take for 3X3 matrix
         */
        static string[,] GetState()
        {
            string[,] state=new string[3,3];
            for (var x=0;x<3;x++)
            {
                var row = Console.ReadLine().Split(" ");
                for (var y = 0; y < 3; y++)
                {
                    state[x, y] = row[y];
                }
            }
            return state;
        }

        /*
         * A-Star Algorithm
         * Open- List of opened nodes
         * Closed- List of closed nodes
         * InOpen: returns a  node if it is present in Open, else null
         * InClosed: returns a  node if it is present in Closed, else null
         */

        static Node AStar(Node root)
        {
            Open.Add(root);
            while (Open.Count!=0)
            {
                var node = Open.First();
                Closed.Add(node);
                Open.RemoveAt(0);

                if (node.HeuristicValue==0)
                {
                    return node;
                }

                foreach ( var child in node.GenerateChildren())
                {
                    var InOpen = Open.Where(x => x.IsEqual(child)).FirstOrDefault();
                    var InClosed = Closed.Where(x => x.IsEqual(child)).FirstOrDefault();
                    if (child.HeuristicValue==0)
                    {
                        return child;
                    }
                    if (InClosed != null && InClosed.Level > child.Level)
                    {
                        Closed.Remove(InClosed);
                        Open.Add(child);
                    }
                    else if (InOpen != null && InOpen.Level > child.Level)
                    {
                        InOpen.Level = child.Level;
                        InOpen.Parent = child.Parent;
                    }
                    else
                    {
                        Open.Add(child);
                    }
                }
                Open=Open.OrderBy(x => x.FScore).ToList();
            }
            return null;
        }

        static void Main(string[] args)
        {
            Open = new List<Node>();
            Closed = new List<Node>();
            Console.WriteLine("Enter the Initial State");
            var InitState = GetState();
            var root = new Node(InitState, 0);
            Console.WriteLine("Enter the Goal State");
            Goal = GetState();
            var result=AStar(root);
            var Path = result.GetPath();


            //Display the moves from Initial state to goal state
            foreach(var node in Path) 
            {
                
                for (var i=0;i< node.Value.GetLength(0); i++)
                {
                    
                    for (var j=0;j< node.Value.GetLength(1); j++)
                    {
                        Console.Write($"{node.Value[i,j]}\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\n\n\n\n");
            }
        }
    }
}
