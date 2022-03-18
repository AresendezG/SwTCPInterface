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
		public static extern int sw_connect();
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_disconnect();
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetProjectCount();
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetProjectNameAt(int counter,StringBuilder theName);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_LoadProjectAt(int counter);
		[DllImport("ScanWorksCOMServer.DLL", CharSet=CharSet.Ansi)]
		public static extern int sw_GetDesignCount();
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
		public ScanWorksAPI()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
