using System;
using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;
public static class Log
{
    public static void CallInfo(string msg = "")
    {
        MethodBase callingMethod = new StackTrace().GetFrame(1).GetMethod();
        Type callingType = callingMethod.DeclaringType;
        //Debug.Log($"{callingType.Name} : {callingMethod.Name}    Msg:{msg}");
        Info(" ", callingType.Name, callingMethod.Name, $"  Msg : {msg}");
    }
    public static void Error(string msg = "")
    {
        Debug.LogError($"����{msg}");
    }
    public static void Info(string spliter = " ", params string[] strings)
    {
        string result = string.Join(spliter, strings);
        Debug.Log(result);
    }
    public static void Info(PatientInfo patientInfo)
    {
        Log.Info(" ",
            "ID��" + patientInfo.patientID,
            "������" + patientInfo.patientName,
            "�Ա�" + patientInfo.patientGender.ToString(),
            "���䣺" + patientInfo.patientAge.ToString(),
            "��ַ��" + patientInfo.patientAddress,
            "�绰��" + patientInfo.patientPhoneNumber
        );
    }
}
