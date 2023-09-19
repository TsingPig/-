using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientInfoMono : MonoBehaviour
{
    ListView _listView;

    private void Awake()
    {
    
    }
    public void WindowsChange(int index)
    {
        switch (index)
        {
            case 0:break;
            case 1:
                Debug.Log("WindowsChange");
                _listView = UIManager.Instance.GetOrAddComponentInChilden<ListView>("���߼����Ŀ�б�", transform);
                _listView.InitializeItems(1);
            break;
        }
    }
}
