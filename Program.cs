using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTCP;
using ScanWorksAPILib;
using System.Net.Sockets;

namespace SwTCPInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            string UserOption="none";
            int ApiReturn = -100;

            SimpleTcpServer ServerObject = new SimpleTcpServer
            {
                Delimiter = 0x13, // HEX Value for Enter
                StringEncoder = Encoding.UTF8
            };
            //ScanWorksAPI ScanworksMgr; 
            try
            {
                if (args[0].Contains("LaunchTCP")) // TCP Server Mode
                {
                    if (StartServer(ServerObject))
                    {
                        Console.WriteLine("TCP Server Listening at Port: 12012");
                        ServerObject.DataReceived += ServerDataReceived;
                    }
                    else
                    {
                        //Console.WriteLine("Unable to Start TCP Server");
                    }

                }
                else // Console Mode
                {
                    UserOption = "none";
                }
            }
            catch (IndexOutOfRangeException)
            {

            }
            // Console mode
            while (UserOption != "exit")
            {
                Console.WriteLine("Type your Option:");
                UserOption = Console.ReadLine();
                UserOption = UserOption.ToLower();

                switch (UserOption)
                {
                    case "connect":
                        ApiReturn = ScanWorksAPI.sw_connect();
                        Console.WriteLine("API Return for Connect: " + ApiReturn);
                        break;
                    case "disconnect":
                        ApiReturn = ScanWorksAPI.sw_disconnect();
                        Console.WriteLine("API Return for Disconnect: " + ApiReturn);
                        break;
                    case "getprojectcount":
                        ApiReturn = ScanWorksAPI.sw_GetProjectCount();
                        Console.WriteLine("The Project count is: " + ApiReturn);
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine("Invalid Command");
                        break;
                }

            }

            // Enforce SW Disconnect when program closes
            ScanWorksAPI.sw_disconnect();


        }

        // Start the TCP Server listener
        private static bool StartServer(SimpleTcpServer ServerObject)
        {
            // TCP Server only on local looopback (To enable TestStand/Interprocess communication)
            byte[] ipAddressBinary = { 127, 0, 0, 1 };
            System.Net.IPAddress ip = new System.Net.IPAddress(ipAddressBinary);
            // Listen on Localhost port #12012 
            try
            {
                ServerObject.Start(ip, 12012);
                return true;
            }
            catch (SocketException)
            {
                // MessageBox.Show("Error, Socket Port in use", "Socket Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Unable to Start TCP Server");
                return false;

            }

        }

        // TCP Server Listener Event Handler

        private static void ServerDataReceived(object sender, SimpleTCP.Message msg)
        {
            string ReturnMessage = "";
            // User must send command,parameter1,parameter2...
            string[] UserData = msg.MessageString.Split(',');
            switch (UserData[0]) // Switch the command
            {
                case "connect":
                    if (ScanWorksAPI.sw_connect() == 0)
                    {
                        ReturnMessage += "Scanworks API Connected \n";
                    }
                    else
                    {
                        ReturnMessage += "ERROR: Unable to Connect Scanworks API\n";
                    }
                    break;
                case "disconnect":
                    if (ScanWorksAPI.sw_disconnect() == 0)
                    {
                        ReturnMessage += "Scanworks API is now Disconnected";
                    }
                    else
                    {
                        ReturnMessage += "ERROR: Unable to Disconnect The Scanworks API\n";
                    }
                    break;
                case "getprojectcount":
                    ReturnMessage += "Project Count declared in Scanworks: " + ScanWorksAPI.sw_GetProjectCount();    
                    break;
                case "getprojectat":
                    StringBuilder ProjectName = new StringBuilder(1000);
                    ScanWorksAPI.sw_GetProjectNameAt(Convert.ToInt32(UserData[1]), ProjectName);
                    ReturnMessage += "Project at: "+UserData[1]+" name is: "+ProjectName;
                    break;

                default:
                    break;
            }

            ReturnMessage += "\r";
            msg.Reply(string.Format(ReturnMessage));
        }

    }
}
