using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3
{
    class Likleshoodtable
    {
        public ID3Data data { get; set; }
        public Dictionary<string, Dictionary<string, double[]>> table {get ; set;}

        public List<string> Classifiers { get; set; }

        public Likleshoodtable(string filename)
        {
            data = new ID3Data();
            data.Readdata(filename);
            table = new Dictionary<string, Dictionary<string, double[]>>();
        }

        private void BuildClassifiers()
        {
            foreach(string[] row in data.Data)
            {
                if(!Classifiers.Contains(row[row.Length-1]))
                {
                    Classifiers.Add(row[row.Length - 1]);
                }
            }
        }

        private void InitTable()
        {
            int i = 0;

            foreach (string head in data.headers)
            {
                table.Add(head, new Dictionary<string, double[]>());
                foreach (string[] row in data.Data)
                {
                    foreach (string item in row)
                    {
                        if (!table[head].ContainsKey(item))
                        {
                            table[head].Add(item, new double[Classifiers.Count + 1]);
                        }
                    }
                }
            }
        }

        public void BuildTable()
        {
            BuildClassifiers();
            InitTable();



        }


    }
}
