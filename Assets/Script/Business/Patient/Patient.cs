using Highlighters;
using System;
using System.Collections;
using System.Collections.Generic;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.AI;

public class Patient : MonoBehaviour, ISelectable
{
    #region ����ϵͳ

    private NavMeshAgent _agent;

    private bool walk_active = false;

    private Animator _animator;

    private Transform _prePatient;

    public Action<Transform> FinishInspection_Event;

    public bool Walk_Active
    {
        get { return walk_active; }
        set
        {
            walk_active = value;
            //foreach (Animator a in _anims)
            //    a.SetBool("walk", walk_active);
            _animator.SetBool("walk", walk_active);
        }
    }

    #endregion

    private Inspection _inspection;

    [SerializeField] private List<Instrument> _instruments;

    private PatientInfo _patientInfo;
    public PatientInfo PatientInfo => _patientInfo;
    public Inspection Inspection { get => _inspection; set => _inspection = value; }

    private Highlighter _highlighter;
    public Highlighter Highlighter => _highlighter;

    private void Awake()
    {
        _patientInfo=RandomSystem.RandomPatientInfo();
        _agent = GetComponent<NavMeshAgent>();
        _highlighter = transform.GetComponent<Highlighter>();
        //foreach (Animator a in CharacterCustomization.animators)
        //    _anims.Add(a);
        _animator =  transform.GetChild(2).GetComponent<Animator>();
    }

    private void Start()
    {
        _inspection = new Inspection();
        _instruments.Add(_inspection.GetNext(_agent));
        MoveNextInspection();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    MoveNextInspection();
        //}

    }

    /// <summary>
    /// ������һ������
    /// </summary>
    public void MoveNextInspection()
    {
        if (_instruments.Count > 0 && _instruments[0] != null)
        {
            MoveTarget(_instruments[0].AddMovingPatients(this));
        }
        else
        {
            MoveTarget(InspectionManager.Instance.InspectionExit);
            Log.Info($"{gameObject.name} �������������");
        }

    }

    /// <summary>
    /// ���ö����и���Ŀ��
    /// </summary>
    /// <param name="patient"></param>
    public void SetPrePatient(Transform patient)
    {
        if (_prePatient == null)
        {
            _prePatient = patient.parent;
            MoveTarget(patient);
        }
    }

    /// <summary>
    /// ���������ǰ��
    /// </summary>
    /// <param name="patient"></param>
    public void FollowPrePatient(Transform patient)
    {
        StopAllCoroutines();
        StartCoroutine(FollowPrePatientCoroutine(patient));
    }

    /// <summary>
    /// ���������ǰ��
    /// </summary>
    /// <param name="patient"></param>
    /// <returns></returns>
    IEnumerator FollowPrePatientCoroutine(Transform patient)
    {
        if (_prePatient == null)
        {
            MoveTarget(_instruments[0].Target);
        }
        else if (patient == _prePatient)
        {
            _prePatient = null;
            MoveTarget(_instruments[0].Target);
        }
        else
        {
            Log.Info($"{transform.name}����{_prePatient.name}");
            if (_instruments[0].Patients.Contains(this))
            {
                yield return new WaitForSeconds(_instruments[0].Patients.IndexOf(this) * 0.65f);
            }
            MoveTarget(_prePatient.GetChild(0));
        }
        yield break;
    }

    /// <summary>
    /// �����Լ�ǰ��Ĳ���
    /// </summary>
    /// <param name="target">Ŀ�겡��</param>
    public void UpdatePrePatient(Transform target)
    {
        _prePatient = target.parent;
        MoveTarget(target);
    }

    /// <summary>
    /// ��Ŀ��㣨������ǰһ���ˡ���һ�������豸λ�ã��ƶ�
    /// </summary>
    /// <param name="target"></param>
    private void MoveTarget(Transform target)
    {
        StopAllCoroutines();
        StartCoroutine(Move(target));
    }

    IEnumerator Move(Transform target)
    {
        if (target != null)
        {

            Walk_Active = true;
            _agent.SetDestination(target.position);
            Log.Info($"{gameObject.name} ��ʼѰ· {target.parent.name}");

            NavMeshHit hit;
            if (NavMesh.SamplePosition(target.position, out hit, 10f, NavMesh.AllAreas))
            {
                Vector3 reachablePosition = hit.position;
                _agent.SetDestination(reachablePosition);
                while (Vector3.Distance(transform.position, reachablePosition) > _agent.stoppingDistance)
                {
                    _animator.speed = (_agent.velocity.magnitude / _agent.speed) / 2f + 0.5f;
                    yield return null;
                }
            }

        }

        Log.Info($"{gameObject.name} ����Ŀ�ĵ� {target.name}");

        Walk_Active = false;

        if (target.parent.name.Equals("Exit"))
        {
            FinishInspection_Event?.Invoke(transform);
        }

        if (target.parent.GetComponent<Patient>())
        {
            _instruments[0].EnQueue(this);
        }

        if (target.parent.TryGetComponent(out Instrument instrument))
        {
            float factor = 0f;
            Vector3 targetPos = target.position;
            targetPos.y = 0f;
            Vector3 pos = transform.position;
            pos.y = 0f;

            //Vector3 lookDirection = (targetPos - pos).normalized;
            Vector3 lookDirection = target.forward.normalized;
            while (Vector3.Angle(lookDirection, transform.forward) > 15f)
            {

                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, factor);
                factor += Time.deltaTime / 3f;
                yield return null;
            }


            Log.Info($"{gameObject.name} ��ʼ����");

            yield return StartCoroutine(instrument.StartInspection(this));

            Log.Info($"{gameObject.name} ���ƽ���");

            _instruments.RemoveAt(0);

            _instruments.Add(_inspection.GetNext(_agent));

            MoveNextInspection();


        }

    }
    public void OnSelected()
    {
        Highlighter.Settings.UseMeshOutline = true;
        Highlighter.HighlighterValidate();
        InputManager.Instance.CinemachineVirtualCameraTarget = transform;

    }

    public void OffSelected()
    {
        Highlighter.Settings.UseMeshOutline = false;
        Highlighter.HighlighterValidate();
        InputManager.Instance.CinemachineVirtualCameraTarget = null;

    }
    public void EnterInfoPanel()
    {
        UIManager.Instance.Enter(new PatientInfoPanel(_patientInfo));
    }
}


