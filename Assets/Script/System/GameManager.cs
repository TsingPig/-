using TsingPigSDK;
using UnityEngine;

public sealed class GameManager : Singleton<GameManager>
{
    private void Init()
    {
        Log.CallInfo($"{DataManager.Instance.name}����");
        Log.CallInfo($"{PeriodManager.Instance.name}����");
        Log.CallInfo($"{InspectionManager.Instance.name}����");
        Log.CallInfo($"{InstrumentManager.Instance.name}����");
        Log.CallInfo($"{PatientManager.Instance.name}����");
    }
    private void GameEntry()
    {

    }
    protected override void Awake()
    {
        base.Awake();
        Init();
        GameEntry();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            RandomSystem.RandomPatientInfo();
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            PeriodManager.Instance.LogPeriod();
        }
    }
}
