using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;

namespace Assignment1_A_Star
{
    class Program
    {
        static string[][] Goal { get; set; }
        static List<Node> Opened;
        static List<Node> Closed;
        static List<Node> Path;

        public class Node
        {
            public string[][] Value { get; set; }
            public int HeuristicValue { get; set; }
            public int Level { get; set; }
            public int FScore { get; set; }
            public Node(string[][] value, int level)
            {
                Value = value;
                Level = level;

                HeuristicValue = 0;
                for (var i = 0; i < Value.Length; i++)
                {
                    for (var j=0;j<Value[i].Length;j++)
                    {
                        if (Value[i][j]!="_" && Value[i][j]!=Goal[i][j])
                        {
                            HeuristicValue+=1;
                        }
                    }
                }
                FScore = HeuristicValue + Level;
            }
            public void GetBlankSpace(out int x, out int y)
            {
                x = Value.Length;
                y = Value[0].Length;
                for (int i=0; i< Value.Length;i++)
                {
                    for (int j=0;j< Value[i].Length;j++)
                    {
                        if (Value[i][j]=="_")
                        {
                            x = i;
                            y = j;
                            return;
                        }
                    }
                }
            }
            public void GenerateChildren()
            {
                int x, y;
                GetBlankSpace(out x,out y);
                var PossibleMoves = new List<Tuple<int, int>>
                {
                  new Tuple<int, int>(x-1,y),
                  new Tuple<int, int>(x+1,y),
                  new Tuple<int, int>(x,y-1),
                  new Tuple<int, int>(x,y+1),
                };
            }
            
        }

        static string[][] GetState()
        {

            var i = 0;
            var state = new string[3][];
            try
            {

            
            while (i < 3)
            {
                var row = Console.ReadLine().Split(" ").ToArray();
                if (row.Count() != 3)
                {
                    throw new Exception("Invalid Input");
                }
                state[i] = row;
                i+=1;
            }
                return state;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            return null;
        }

        

        static void Main(string[] args)
        {
            //initState,Final state, heuristic fnction: no. of out of position tiles
            Console.WriteLine("Enter the inital state");
            string[][] InitialState = GetState();
            Console.WriteLine("Enter the Goal State");
            Goal = GetState();

            var root = new Node(InitialState, 0);



            Console.WriteLine("Hello World!");
        }
    }
}
