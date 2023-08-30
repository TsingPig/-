using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace TsingPigSDK
{
    public class UIDicManager
    {
        /// <summary>
        /// UI��Ϣ�����ֵ�
        /// </summary>
        private Dictionary<UIType, GameObject> _dicUI;

        /// <summary>
        /// ��ʾһ��UI����
        /// </summary>
        /// <param name="type">ui��Ϣ</param>
        /// <returns></returns>
        public async Task<GameObject> GetSingleUI(UIType type)
        {
            GameObject parent = GameObject.Find("Canvas");
            if (parent != null)
            {
                if (_dicUI.ContainsKey(type))
                {
                    return _dicUI[type];
                }
                else
                {
                    GameObject uiAsset = await Res.LoadAsync<GameObject>(type.Path);
                    GameObject ui = GameObject.Instantiate(uiAsset) as GameObject;
                    ui.name = type.Name;
                    _dicUI.Add(type, ui);
                    return ui;
                }
            }
            else
            {
                Log.Error("��ʧCanvas���봴��Canvas����");
                return null;
            }
        }
        public void DestroyUI(UIType type)
        {
            foreach (var item in _dicUI.Values)
            {
                Debug.Log(item.ToString());
            }
            if (_dicUI.ContainsKey(type))
            {
                GameObject.Destroy(_dicUI[type]);
                _dicUI.Remove(type);
            }
        }
    }
}