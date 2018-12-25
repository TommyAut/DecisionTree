using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3
{
    class ID3Node
    {
        public string Name { get; set; }
        public ID3Data Data { get; set; }
        private Dictionary<string, ID3Node> Neighbors;
        
        public ID3Node()
        {
            Data = new ID3Data();
            Neighbors = null;
        }

        public void BuildTree()
        {
            if (IsPureDecision())
            {
                Name = Data.Data[0][Data.headers.Length - 1];
                return;
            }
            else if(NoMoreAttributesLeft())
            {
                Name = Data.headers[Data.headers.Length - 1];
                return;
            }

            Name = Data.GetHighestGain();

            int HeaderPosition = GetHeaderPosition(Name);
            CreateNeighbors(HeaderPosition);

            foreach(KeyValuePair<string,ID3Node> KVP in Neighbors)
            {
                CreateDataForNeighbor(HeaderPosition, KVP.Key,KVP.Value);
                KVP.Value.BuildTree();
            }
        }

        public void PrintNode(int depth)
        {
            Console.Write("->" + this.Name);
            Console.WriteLine("");
            if (Neighbors == null)
                return;
            foreach (KeyValuePair<string,ID3Node> Neigbor in Neighbors)
            {
                
                for (int i = 0; i <= depth; i++)
                {
                    Console.Write("|      ");
                }
                Console.Write("->" + Neigbor.Key);
                Neigbor.Value.PrintNode(depth+1);
            }
        }

        private void CreateDataForNeighbor(int HeaderPosition, string name, ID3Node Neighbor)
        {
            SetHeadersForNeighbor(HeaderPosition, Neighbor);

            foreach (string[] row in Data.Data)
            {
                if(row[HeaderPosition] == name)
                {
                    Neighbor.Data.Data.Add(GetRowDataForNeigbor(HeaderPosition, row));
                }
            }
        }

        private string[] GetRowDataForNeigbor(int HeaderPosition,string[] row)
        {
            string[] NewRow = new string[row.Length - 1];
            int j = 0;
            for(int i = 0; i < row.Length; i++)
            {
                if(i != HeaderPosition)
                {
                    NewRow[j] = row[i];
                    j++;
                }
            }

            return NewRow;
        }

        private void SetHeadersForNeighbor(int HeadPosition, ID3Node Neighbor)
        {
            string[] NewHeaders = new string[Data.headers.Length - 1];
            int i = 0;
            foreach (string name in Data.headers)
            {
                if (name != Data.headers[HeadPosition])
                {
                    NewHeaders[i] = name;
                    i++;
                }
            }
            Neighbor.Data.headers = NewHeaders;
        }

        private int GetHeaderPosition(string HeadName)
        {
            for (int i = 0; i < Data.headers.Length; i++)
            {
                if (HeadName == Data.headers[i])
                    return i;
            }
            return -1;
        }

        private bool NoMoreAttributesLeft()
        {
            return Data.headers.Length <= 1;
        }

        private bool IsPureDecision()
        {
            List<string> Uniques = new List<string>();

            foreach (string[] row in Data.Data)
            {
                if (!Uniques.Contains(row[row.Length - 1]))
                {
                    Uniques.Add(row[row.Length - 1]);
                }
            }
            return Uniques.Count <= 1;
        }

        private void CreateNeighbors(int HeaderPosition)
        {
            Neighbors = new Dictionary<string, ID3Node>();
            foreach(string[] row in Data.Data)
            {
                if (!Neighbors.ContainsKey(row[HeaderPosition]))
                {
                    Neighbors.Add(row[HeaderPosition], new ID3Node());
                }
            }
        }

    }
}
