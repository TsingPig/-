using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Main : MonoBehaviour
{
    public CanvasGroup[] Panels;

    private TMP_Text[] _txtsInfoPanel;

    public void OpenPanel(string name)
    {
        CanvasGroup targetPanel= Panels.First((CanvasGroup item) => item.name == name);
        OpenPanel(targetPanel);
    }
 
    public void ClosePanel(string name)
    {
        CanvasGroup targetPanel = Panels.First((CanvasGroup item) => item.name == name);
       ClosePanel(targetPanel);
    }
   
    private void Start()
    {
        for (int i = 1; i < Panels.Length; i++)
        {
            ClosePanel(Panels[i]);
        }

        _txtsInfoPanel = new TMP_Text[] {
                UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("��ģ��ʱ��",transform),
                UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("ģ������",transform),
                UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("����������", transform),
                UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("�������", transform),
                UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("��ɼ����", transform),
                UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("���豸��", transform),
                UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("ʹ����", transform),
                UIManager.Instance.GetOrAddComponentInChilden<TMP_Text>("ά����", transform),
        };
       
    }
    private void OpenPanel(CanvasGroup targetPanel)
    {
        targetPanel.alpha = 1f;
        targetPanel.interactable = true;
        targetPanel.blocksRaycasts = true;
    }
    private void ClosePanel(CanvasGroup targetPanel)
    {
        targetPanel.alpha = 0f;
        targetPanel.interactable = false;
        targetPanel.blocksRaycasts = false;
    }
}
