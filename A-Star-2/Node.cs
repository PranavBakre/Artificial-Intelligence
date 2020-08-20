using System;
using System.Collections.Generic;
using System.Text;

namespace A_Star_2
{
    partial class Program
    {
        public class Node
        {
            private string[,] _value;
            private int _heuristicValue;

            public Node(int level,string[,] value)
            {
                Level = level;
                Value = value;
            }

            public Node(int level, string[,] value,Node parent)
            {
                Level = level;
                Value = value;
                Parent = parent;
            }


            public string[,] Value
            {
                get => _value;
                set
                {
                    _value = value;

                    var h = 0;
                    for (var i = 0; i < _value.GetLength(0); i++)
                    {

                        for (var j = 0; j < _value.GetLength(1); j++)
                        {
                            if (_value[i, j] != "_" && _value[i, j] != Goal[i, j])
                            {
                                h++;
                            }
                        }
                    }
                    _heuristicValue = h;
                }
                

            }

            public int Level { get; }
            public Node Parent { get; set; }
            public int FScore => Level + _heuristicValue;


            public Tuple<int,int> GetBlankSpace()
            {
                for(int i =0;i<_value.GetLength(0);i++)
                {
                    for ( int j=0;j<_value.GetLength(1);j++)
                    {
                        if (_value[i, j].Equals("_"))
                        {
                            return new Tuple<int, int>(i,j);
                        }
                    }
                }
                return null;
            }

            public IEnumerable<Node> GenerateChildren()
            {
                Tuple<int,int> blank = GetBlankSpace();
                int x = blank.Item1;
                int y = blank.Item2;

                var possibilities = new List<Tuple<int, int>>()
                {
                    new Tuple<int, int>(x-1,y),
                    new Tuple<int, int>(x+1,y),
                    new Tuple<int, int>(x,y-1),
                    new Tuple<int, int>(x,y+1)
                    
                };

                foreach (var node in possibilities)
                {
                    bool condX = node.Item1 >= 0 && node.Item1 < _value.GetLength(0);
                    bool condY = node.Item2 >=0 && node.Item2 < _value.GetLength(1);
                    if (condX && condY)
                    {
                        var childValue =Value.Clone() as string[,];
                        childValue[x, y] = childValue[node.Item1, node.Item2];
                        childValue[node.Item1, node.Item2] = "_";
                        yield return new Node(Level+1,childValue, this);
                    }
                }

            }

            public bool Equals(Node obj)
            {
                if (_value.GetLength(0) != obj.Value.GetLength(0))
                {
                    return false;
                }
                if (_value.GetLength(1) != obj.Value.GetLength(1))
                {
                    return false;
                }
                for(int i = 0;i<_value.GetLength(0);i++)
                {
                    for(int j=0;j<_value.GetLength(1);j++)
                    {
                        if (!_value[i,j].Equals(obj.Value[i,j]))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public List<Node> GetPath()
            {
                var temp = this;
                var path = new List<Node>();
                path.Add(temp);
                while (temp.Parent != null)
                {
                    temp = temp.Parent;
                    path.Add(temp);
                }
                path.Reverse();
                return path;
            }

            public override string ToString()
            {
                StringBuilder s = new StringBuilder();
                
                for (int i = 0; i < _value.GetLength(0); i++)
                {
                    s.Append("[");
                    for (int j = 0; j < _value.GetLength(1); j++)
                    {
                        s.Append($"\t{_value[i, j]}\t");
                    }
                    s.Append("]\n");
                }
                return s.ToString();
            }
        }
    }
}