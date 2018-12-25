using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ID3
{
    class ID3Tree
    {
        ID3Node Head;
        ID3Data Data;

        public ID3Tree(string filename)
        {
            Data = new ID3Data();
            Data.Readdata(filename);
        }

        /*
        private void BuildDataForHead(string HeadName)
        {
            SetHeadersForHead(HeadName);
            int HeaderPosition = GetHeaderPosition(HeadName);
            SetDataForHead(HeaderPosition);
        }

        private void SetDataForHead(int HeaderPosition)
        {
            foreach (string[] row in Data.Data)
            {
                Data.Data.Add(SetRowInDataForHead(HeaderPosition, row));
            }
        }

        private string[] SetRowInDataForHead(int HeadPosition, string[] row)
        {
            string[] NewRow = new string[row.Length - 1];
            int j = 0;
            for(int i = 0; i<row.Length;i++)
            {
                if(i != HeadPosition)
                {
                    NewRow[j] = row[i];
                }
            }
            return NewRow;
        }

        private int GetHeaderPosition(string HeadName)
        {
            for(int i = 0; i < Data.headers.Length; i++)
            {
                if (HeadName == Data.headers[i])
                    return i;
            }
            return -1;
        }

        private void SetHeadersForHead(string HeadName)
        {
            string[] NewHeaders = new string[Data.headers.Length-1];
            int i = 0;
            foreach(string name in Data.headers)
            {
                if(name != HeadName)
                {
                    NewHeaders[i] = name;
                    i++;
                }
            }
            Head.Data.headers = NewHeaders;
        }
        */

        public void BuildTree()
        {
            Head = new ID3Node();
            Head.Data = Data;
            Head.BuildTree();
        }

        public void PrintTree()
        {
            Head.PrintNode(0);
        }

    }
}
