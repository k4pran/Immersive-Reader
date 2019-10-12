using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Modules.Common {

    public class Logger {

        private static readonly string TRACE_COLOR = "brown";
        private static readonly string DEBUG_COLOR = "blue";
        private static readonly string INFO_COLOR = "green";
        private static readonly string WARNING_COLOR = "orange";
        private static readonly string ERROR_COLOR = "red";
        
        private static readonly string CALLER_ORIGIN_COLOR = "teal";
        private static readonly string THREAD_NAME_COLOR = "purple";

        private static readonly string DATE_TIME_FORMAT = "yyyy-MM-dd hh:mm:ss";
        
        public static void Trace(string message,
            [CallerFilePathAttribute] string callerPath = "", 
            [CallerMemberName] string memberName = "", 
            [CallerLineNumber] int callerLineNumber = 0) {
            string origin = BuildCallerOrigin(callerPath, memberName, callerLineNumber.ToString());
            LogTrace(message, origin);
        }
        
        public static void Debug(string message,
            [CallerFilePathAttribute] string callerPath = "", 
            [CallerMemberName] string memberName = "", 
            [CallerLineNumber] int callerLineNumber = 0) {
            string origin = BuildCallerOrigin(callerPath, memberName, callerLineNumber.ToString());
            LogDebug(message, origin);
        }

        public static void Info(string message,
                                [CallerFilePathAttribute] string callerPath = "", 
                                [CallerMemberName] string memberName = "", 
                                [CallerLineNumber] int callerLineNumber = 0) {
            string origin = BuildCallerOrigin(callerPath, memberName, callerLineNumber.ToString());
            LogInfo(message, origin);
        }
        
        public static void Warning(string message,
            [CallerFilePathAttribute] string callerPath = "", 
            [CallerMemberName] string memberName = "", 
            [CallerLineNumber] int callerLineNumber = 0) {
            string origin = BuildCallerOrigin(callerPath, memberName, callerLineNumber.ToString());
            LogWarning(message, origin);
        }
        
        public static void Error(string message,
            [CallerFilePathAttribute] string callerPath = "", 
            [CallerMemberName] string memberName = "", 
            [CallerLineNumber] int callerLineNumber = 0) {
            string origin = BuildCallerOrigin(callerPath, memberName, callerLineNumber.ToString());
            LogError(message, origin);
        }

        private static string BuildCallerOrigin(string callerPath, string memberName, string lineNumber) {
            string relCallerPath = FileUtils.absToRelativePath(callerPath);
            return String.Format("<color={0}>{1} {2}:{3}</color>", CALLER_ORIGIN_COLOR, 
                relCallerPath, memberName, lineNumber);
        }
        
        private static void LogTrace(string message, string origin) {
            UnityEngine.Debug.Log(BuildLog(message, origin, TRACE_COLOR, LogLevel.TRACE));
        }

        private static void LogDebug(string message, string origin) {
            UnityEngine.Debug.Log(BuildLog(message, origin, DEBUG_COLOR, LogLevel.DEBUG));
        }
        
        private static void LogInfo(string message, string origin) {
            UnityEngine.Debug.Log(BuildLog(message, origin, INFO_COLOR, LogLevel.INFO));
        }

        private static void LogWarning(string message, string origin) {
            UnityEngine.Debug.Log(BuildLog(message, origin, WARNING_COLOR, LogLevel.WARNING));
        }
        
        private static void LogError(string message, string origin) {
            UnityEngine.Debug.Log(BuildLog(message, origin, ERROR_COLOR, LogLevel.ERROR));
        }

        private static string BuildLog(string message, string origin, string color, LogLevel logLevel) {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("<i>{0}</i>", CurrentDateTime()));
            sb.Append("  ");
            sb.Append(String.Format("<color={0}>{1}</color>", color, rightPadString(logLevel.ToString(), 15)));
            sb.Append("  ");
            sb.Append(String.Format("<color={0}>{1}</color>", THREAD_NAME_COLOR, CurrentThreadId()));
            sb.Append(" --- ");
            sb.Append(String.Format("{0} : ", origin));
            sb.Append(String.Format("<b>{0}</b>", message));
            return sb.ToString();
        }

        private static string CurrentDateTime() {
            return DateTime.Now.ToString(DATE_TIME_FORMAT);
        }

        private static string CurrentThreadId() {
            return Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private static string rightPadString(string originalString, int paddedLength) {
            return originalString.PadRight(paddedLength, ' ').Substring(0, paddedLength);
        }

        private enum LogLevel {
            TRACE,
            DEBUG,
            INFO,
            WARNING,
            ERROR
        }
    }
}