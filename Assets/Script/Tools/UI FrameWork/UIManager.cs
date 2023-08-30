using UnityEngine;
namespace TsingPigSDK
{

    public class UIManager : Singleton<UIManager>
    {
        [HideInInspector] public static PanelManager panelManager;
        
        public GameObject ActivePanelObject
        {
            get { return panelManager.TopPanelObject; }
        }

        private new void Awake()
        {
            base.Awake();
            panelManager = new PanelManager();
        }

        private void Start()
        {
            LoadMainPanel();
        }

        private void Update()
        {

        }

        /// <summary>
        /// ����ǰ�Ļ��������������߻�õ�ǰ�����ĳ�����
        /// </summary>
        /// <typeparam name="T">������� �޶�ΪComponent����</typeparam>
        /// <returns></returns>
        public T GetOrAddComponetToActivePanel<T>() where T : Component
        {
            if (ActivePanelObject.GetComponent<T>() == null)
            {
                ActivePanelObject.AddComponent<T>();

            }
            return ActivePanelObject.GetComponent<T>();
        }

        /// <summary>
        /// �������Ʋ���һ���Ӷ���
        /// </summary>
        /// <param name="name">�Ӷ�������</param>
        /// <returns></returns>
        public GameObject FindChildGameObject(string name)
        {
            Transform[] transforms = ActivePanelObject.GetComponentsInChildren<Transform>();
            foreach (var item in transforms)
            {
                if (item.gameObject.name == name)
                {
                    return item.gameObject;
                }
            }
            Debug.LogWarning($"{ActivePanelObject.name}���Ҳ�����Ϊ{name}��������");
            return null;
        }

        /// <summary>
        /// �������ƻ�ȡ�Ӷ�������
        /// </summary>
        /// <typeparam name="T">�������</typeparam>
        /// <param name="name">�Ӷ�������</param>
        /// <returns></returns>
        public T GetOrAddComponentInChilden<T>(string name) where T : Component
        {
            GameObject child = FindChildGameObject(name);
            if (child != null)
            {
                if (child.GetComponent<T>() != null)
                {
                    return child.GetComponent<T>();
                }
                child.AddComponent<T>();
            }
            return null;
        }

        public void LoadMainPanel()
        {

        }
    }
}