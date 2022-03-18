using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScanWorksAPILib;
using System.Net.Sockets;
using SimpleTCP;

namespace SwInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            string UserOption
            SimpleTcpServer ServerObject;

            
            if (args[0].Contains("LaunchTCP"))
            {

            }
            else
            {
                Console.WriteLine("Enter your Option: ")
                while()
            }






        }

        // Start the TCP Server listener
        private static bool StartServer(SimpleTcpServer ServerObject)
        {
            // TCP Server only on local looopback (To enable TestStand/Interprocess communication)
            byte[] ipAddressBinary = { 127, 0, 0, 1 };
            System.Net.IPAddress ip = new System.Net.IPAddress(ipAddressBinary);
            // Listen on Localhost port #13085 
            try
            {
                ServerObject.Start(ip, 13085);
                return true;
            }
            catch (SocketException)
            {
                // MessageBox.Show("Error, Socket Port in use", "Socket Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Unable to Start TCP Server");
                return false;

            }

        }


    }
}
