using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Date _date;
    private DayPeriod _dayPeriod;
    public Date Date { get => _date; }
    public DayPeriod DayPeriod { get => _dayPeriod; }
    public Period()
    {
        _date = Date.Monday;
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
        }
        else
        {
            _dayPeriod = (DayPeriod)((int)_dayPeriod + 1); // �����л�����һ��ʱ���
        }
    }

    public string GetPeriod()
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
        return $"{day} {period}";
    }
}
public class PeriodManager : Singleton<PeriodManager>
{
    public const float DAY_PERIOD_DURATION = 3f;

    private float _currentTime;

    private Period _currentPeriod;

    private void Init()
    {
        _currentTime = 0;
        _currentPeriod = new Period();
        StartCoroutine(UpdateTime());
        Log.Info("��ʼ��ʱ��Σ�", _currentPeriod.GetPeriod());
    }
    private IEnumerator UpdateTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(DAY_PERIOD_DURATION);
            _currentPeriod.MoveNext();
        }
    }
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    private void Update()
    {

    }
    public void LogPeriod()
    {
        Log.Info("��ǰʱ��Σ�", _currentPeriod.GetPeriod());
    }
}


