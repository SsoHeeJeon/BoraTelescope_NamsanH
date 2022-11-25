using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ErrorLog : bora_client_test
{
    public LogSendServer logsave;

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error)
        {
            string a = "at Assets/Scripts/";
            int index = stackTrace.IndexOf(a);
            var result = stackTrace.Substring(index + a.Length);
            result = result.Split('.')[0];
            if (type == LogType.Error)
            {
                logsave.WriteErrorLog(LogSendServer.ErrorLogCode.UnityError, type + ":" + logString + "-" + stackTrace, result);
            } else if(type == LogType.Exception)
            {
                logsave.WriteErrorLog(LogSendServer.ErrorLogCode.UnityException, type + ":" + logString + "-" + stackTrace, result);
            }
        }
    }
}
