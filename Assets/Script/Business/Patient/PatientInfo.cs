public enum Gender
{
    Female = 0,
    Male = 1
}

public class PatientInfo
{
    public string patientID;
    public string patientName;
    public Gender patientGender;
    public int patientAge;
    public string patientAddress;
    public string patientPhoneNumber;
}
namespace TsingPigSDK
{

    public static partial class Log
    {
        public static void Info(PatientInfo patientInfo)
        {
            Log.Info(
                "ID��" + patientInfo.patientID,
                "������" + patientInfo.patientName,
                "�Ա�" + patientInfo.patientGender.ToString(),
                "���䣺" + patientInfo.patientAge.ToString(),
                "��ַ��" + patientInfo.patientAddress,
                "�绰��" + patientInfo.patientPhoneNumber
            );
        }
    }

}