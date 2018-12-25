using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ID3
{
    class ID3Data
    {
        public List<string[]> Data { get; set; }
        public string[] headers { get; set; }

        public ID3Data()
        {
            Data = new List<string[]>();
        }

        /*
         * only ready .csv data
        */
        public void Readdata(string Filename)
        {
            char delimiter = ',';
            try
            {
                if (!File.Exists(Filename))
                {
                    throw new ArgumentException("File does not exist");
                }
                using (StreamReader sr = new StreamReader(Filename))
                {
                    string headline = sr.ReadLine();
                    if (headline.Contains(','))
                        delimiter = ',';
                    else if (headline.Contains(';'))
                        delimiter = ';';

                    headers = headline.Split(delimiter);
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] linesplit = line.Split(delimiter);
                        Data.Add(linesplit);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public double CalcMainEntropy()
        {
            Dictionary<string, double> Classifiers = new Dictionary<string, double>();
            double max = Data.Count;

            foreach (string[] row in Data)
            {
                if (Classifiers.ContainsKey(row[row.Length - 1]))
                {
                    Classifiers[row[row.Length - 1]]++;
                }
                else
                {
                    Classifiers.Add(row[row.Length - 1], 1);
                }
            }

            double sum = 0;

            foreach (KeyValuePair<string, double> kvp in Classifiers)
            {
                sum = sum + -(kvp.Value / max) * Math.Log((kvp.Value / max), 2);
            }
            return sum;
        }

        private double GetAmountOfOfApperancesFor(int HeaderPosition, string name)
        {
            double max = 0;
            foreach (string[] row in Data)
            {
                if (row[HeaderPosition] == name)
                {
                    max++;
                }
            }

            return max;
        }

        private Dictionary<string, Dictionary<string, int>> ProcessDataFor2(int HeaderPosition, string name)
        {
            Dictionary<string, Dictionary<string, int>> ProcessData = new Dictionary<string, Dictionary<string, int>>();

            foreach (string[] row in Data)
            {
                if (!ProcessData.ContainsKey(row[HeaderPosition]))
                {
                    ProcessData.Add(row[HeaderPosition], new Dictionary<string, int>());
                    ProcessData[row[HeaderPosition]].Add(row[row.Length - 1], 1);
                }
                else
                {
                    if (ProcessData[row[HeaderPosition]].ContainsKey(row[row.Length - 1]))
                    {
                        ProcessData[row[HeaderPosition]].Add(row[row.Length - 1], 1);
                    }
                    else
                    {
                        ProcessData[row[HeaderPosition]][row[row.Length - 1]]++;
                    }
                }
            }

            return ProcessData;
        }

        private Dictionary<string, double> ProcessDataFor(int HeaderPosition, string name)
        {
            Dictionary<string, double> ProcessData = new Dictionary<string, double>();

            foreach(string[] row in Data)
            {
                if(row[HeaderPosition] == name)
                {
                    if(ProcessData.ContainsKey(row[row.Length - 1]))
                    {
                        ProcessData[row[row.Length - 1]]++;
                    }
                    else
                    {
                        ProcessData.Add(row[row.Length - 1], 1);
                    }
                }
            }
            return ProcessData;
        }

        public double CalcEntropyFor(int HeaderPosition, string name)
        {
            Dictionary<string, double> ProcessedData = ProcessDataFor(HeaderPosition, name);

            double sum = 0;
            double max = GetAmountOfOfApperancesFor(HeaderPosition, name);

            foreach(KeyValuePair<string,double> kvp in ProcessedData)
            {
                sum = sum + -(kvp.Value / max) * Math.Log((kvp.Value / max), 2);
            }

            return sum;
        }

        public double CalcEntropyFor2(int HeaderPosition,string name)
        {
            Dictionary<string,Dictionary<string,int>> ProcessedData = ProcessDataFor2(HeaderPosition, name);

            double sum = 0;

            foreach (KeyValuePair<string, Dictionary<string, int>> StringDictionary in ProcessedData)
            {
                double max = GetAmountOfOfApperancesFor(HeaderPosition, StringDictionary.Key);
                foreach (KeyValuePair<string, int> kvp in StringDictionary.Value)
                {
                    sum = sum + -(kvp.Value / max) * Math.Log((kvp.Value / max), 2);
                }
            }
            return sum;
        }

        public double CalcGainFor(int HeaderPosition)
        {
            double sum = 0;
            List<string> names = new List<string>();

            foreach(string[] row in Data)
            {
                if(!names.Contains(row[HeaderPosition]))
                {
                    names.Add(row[HeaderPosition]);
                }
            }

            foreach(string name in names)
            {
                sum = sum + (GetAmountOfOfApperancesFor(HeaderPosition, name) / Data.Count) * CalcEntropyFor(HeaderPosition, name);
            }

            return (CalcMainEntropy() - sum);
        }

        public string GetHighestGain()
        {
            string highest = "";
            double max = 0;

            for(int i = 0; i < headers.Length-1;i++)
            {
                double x = CalcGainFor(i);
                if(max < x)
                {
                    max = x;
                    highest = headers[i];
                }
            }

            return highest;
        }

    }
}
