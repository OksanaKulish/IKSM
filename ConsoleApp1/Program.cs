using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LukeSkywalker.IPNetwork;

namespace Vigener
{
    class IKSM
    {
        public static void Main(string[] args)
        {
            string subadd = "";
            string inp = "172.16.6.0/24";
            Console.WriteLine("Input:\t" + inp);
            String[] masc = inp.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine("Ip-address:\t" + masc[0]);
            Console.WriteLine("Maska:\t" + masc[1]);
            Console.WriteLine();
            int[] hosts = new int[] { 25, 25, 25, 12, 6, 2 };

            foreach (int host in hosts)
            {
                Console.WriteLine("For " + host + " hosts");
                subadd = Subnet(host, masc[0]);
                masc[0] = Hosts(subadd, masc[0]);
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        public static string Hosts(string subadd, string masc)
        {
            string res = "";
            IPNetwork ipnetwork = IPNetwork.Parse(subadd);
            Console.Write("Host diapazon:\t" + ipnetwork.FirstUsable + "-");
            Console.WriteLine(ipnetwork.LastUsable);
            string brodcast = ipnetwork.Broadcast.ToString();
            Console.WriteLine("Host broadcast:\t" + brodcast);

            String[] elem = brodcast.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            int s = Convert.ToInt32(elem[3]) + 1;
            res += elem[0] + "." + elem[1] + "." + elem[2] + "." + s;
            return res;
        }

        public static string Subnet(int host, string masc)
        {
            string res = "";
            double bitforhosts = Math.Round(Math.Log(host + 2, 2));
            double newmask = 32 - bitforhosts;
            res = masc + "/" + newmask.ToString();
            Console.WriteLine("Subnet address:\t" + res);
            Console.WriteLine("Number of available hosts:\t" + (Math.Pow(2, bitforhosts) - 2));
            return res;
        }
    }
}
