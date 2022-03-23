using System;
using System.Text;
using System.Runtime.InteropServices;


namespace ScanWorksAPILib
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class ScanWorksAPI
	{
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_connect(); // Implemented
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_disconnect(); // Implemented
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetProjectCount(); // Implemented
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetProjectNameAt(int counter,StringBuilder theName); // Implemented
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_LoadProjectAt(int counter);
		[DllImport("ScanWorksCOMServer.DLL", CharSet = CharSet.Ansi)]
		public static extern int sw_LoadProject(String ProjectName); // Load a project by Name
		[DllImport("ScanWorksCOMServer.DLL", CharSet = CharSet.Ansi)]
		public static extern int sw_LoadDesign(String DesignName); // Load a project by Name
		[DllImport("ScanWorksCOMServer.DLL", CharSet = CharSet.Ansi)]
		public static extern int sw_LoadAction(String ActionName); // Load a project by Name
		[DllImport("ScanWorksCOMServer.DLL", CharSet = CharSet.Ansi)]
		public static extern int sw_LoadSequence(String SequenceName); // Load a project by Name
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetDesignCount(); // Implemented as part of a routine
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetDesignNameAt(int counter,StringBuilder theName);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_LoadDesignAt(int counter);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetActionCount();
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetActionNameAt(int counter,StringBuilder theName);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_LoadActionAt(int counter);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_RunAction();
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_DiagnoseAction();
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetSequenceCount();
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetSequenceNameAt(int counter,StringBuilder theName);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_LoadSequenceAt(int counter);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_RunSequence();
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetSequenceReportFile(StringBuilder theReportFileName);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetSequenceSummaryReport(StringBuilder ReportTitle,StringBuilder theReportFileName);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetReportFileNameCount();
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetReportFileNameAt(int counter,StringBuilder theReportName, StringBuilder theReportTitle);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetErrorMessage(StringBuilder theErrorText);
        [DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_connectWithController(String controllerString);
		[DllImport("ScanWorksCOMServer.DLL", CharSet = CharSet.Ansi)]
		public static extern int sw_DeleteProject(String ProjectName);
		[DllImport("ScanWorksCOMServer.DLL", CharSet = CharSet.Ansi)]
		public static extern int sw_ImportProjectTo(String FileName, int ProjectNameSize, String ProjectName, String UseProjectName, String TargetDir);
		[DllImport("ScanWorksCOMServer.DLL", CharSet = CharSet.Ansi)]
		public static extern int sw_ExportProject(String FileName, int ExportType, int IncludeUserFiles, int Linked, int AllowOverwrite);


		public ScanWorksAPI()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// Scanworks Result Code
		public static string ScanworksHandler(int CodeResponse)
        {
			string CodeMessage;

			switch (CodeResponse)
            {
				case 0:
					CodeMessage = "Success";
					break;
				case -1:
					CodeMessage = "Client Not Connected to Scanworks";
					break;
				case -2:
					CodeMessage = "Session is already Initiated";
					break;
				case -3:
					CodeMessage = "Unable to Connect to Scanworks Software";
					break;
				case -4:
					CodeMessage = "Specified Project cannot be found";
					break;
				case -5:
					CodeMessage = "No project has been selected";
					break;
				case -6:
					CodeMessage = "Specified Design could not be found";
					break;
				case -7:
					CodeMessage = "Action Specified could not be found";
					break;
				case -8:
					CodeMessage = "No design description has been loaded";
					break;
				case -9:
					CodeMessage = "No action has been loaded";
					break;
				case -10:
					CodeMessage = "The unit being tested failed the action that was run";
					break;
				case -11:
					CodeMessage = "The action encountered a major error, possibly caused by corruption of the action file or a problem with the current design or action files";
					break;
				case -12:
					CodeMessage = "The action or sequence is not ready to run";
					break;
				case -18:
					CodeMessage = "The input buffer passed in as an output parameter is not long enough to hold the information";
					break;
				case -21:
					CodeMessage = "The specified file is not a valid ScanWorks exported project file name";
					break;
				case -22:
					CodeMessage = "A project by the name specified already exists";
					break;
				case -39:
					CodeMessage = "No sequence has been loaded";
					break;
				case -40:
					CodeMessage = "The sequence encountered a major error; possibly it was not ready to run";
					break;
				case -41:
					CodeMessage = "The unit being tested failed the sequence that was run";
					break;
				case -46:
					CodeMessage = "The directory specified is not a valid directory name or does not exist";
					break;
				case -47:
					CodeMessage = "The project failed to export for some unknown reason";
					break;
				default:
					CodeMessage = "Unknown Error Status";
					break;
			}


			return CodeMessage;
        }

		public static void PrintResult(int StatusCode, string GenericMessage)
        {
			if (StatusCode == 0) // Successful Code
			{
				Console.WriteLine("<-< Successfull "+GenericMessage+" >->"); //Distinguish characters from the actual response
				Console.WriteLine("ScanworksReturn: " + StatusCode);
				Console.WriteLine("\n >>"); //Ending character
			}
			else
			{
				Console.WriteLine("ERROR: During Attempt to: " + GenericMessage);
				Console.WriteLine("API Message " + ScanWorksAPI.ScanworksHandler(StatusCode));
				Console.WriteLine("ScanworksReturn: " + StatusCode);
				Console.WriteLine("\n >>");
			}
		}

		public static string FormatStatusTCP(int StatusCode, string GenericMessage)
		{
			string ReturnString = "";

			if (StatusCode == 0) // Successful Code
			{
				ReturnString += "<-< Successfull " + GenericMessage + " >-> \n"; //Distinguish characters from the actual response
				ReturnString += "ScanworksReturn: " + StatusCode + "\n\n >>";
			}
			else
			{
				ReturnString += "ERROR: During Attempt to: " + GenericMessage +"\n";
				ReturnString += "API Message " + ScanWorksAPI.ScanworksHandler(StatusCode)+"\n";
				ReturnString += "ScanworksReturn: " + StatusCode+"\n";				
			}
			return ReturnString;
		}


	}
}
