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
            string ItemType = "nothing";
            int ItemCount = 0;
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
                StringBuilder ItemName = new StringBuilder(1000);
                Console.WriteLine("Type your Option:");
                UserOption = Console.ReadLine();
                UserOption = UserOption.ToLower();

                switch (UserOption)
                {
                    case "connect":
                        ApiReturn = ScanWorksAPI.sw_connect();
                        ScanWorksAPI.PrintResult(ApiReturn, "Connection to API");
                        break;
                    case "disconnect":
                        ApiReturn = ScanWorksAPI.sw_disconnect();
                        ScanWorksAPI.PrintResult(ApiReturn, "Disconnection from API");

                        break;
                    case "printall":
                        Console.WriteLine("To display Sequences and Actions, a Project and Design must be loaded");
                        Console.WriteLine("Enter Item to Print [projects|designs|sequences|actions]: >");
                        ItemType = Console.ReadLine();
                        switch (ItemType.ToLower())
                        {
                            case "projects":
                                ItemCount = ScanWorksAPI.sw_GetProjectCount();
                                break;
                            case "actions":
                                ItemCount = ScanWorksAPI.sw_GetActionCount();
                                break;
                            case "sequences":
                                ItemCount = ScanWorksAPI.sw_GetSequenceCount();
                                break;
                            case "designs":
                                ItemCount = ScanWorksAPI.sw_GetDesignCount();
                                break;
                        }

                        for (int i=0; i < ItemCount; i++)
                        {
                            switch (ItemType.ToLower())
                            {
                                case "projects":
                                    ApiReturn = ScanWorksAPI.sw_GetProjectNameAt(i, ItemName);
                                    break;
                                case "actions":
                                    ApiReturn = ScanWorksAPI.sw_GetActionNameAt(i, ItemName);
                                    
                                    break;
                                case "sequences":
                                    ApiReturn = ScanWorksAPI.sw_GetSequenceNameAt(i, ItemName);
                                    break;
                                case "designs":
                                    ApiReturn = ScanWorksAPI.sw_GetDesignNameAt(i, ItemName);
                                    break;
                            }

                            if (ApiReturn == 0)
                            {
                                Console.WriteLine("Item " + ItemType + " at: " + i + " is: \t" + ItemName.ToString());
                            }
                        }
                        break;
                    case "countall":
                        Console.WriteLine("Enter item type to Count [projects|actions|sequences|designs]: >");
                        ItemType = Console.ReadLine();
                        switch (ItemType.ToLower())
                        {
                            case "projects":
                                ItemCount = ScanWorksAPI.sw_GetProjectCount();
                                break;
                            case "actions":
                                ItemCount = ScanWorksAPI.sw_GetActionCount();
                                break;
                            case "sequences":
                                ItemCount = ScanWorksAPI.sw_GetSequenceCount();
                                break;
                            case "designs":
                                ItemCount = ScanWorksAPI.sw_GetDesignCount();
                                break;
                            default:
                                Console.WriteLine("Wrong Type of Item. Select from [projects|actions|sequences|designs]. Printing Default Project Count");
                                ItemCount = ScanWorksAPI.sw_GetProjectCount();
                                break;
                        }
                        Console.WriteLine("Item " + ItemType + " total Count: " + ItemCount);
                        Console.WriteLine("ScanworksReturn: " + ItemCount);
                        Console.WriteLine("\n >>");
                        break;
                    case "import":
                        Console.WriteLine("Enter Project Path: >");
                        string ProjectPath = Console.ReadLine();
                        Console.WriteLine("Enter ProjectName to Use: >");
                        string ProjectNameToUse = Console.ReadLine();
                        ApiReturn = ScanWorksAPI.sw_ImportProjectTo(ProjectPath, 1024, ProjectNameToUse, ProjectNameToUse, "");
                        ScanWorksAPI.PrintResult(ApiReturn, "Import of " + ProjectNameToUse);

                        break;
                    case "load":
                        Console.WriteLine("\nEnter Item Type to load [project|action|sequence|design]: >");
                        ItemType = Console.ReadLine();
                        Console.WriteLine("\nEnter Item Name to Load: >");
                        //int ItemIndex = Convert.ToInt32(Console.ReadLine());
                        string InputItem = Console.ReadLine();
                        switch (ItemType.ToLower())
                        {
                            case "project":
                                ApiReturn = ScanWorksAPI.sw_LoadProject(InputItem);
                                break;
                            case "action":
                                ApiReturn = ScanWorksAPI.sw_LoadAction(InputItem);
                                break;
                            case "sequence":
                                ApiReturn = ScanWorksAPI.sw_LoadSequence(InputItem);
                                break;
                            case "design":
                                ApiReturn = ScanWorksAPI.sw_LoadDesign(InputItem);
                                break;
                            default:
                                Console.WriteLine("<< ERROR: Wrong Type of Item. Select from [project|action|sequence|design] >>");
                                ApiReturn = -500;
                                break;
                        }
                        ScanWorksAPI.PrintResult(ApiReturn, "Load Item");
                        break;
                    case "export":
                        Console.WriteLine("Enter the Backup Path");
                        string BackupPath = Console.ReadLine();
                        StringBuilder BackupPathParam = new StringBuilder(1000);
                        BackupPathParam.Clear();
                        BackupPathParam.Append(BackupPath);
                        ApiReturn = ScanWorksAPI.sw_ExportProject(BackupPath, 1, 0, 0, 0);
                        ScanWorksAPI.PrintResult(ApiReturn, "Export Project");

                        break;
                    case "delete":
                        Console.WriteLine("Enter the Project Name to Delete: ");
                        string DeleteName = Console.ReadLine();
                        ApiReturn = ScanWorksAPI.sw_DeleteProject(DeleteName);
                        ScanWorksAPI.PrintResult(ApiReturn, "Deletion of Project " + DeleteName);
                        
                        break;
                    case "run":
                        Console.WriteLine("Choose to run [Sequence|Action]: >");
                        ItemType = Console.ReadLine();
                        ApiReturn = -1000;
                        switch (ItemType.ToLower())
                        {
                            case "sequence":
                                ApiReturn = ScanWorksAPI.sw_RunSequence();
                                break;
                            case "action":
                                ApiReturn = ScanWorksAPI.sw_RunAction();
                                break;
                            default:
                                Console.WriteLine("ERROR: User Error, undefined type. Select [sequence|action] to run");
                                ApiReturn = -1500;
                                break;
                        }
                        if (ApiReturn == 0)
                        {
                            Console.WriteLine("PASS Result");
                            Console.WriteLine("ScanworksReturn: " + ApiReturn);
                            Console.WriteLine("\n >>");
                        }
                        else
                        {
                            if (ApiReturn == -41 || ApiReturn == -10)
                            {
                                Console.WriteLine("FAIL Result");
                                Console.WriteLine("ScanworksReturn: " + ApiReturn);
                                Console.WriteLine("\n >>");
                            }
                            else
                            {
                                Console.WriteLine("ERROR: during run of " + ItemType + " " + "\n" +
                                    "Error Code: " + ApiReturn + "\n" +
                                    "Error Message+" + ScanWorksAPI.ScanworksHandler(ApiReturn));
                                Console.WriteLine("ScanworksReturn: " + ApiReturn);
                                Console.WriteLine("\n >>");
                            }
                        }
                        break;
                    case "print_details":
                        StringBuilder ReportName = new StringBuilder(500);
                        StringBuilder ReportPath = new StringBuilder(500);

                        ReportName.Append("DefaultReport");
                        ReportPath.Append("c:\\working\\Backup\\details.txt");

                        ApiReturn = ScanWorksAPI.sw_GetReportFileNameCount();
                        for (int i=0; i < ApiReturn; i++)
                        {
                            Console.WriteLine("Available Report: \t" + ScanWorksAPI.sw_GetReportFileNameAt(i, ReportName, ReportName));
                        }

                        Console.WriteLine("\nExtended Details ");
                        ApiReturn = ScanWorksAPI.sw_GetSequenceReportFile(ReportPath);
                        if (ApiReturn == 0)
                        {
                            Console.WriteLine("Successfull Report Created");
                        }
                        else
                        {
                                Console.WriteLine("FAIL: during Report Build \n" +
                                                  "Error Code: " + ApiReturn + "\n" +
                                                  "Error Message+" + ScanWorksAPI.ScanworksHandler(ApiReturn));
                        }
                            break;
                    case "exit":
                        ApiReturn = ScanWorksAPI.sw_disconnect();
                        return;
                    default:
                        Console.WriteLine("ERROR: Invalid Command");
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
            int ItemCount = 0;
            switch (UserData[0]) // Switch the command
            {
                case "connect": // Connect to the scanworks API (must be executed first)
                    ScanworksReturnCode = ScanWorksAPI.sw_connect();
                    if (ScanworksReturnCode == 0)
                    {
                        ReturnMessage += "Scanworks API Connected\n";
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
                case "countall": // Return the count of the selected item
                    try
                    {
                        switch (UserData[1]) // Select which type of object are we counting ... 
                        {
                            case "projects":
                                ItemCount = ScanWorksAPI.sw_GetProjectCount();
                                break;
                            case "actions":
                                ItemCount = ScanWorksAPI.sw_GetActionCount();
                                break;
                            case "sequences":
                                ItemCount = ScanWorksAPI.sw_GetSequenceCount();
                                break;
                            case "designs":
                                ItemCount = ScanWorksAPI.sw_GetDesignCount();
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
                case "printall":
                    StringBuilder ItemName = new StringBuilder(1000);
                    ScanworksReturnCode = -1000;
                    switch (UserData[1]) // Select which type of Object you want to print and count how many of those items there are
                    {
                        case "projects":
                            ItemCount = ScanWorksAPI.sw_GetProjectCount();
                            break;
                        case "actions":
                            ItemCount = ScanWorksAPI.sw_GetActionCount();
                            break;
                        case "sequences":
                            ItemCount = ScanWorksAPI.sw_GetSequenceCount();
                            break;
                        case "designs":
                            ItemCount = ScanWorksAPI.sw_GetDesignCount();
                            break;
                    }                    
                    for (int i=0; i < ItemCount; i++) // Print all of the items 
                    {
                        ItemName.Clear(); // Clear the handler
                        switch (UserData[1]) // Select which type of Object you want to print
                        {
                            case "projects":
                                ScanworksReturnCode = ScanWorksAPI.sw_GetProjectNameAt(i,ItemName);
                                break;
                            case "actions":
                                ScanworksReturnCode = ScanWorksAPI.sw_GetActionNameAt(i,ItemName);
                                break;
                            case "sequences":
                                ScanworksReturnCode = ScanWorksAPI.sw_GetSequenceNameAt(i, ItemName);
                                break;
                            case "designs":
                                ScanworksReturnCode = ScanWorksAPI.sw_GetDesignNameAt(i, ItemName);
                                break;
                        }
                        if (ScanworksReturnCode == 0)
                        {
                            ReturnMessage += "Item " + UserData[1] + " at: \t" + ItemName.ToString() + "\n";
                        }
                        else
                        {
                            ReturnMessage += "ERROR: While displaying: " + UserData[1] + "\n" +
                                "Error Code: " + ScanworksReturnCode;
                        }

                    }

                    break;
                    
                case "load": // Load either a Project, a design, or a file... 

                    ScanworksReturnCode = -4500;
                    try
                    {
                        StringBuilder ObjectName = new StringBuilder(1000);
                        ObjectName.Clear();
                       //ObjectPath.Append(UserData[2]); // The location of the desired file is at Parameter 2 (number)

                        int SWItemIndex = Convert.ToInt32(UserData[2]);

                            switch (UserData[1]) // Define the type of item we want to load
                            {
                                case "design":
                                if (SWItemIndex <= ScanWorksAPI.sw_GetDesignCount())
                                {
                                    ScanworksReturnCode = ScanWorksAPI.sw_GetDesignNameAt(SWItemIndex, ObjectName);
                                    ScanworksReturnCode = ScanWorksAPI.sw_LoadDesignAt(SWItemIndex);
                                }
                                else
                                    ReturnMessage += "ERROR: Invalid Design Index";
                                    break;
                                case "project":
                                    if (SWItemIndex <= ScanWorksAPI.sw_GetProjectCount())
                                {
                                    ScanworksReturnCode = ScanWorksAPI.sw_GetProjectNameAt(SWItemIndex, ObjectName);
                                    ScanworksReturnCode = ScanWorksAPI.sw_LoadProjectAt(SWItemIndex);
                                }

                                    else
                                        ReturnMessage += "ERROR: Invalid Project Index";
                                    break;
                                case "action":
                                    if (SWItemIndex <= ScanWorksAPI.sw_GetActionCount())
                                {
                                    ScanworksReturnCode = ScanWorksAPI.sw_GetActionNameAt(SWItemIndex, ObjectName);
                                    ScanworksReturnCode = ScanWorksAPI.sw_LoadActionAt(SWItemIndex);
                                }

                                    else
                                        ReturnMessage += "ERROR: Invalid Action Index";
                                    break;
                                case "sequence":
                                    if (SWItemIndex <= ScanWorksAPI.sw_GetSequenceCount())
                                {
                                    ScanworksReturnCode = ScanWorksAPI.sw_GetSequenceNameAt(SWItemIndex, ObjectName);
                                    ScanworksReturnCode = ScanWorksAPI.sw_LoadSequenceAt(SWItemIndex);
                                }

                                    else
                                        ReturnMessage += "ERROR: Invalid Sequence Index";
                                break;
                                default:
                                        ReturnMessage += "ERROR: Invalid Item Type.";
                                ScanworksReturnCode = -17000;
                                break;

                            }
                        if (ScanworksReturnCode == 0)
                        {
                            ReturnMessage += "Successfull Load of " + UserData[1] + " at: " + UserData[2]+ "\n" +
                                             "Item Name: "+ObjectName.ToString();
                        }
                        else
                        {
                            ReturnMessage += "ERROR at Item Loading \n" +
                                             "Scanworks Error Code: " + ScanworksReturnCode + "\n" +
                                             "Scanworks Message: "+ScanWorksAPI.ScanworksHandler(ScanworksReturnCode);
                        }

                    }
                    catch (IndexOutOfRangeException)
                    {
                        ReturnMessage += "ERROR: Missing the correct parameters to load the Item \n";
                    }
                    break;

                case "delete":
                    string ProjName;
                    try
                    {                       
                        ProjName = UserData[1];
                        ScanworksReturnCode = ScanWorksAPI.sw_DeleteProject(ProjName);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        ReturnMessage += "ERROR: Incorrect Parameter for Delete Operation \n";
                    }
                    break;
                case "run":
                    // Console.WriteLine("Choose [Sequence|Action]");
                    // UserData[2];
                    // ScanworksReturnCode = -1000;
                    switch (UserData[1].ToLower()) // Define if you want to run a sequence or action 
                    {
                        case "sequence":
                            ScanworksReturnCode = ScanWorksAPI.sw_RunSequence(); // Seq must be already loaded
                            break;
                        case "action":
                            ScanworksReturnCode = ScanWorksAPI.sw_RunAction(); // Action must be already loaded
                            break;
                        default:
                            Console.WriteLine("ERROR: User Error, undefined type. Select [sequence|action] to run");
                            ScanworksReturnCode = -1500;
                            break;
                    }
                    if (ScanworksReturnCode == 0)
                    {
                        ReturnMessage += "PASS Result\n";
                    }
                    else
                    {
                        if (ScanworksReturnCode == -41 && ScanworksReturnCode == -10) // This happens when the RUN command fails, but no error occurred
                        {
                            ReturnMessage += "FAIL Result\n";
                        }
                        else
                        {
                            ReturnMessage += "ERROR: during run Attempt " + UserData[1] + "\n" +
                                "Error Code: " + ScanworksReturnCode + "\n" +
                                "Error Message+" + ScanWorksAPI.ScanworksHandler(ScanworksReturnCode);
                        }
                    }
                    break;
                case "export":
                    try
                    {
                        StringBuilder PathToExport = new StringBuilder(1000);
                        PathToExport.Clear();
                        PathToExport.Append(UserData[1]);
                        ScanworksReturnCode = ScanWorksAPI.sw_ExportProject(UserData[1], 1, 0, 0, 0);

                        if (ScanworksReturnCode == 0)
                        {
                            ReturnMessage += "Successful Export Project \n";
                        }
                        else
                        {
                            ReturnMessage += "ERROR: Unable to Export Project\n" +
                                             "Scanworks Error Code: " + ScanworksReturnCode +
                                             "Scanworks Message: " + ScanWorksAPI.ScanworksHandler(ScanworksReturnCode);
                        }
                    }
                    catch
                    {
                        ReturnMessage += "ERROR: User Error. Verify the Input Selector \n";
                    }

                    break;
                case "import":
                    //Console.WriteLine("Enter Project Path:");
                    string ProjectPath = UserData[1];
                    //Console.WriteLine("Enter ProjectName to Use");
                    string ProjectNameToUse = UserData[2];
                    ScanworksReturnCode = ScanWorksAPI.sw_ImportProjectTo(ProjectPath, 1024, ProjectNameToUse, ProjectNameToUse, "");

                    if (ScanworksReturnCode == 0)
                    {
                        ReturnMessage += "Successful import of the Project: " + ProjectNameToUse;
                    }
                    else
                    {
                        ReturnMessage += "ERROR: During Import\n" +
                                         "Scanworks Error Message: " + ScanWorksAPI.ScanworksHandler(ScanworksReturnCode);
                    }
                    break;

                default:
                    ReturnMessage += "Invalid Operation. Verify the Parameters \n";
                    break;
            }

            ReturnMessage += "\r";
            msg.Reply(string.Format(ReturnMessage));
        }

    }
}
