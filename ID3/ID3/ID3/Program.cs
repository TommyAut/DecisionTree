using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ID3
{
    class Program
    {
        static void Main(string[] args)
        {
            ID3Tree TreePlay = new ID3Tree("ToPlayOrNotToPlay.csv");
            TreePlay.BuildTree();
            TreePlay.PrintTree();
            Console.WriteLine();
            Console.WriteLine();
            ID3Tree TreeParty = new ID3Tree("PartyOrNot.csv");
            TreeParty.BuildTree();
            TreeParty.PrintTree();
            Console.ReadLine();

            /*
            Console.WriteLine(data.CalcMainEntropy());
            double x = (double)5 / (double)14;
            Console.WriteLine(x*data.CalcEntropyFor(0, "sunny"));
            Console.WriteLine(data.CalcGainFor(0));
            Console.WriteLine(data.GetHighestGain());
            */
        }

        /*
         * only ready .csv data
        */
        static List<string[]> Readdata(string filename)
        {
            List<string[]> ldata = new List<string[]>();
            try
            {
                if (!File.Exists(filename))
                {
                    throw new ArgumentException("File does not exist");
                }
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] linesplit = line.Split(',');
                        ldata.Add(linesplit);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return ldata;
        }
    }
}
