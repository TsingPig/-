using System;
using UnityEditor;
using UnityEngine;

namespace TsingPigSDK
{

    public class UISystem : Singleton<UISystem>
    {
        private CursorState _cursorState = CursorState.None;

        public CursorState CursorState
        {
            get => _cursorState;
            set
            {
                if (value == CursorState.None)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else if (value == CursorState.Lock)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else if(value==CursorState.UI)
                {
                    _panelBuffer.StateRecord = _cursorState;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                _cursorState = value;
                Debug.Log(_cursorState);
            }
        }

        protected PanelBuffer _panelBuffer;
        public GameObject ActivePanelObject
        {
            get { return _panelBuffer.TopPanelObject; }
        }
        private new void Awake()
        {
            base.Awake();
            _panelBuffer = new PanelBuffer();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pop();
            }
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

        public void Enter(BasePanel panel)
        {
            _panelBuffer.Push(panel);
        }
        public void Enter(string panelName)
        {
            Type type = Type.GetType(panelName);

            if (type != null)
            {
                object panelInstance = Activator.CreateInstance(type);
                _panelBuffer.Push(panelInstance as BasePanel);
            }
            else
            {
                Log.Error($"δ�ҵ����ͣ�{panelName}");
            }
        }

        public void Pop()
        {
            _panelBuffer.Pop();
        }

        public GameObject GetSingleUI(UIType uiType)
        {
            return _panelBuffer.GetSingleUI(uiType);
        }
    }
}