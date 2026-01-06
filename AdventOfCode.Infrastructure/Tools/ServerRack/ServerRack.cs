using AdventOfCode.Infrastructure.Tools.Laser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace AdventOfCode.Infrastructure.Tools.ServerRack
{
    public static class ServerRackCalculator
    {

        public static Dictionary<string, Server> ServerRack = new Dictionary<string, Server>();

        public static long CalculateYouToOut(List<string> fileContents)
        {
            long result = 0;

            //string StartingPoint = "you";
            string StartingPoint = "svr";


            ServerRack.Add("out", new Server("out", new List<string>()));
            ServerRack["out"].result = new ServerMoveResult { PathCount = 1, ValidPathCount = 0, DACPaths = 0, FFTPaths = 0 };

            foreach (var row in fileContents)
            {
                var colonSplit = row.ToLower().Split(':');

                string name = colonSplit[0];

                List<string> outputs = colonSplit[1].Trim().Split(' ').ToList();


                ServerRack.Add(name, new Server(name, outputs));
            }


            foreach (var output in ServerRack[StartingPoint].Outputs)
            {
                var currResult = ServerMove(output);//, false, false);
                //if (currResult.containsDAC && currResult.containsFFT)
                //{
                result += currResult.ValidPathCount;
                //}
            }


            foreach (var server in ServerRack)
            {
                Console.WriteLine(server.Value.ToString());
            }


            return result;
        }

        public static ServerMoveResult ServerMove(string name/*, bool conatinsDAC, bool containsFFT*/)
        {

            ServerMoveResult result = new ServerMoveResult { PathCount = 0, ValidPathCount = 0, DACPaths = 0, FFTPaths = 0 };

            //if (name == "out")
            //{
            //    //if (conatinsDAC && containsFFT)
            //    //{
            //    //    result.PathCount = 1;
            //    //}


            //    return ServerRack[name].result;
            //}


            if (ServerRack[name].result.PathCount != -1)//includes "out"
            {
                return ServerRack[name].result;
            }

            foreach (var server in ServerRack[name].Outputs)
            {

                var curResult = ServerMove(server);


                result.ValidPathCount += curResult.ValidPathCount;
                result.DACPaths += curResult.DACPaths;
                result.FFTPaths += curResult.FFTPaths;
                result.PathCount += curResult.PathCount;

                if (server == "dac")
                {
                    result.ValidPathCount += curResult.FFTPaths;
                    result.DACPaths += curResult.PathCount;
                }
                else if (server == "fft")
                {
                    result.ValidPathCount += curResult.DACPaths;
                    result.FFTPaths += curResult.PathCount;
                }




            }

            ServerRack[name].result = result;

            return result;
        }

    }
    public class ServerMoveResult
    {
        public long PathCount { get; set; }
        public long DACPaths { get; set; }
        public long FFTPaths { get; set; }
        public long ValidPathCount { get; set; }
        public bool containsDAC
        {
            get
            {
                return DACPaths > 0;
            }
        }
        public bool containsFFT
        {
            get
            {
                return FFTPaths > 0;
            }
        }
    }

    public class Server
    {
        public string Name { get; set; }
        public List<string> Outputs { get; set; }
        //public long PathCount { get; set; }
        public ServerMoveResult result { get; set; }
        public Server()
        {
            result = new ServerMoveResult { PathCount = -1, ValidPathCount = 0, DACPaths = 0, FFTPaths = 0 };
            Name = "";
            Outputs = new List<string>();
        }
        public Server(string Name, List<string> output)
        {
            result = new ServerMoveResult { PathCount = -1, ValidPathCount = 0, DACPaths = 0, FFTPaths = 0 };
            this.Name = Name;
            this.Outputs = output;
        }

        public override string ToString()
        {


            return $"{Name}: {string.Join(' ', Outputs)} - Pathcount: {result.PathCount}";
        }

    }
}
