using TMPro;
using TsingPigSDK;

public class PatientInfoPanel : BasePanel
{
    TMP_Text _patientID;
    TMP_Text _patientName;
    TMP_Text _patientAge;
    TMP_Text _patientGender;
    TMP_Text _patientAddress;
    TMP_Text _patientPhone;

    PatientInfo _patientInfo;
    public PatientInfoPanel(PatientInfo patientInfo) 
    {
        _patientInfo = patientInfo;
    }
    public override void OnEntry()
    {
        _patientID = UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("ID");
        _patientName = UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("����");
        _patientAge = UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("����");
        _patientAddress = UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("��ַ");
        _patientGender = UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("�Ա�");
        _patientPhone = UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("�绰");
        
        
        _patientID.text = _patientInfo.patientID;
        _patientName.text= _patientInfo.patientName;
        _patientAddress.text= _patientInfo.patientAddress;
        _patientGender.text = _patientInfo.patientGender == Gender.Male ? "��" : "Ů";
        _patientPhone.text = _patientInfo.patientPhoneNumber;
        _patientAge.text= _patientInfo.patientAge.ToString();   
    }
}
