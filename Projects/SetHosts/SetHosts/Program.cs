using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SetHosts
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {


                DirectoryInfo dir = new DirectoryInfo(@"C:\Windows\System32\drivers\etc");
                FileInfo[] fileInfo = dir.GetFiles();
                foreach (FileInfo file in fileInfo)
                {
                    if (file.Name == "hosts")
                    {
                        StreamWriter stream = file.AppendText();
                        stream.WriteLine("\r \n");
                        stream.WriteLine("129.135.148.30      www.s3dsupport.com");
                        stream.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
