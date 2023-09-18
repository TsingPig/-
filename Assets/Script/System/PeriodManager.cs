using System.Collections;
using TsingPigSDK;
using UnityEngine;
public class PeriodManager : Singleton<PeriodManager>
{
    public const float DAY_PERIOD_DURATION = 2f;

    private float _currentSeconds;

    private Period _currentPeriod;

    public float CurrentSeconds => _currentSeconds;

    public string CurrentTimeString => GetTimeString((int)_currentSeconds);
    public string CurrentPeriodString => _currentPeriod.GetPeriodString();
    private static string GetTimeString(int seconds)
    {
        string timeString = string.Empty;

        int hours = seconds / 3600;
        timeString += hours.ToString().PadLeft(2, '0') + ":";
        seconds -= 3600 * hours;

        int minutes = seconds / 60;
        timeString += minutes.ToString().PadLeft(2, '0') + ":";
        seconds -= minutes * 60;
        timeString += seconds.ToString().PadLeft(2, '0');

        return timeString;
    }

    private void StopTime()
    {
        StopCoroutine(UpdatePeriod());
        StopCoroutine(UpdateTime());
        int totalPatientCount = PatientManager.Instance.TotalPatientCount;
        Log.Info($"�ܹ���ʱ��{CurrentSeconds}�������{totalPatientCount}������");
    }
    private void Init()
    {
        _currentPeriod = new Period();
    }
    private IEnumerator UpdateTime()
    {
        while (true)
        {
            //Log.Info($"��ǰʱ�䣺{_currentTime}");
            _currentSeconds += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator UpdatePeriod()
    {
        while (true)
        {
            //Log.Info($"��ǰ����ʱ�䣺{_currentTime}");
            yield return new WaitForSeconds(DAY_PERIOD_DURATION);
            _currentPeriod.MoveNext();
        }
    }

    private void Start()
    {
        PatientManager.Instance.AllPatientFinish_Event += StopTime;
        _currentSeconds = 0;
        StartCoroutine(UpdatePeriod());
        StartCoroutine(UpdateTime());

    }
    private void Update()
    {

    }
    private new void Awake()
    {
        base.Awake();
        Init();
    }
}

public enum Date
{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7
}
public enum DayPeriod
{
    BeforeDown = 1,
    Morning,
    Afternoon,
    Evening
}
public class Period
{

    private int _weekCount;

    private Date _date;

    private DayPeriod _dayPeriod;
    public Date Date { get => _date; }
    public DayPeriod DayPeriod { get => _dayPeriod; }
    public int WeekCount { get => _weekCount; }

    public Period()
    {
        _date = Date.Monday;
        _weekCount = 1;
        _dayPeriod = DayPeriod.BeforeDown;
    }
    public Period(Date date, DayPeriod dayPeriod)
    {
        _date = date;
        _dayPeriod = dayPeriod;
    }
    public void MoveNext()
    {
        if (_dayPeriod == DayPeriod.Evening) // �����ǰʱ����� Evening
        {
            _dayPeriod = DayPeriod.BeforeDown; // �л�����һ��� BeforeDown
            _date = (Date)(((int)_date % 7) + 1); // �л�����һ��
            if (_date == Date.Monday)
            {
                _weekCount++;
            }
        }
        else
        {
            _dayPeriod = (DayPeriod)((int)_dayPeriod + 1); // �����л�����һ��ʱ���
        }
    }

    /// <summary>
    /// ���ص�ǰPeriod������ַ�����ʾ
    /// </summary>
    /// <returns></returns>
    public string GetPeriodString()
    {
        string day = "";
        string period = "";
        switch (_date)
        {
            case Date.Monday:
                day = "����һ";
                break;
            case Date.Tuesday:
                day = "���ڶ�";
                break;
            case Date.Wednesday:
                day = "������";
                break;
            case Date.Thursday:
                day = "������";
                break;
            case Date.Friday:
                day = "������";
                break;
            case Date.Saturday:
                day = "������";
                break;
            case Date.Sunday:
                day = "������";
                break;
        }
        switch (_dayPeriod)
        {
            case DayPeriod.BeforeDown:
                period = "�賿";
                break;
            case DayPeriod.Morning:
                period = "����";
                break;
            case DayPeriod.Afternoon:
                period = "����";
                break;
            case DayPeriod.Evening:
                period = "����";
                break;
        }
        return $"{day}_{period}";
    }
}




