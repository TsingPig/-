using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PatientName", menuName = "Data/PatientName")]
public class PatientName_SO : ScriptableObject
{
    [Header("��Ԥ��"), SerializeField] private List<string> _firstNameLib;

    [Header("Ů��Ԥ��"), SerializeField] private List<string> _femaleLastNameLib;

    [Header("����Ԥ��"), SerializeField] private List<string> _maleLastNameLib;
    public List<string> FirstNameLib => _firstNameLib;
    public List<string> FemaleLastNameLib => _femaleLastNameLib;
    public List<string> MaleLastNameLib => _maleLastNameLib;
}
