﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace A_Star_2
{

    partial class Program
    {

        static string[,] Goal;
        static List<Node> Open, Closed;


        public static string[,] ReadState()
        {
            var state = new string[3, 3];

            for(var i =0;i<state.GetLength(0);i++)
            {
                string[] row;
                do
                {
                    row = Console.ReadLine().Split(" ");
                } while (row.Length != 3);
                for (var j=0;j<state.GetLength(1);j++)
                {
                    state[i, j] = row[j];
                }
            }


            return state;
        }


        static Node A_Star(Node root)
        {
            var goal = new Node(-1, Goal);
            Open.Add(root);
            while (Open.Count != 0)
            {
                var node = Open.First();
                Closed.Add(node);
                Open.RemoveAt(0);

                if(node.Equals(goal))
                {
                    return node;
                }

                foreach(var child in node.GenerateChildren())
                {
                    var nodeInOpen = Open.Where(x => x.Equals(child)).FirstOrDefault();
                    var nodeInClosed = Closed.Where(x => x.Equals(child)).FirstOrDefault();


                    if (child.Equals(goal))
                    {
                        return child;
                    }

                    if (nodeInOpen != null && nodeInOpen.Level > child.Level)
                    {
                        Open.Remove(nodeInOpen);
                        Open.Add(child);
                    }
                    else if (nodeInClosed != null && nodeInClosed.Level > child.Level)
                    {
                        Closed.Remove(nodeInClosed);
                        Open.Add(child);
                    }
                    else
                    {
                        Open.Add(child);
                    }
                }

                Open= Open.OrderBy(x => x.FScore).ToList();
            }
            return null;
        }

        static void Main(string[] args)
        {
            Open = new List<Node>();
            Closed = new List<Node>();
            Console.WriteLine("Enter the Goal state");
            Goal = ReadState();

            Console.WriteLine("Enter the initial state");
            string[,] init = ReadState();
            var root = new Node(0, init);
            A_Star(root).GetPath().ForEach(x => Console.WriteLine(x));

            
        }
    }
}
