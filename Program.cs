using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTCP;
using ScanWorksAPILib;
using System.Net.Sockets;
using System.IO;

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
            int ScanworksReturnCode;
            switch (UserData[0]) // Switch the command
            {
                case "connect": // Connect to the scanworks API (must be executed first)
                    ScanworksReturnCode = ScanWorksAPI.sw_connect();
                    if (ScanworksReturnCode == 0)
                    {
                        ReturnMessage += "Scanworks API Connected \n";
                    }
                    else
                    {
                        ReturnMessage += "ERROR: Unable to Connect Scanworks API. "+ScanworksReturnCode+" \n";
                    }
                    break;
                case "disconnect": // Disconnect to the scanworks API
                    if (ScanWorksAPI.sw_disconnect() == 0)
                    {
                        ReturnMessage += "Scanworks API is now Disconnected";
                    }
                    else
                    {
                        ReturnMessage += "ERROR: Unable to Disconnect The Scanworks API\n";
                    }
                    break;
                case "getcount": // Return the count of the selected item

                    int ItemCount = 0;

                    try
                    {
                        switch (UserData[1]) // Select which type of object are we counting ... 
                        {
                            case "projects":
                                break;
                            case "actions":
                                break;
                            case "sequences":
                                break;
                            case "designs":
                                break;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        ReturnMessage += "ERROR: Missing parameters to define which Item must be counted \n";
                    }
                    ReturnMessage += "Count of "+UserData[2]+" declared in Scanworks: " + ItemCount;    
                    break;
                case "getprojectnameat":
                    try
                    {
                        StringBuilder ProjectName = new StringBuilder(1000);
                        ScanWorksAPI.sw_GetProjectNameAt(Convert.ToInt32(UserData[1]), ProjectName);
                        ReturnMessage += "Project at: " + UserData[1] + " name is: " + ProjectName;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        ReturnMessage += "ERROR: Missing the parameter to define the project at \n";
                    }
                    break;
                    
                case "load": // Load either a Project, a design, or a file... 

                    ScanworksReturnCode = -4500;
                    try
                    {
                        StringBuilder ObjectPath = new StringBuilder(1000);
                        ObjectPath.Clear();
                        ObjectPath.Append(UserData[2]); // The location of the desired file is at Parameter 2 (number)

                        int SWItemIndex = Convert.ToInt32(UserData[2]);

                            switch (UserData[1]) // Define the type of item we want to load
                            {
                                case "design":
                                     if (SWItemIndex <= ScanWorksAPI.sw_GetDesignCount())
                                        ScanworksReturnCode = ScanWorksAPI.sw_LoadDesignAt(SWItemIndex);
                                     else
                                        ReturnMessage += "ERROR: Invalid Design Index";
                                    break;
                                case "project":
                                    if (SWItemIndex <= ScanWorksAPI.sw_GetProjectCount())
                                        ScanworksReturnCode = ScanWorksAPI.sw_LoadProjectAt(SWItemIndex);
                                    else
                                        ReturnMessage += "ERROR: Invalid Project Index";
                                    break;
                                case "action":
                                    if (SWItemIndex <= ScanWorksAPI.sw_GetActionCount())
                                        ScanworksReturnCode = ScanWorksAPI.sw_LoadActionAt(SWItemIndex);
                                    else
                                        ReturnMessage += "ERROR: Invalid Action Index";
                                    break;
                                case "sequence":
                                    if (SWItemIndex <= ScanWorksAPI.sw_GetSequenceCount())
                                        ScanworksReturnCode = ScanWorksAPI.sw_LoadSequenceAt(SWItemIndex);
                                    else
                                        ReturnMessage += "ERROR: Invalid Sequence Index";
                                break;

                            }



                    }
                    catch (IndexOutOfRangeException)
                    {
                        ReturnMessage += "ERROR: Missing the correct parameters to load the Item \n";
                    }
                    break;

                case "none":
                    break;
                default:
                    break;
            }

            ReturnMessage += "\r";
            msg.Reply(string.Format(ReturnMessage));
        }

    }
}
