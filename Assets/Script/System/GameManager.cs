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
        Log.CallInfo($"{InputManager.Instance.name}����");
    }
    private void GameEntry()
    {
        UIManager.Instance.Enter(new MainPanel());

    }
    protected override void Awake()
    {
        base.Awake();
        Init();
 
    }
    private void Start()
    {
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
