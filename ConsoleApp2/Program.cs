using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using LukeSkywalker.IPNetwork;

namespace IKSM
{
    class Program
    {
        static void Main(string[] args)
        {
            Host host1 = new Host(1, 0, 1);
            Host host2 = new Host(1, 0, 8);
            Host host3 = new Host(2, 2, 8);
            Host host4 = new Host(3, 6, 6);
            Host host5 = new Host(3, 10, 1);
            Host host6 = new Host(3, 9, 1);
        }
    }

    public class Host
    {
        int[] host_used = new int[2046];
        int area, floor, room;

        public string[] addr;
        string subnet = "";
        string hosttip = "";
        string hostRange;
        string subnetBroadcast = "";
        string[] ip_bitss;
        int[] ip_bits;
        public Host(int area, int floor, int room)
        {
            this.area = area;
            this.floor = floor;
            this.room = room;
            string base_ip = "192.168.0.0/16";
            string[] ip_mask = base_ip.Split('/');
            string ip = ip_mask[0];
            string mask = ip_mask[1];
            string[] ip_okts = ip.Split('.');
            ip_bits = new int[4];
            ip_bitss = new string[4];
            for (int i = 0; i < ip_okts.Length; i++)
            {
                ip_bits[i] = Convert.ToInt32(ip_okts[i]);
                Console.Write(ip_bits[i] + ".");
            }
            Console.WriteLine();
            for (int i = 0; i < ip_okts.Length; i++)
            {
                ip_bitss[i] = Convert.ToString(ip_bits[i], 2);
                Console.Write(ip_bitss[i] + ".");
            }
            Console.ReadKey();
            // For subnet address formation
            string[] areas = { "00", "01", "10" };

            string[] floors2 = { "00", "01", "10" };
            string[] floors3 = { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
            string[] rooms = { "000", "001", "010", "011", "100", "110", "111" };

            //Subnet
            addr[0] = ip_bitss[0];
            addr[1] = ip_bitss[1];
            string okt = "";
            string okt3;
            string okt4;
            okt += areas[area - 1];
            if (area == 2)
                okt += floors2[floor - 1];
            if (area == 3)
                okt += floors3[floor - 1];
            okt += rooms[room - 1];
            string MASK = mask + okt.Length;

            if (okt.Length > 8)
            {
                okt3 = okt.Substring(8);
                okt4 = okt.Substring(0, 8);
                while (okt.Length < 8)
                    okt4 += "0";
                addr[2] = okt3;
                addr[3] = okt4;
            }

            if (okt.Length < 8)
            {
                while (okt.Length < 8)
                    okt += "0";
                addr[2] = okt;
                addr[3] = "00000000";
            }
            string addr_str = "";
            for (int i = 0; i < 4; i++)
            {
                addr[i] = Convert.ToString(Convert.ToInt32(addr[i]), 2);
                addr_str += addr[i];
                if (i != 3)
                    addr_str += ".";
            }
            addr_str += "/" + MASK;
            subnet = addr_str;
            this.Show();
        }

        public bool ip_check(int address)
        {
            if (host_used.Length == 0)
                return true;
            for (int i = 0; i < host_used.Length; i++)
                if (host_used[i] == address) return false;
            return true;
            DisplayAddresses();
        }
        public void Show()
        {
            Console.Write("\nHost in Area: {0} \nFloor: {1} \nRoom: {2}, \nSubnet: {3} \nHost IP: {4}" +
                "\nHostRange for current subnet: {4}, \nSubnet broadcast: {6}", area, floor, room, subnet, hosttip, subnetBroadcast);
        }
        public static void DisplayAddresses()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {

                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                System.Net.NetworkInformation.IPAddressCollection Servers = adapterProperties.DnsAddresses;
                if (Servers.Count > 0)
                {
                    Console.WriteLine(adapter.Description);
                    foreach (IPAddress dns in Servers)
                    {
                        Console.WriteLine("Servers ............................. : {0}",
                            dns.ToString());
                    }
                    Console.WriteLine();
                }
            }
        }
    }

}
