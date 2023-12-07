using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Pluto
{
    public class Player : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private Animator ani;
        [SerializeField] private SpriteRenderer spr;
        [SerializeField] private PhotonView photonView;

        [SerializeField] private Transform lookPoint;
        [SerializeField] private Transform headInfoTransform;

        [SerializeField] private ZB.CollisionCheck.Receiver hitArea;
        [SerializeField] private GameObject atkArea;

        [SerializeField] private ZB.Point healthPoint;
        [SerializeField] private ZB.Point runPoint;
        [SerializeField] private UIPointShower healthPointShower;
        [SerializeField] private UIPointShower runPointShower;
        [SerializeField] private HealthPoint healthPointNotMine;

        [SerializeField] private PlayerNameShow playerNameShow;


        [Space]
        [SerializeField] private bool isMine;

        private float headInfoDist = 1;

        //�̵�����
        private bool canMove { get => !attacking && !hitting; }
        private float nowSpeed = 5;
        private float walkSpeed = 5;
        private float runSpeed = 10;
        private float runPointVariable = 25;

        //�ǰݰ���
        private bool hitAreaEntering => hitArea.touching;
        private bool hitting;
        private float hitTime = 0.8f;
        private WaitForSeconds wfs_hit;
        private bool isInvincible;
        private float invincibleTime = 0.075f;
        private WaitForSeconds wfs_invincible;

        private float hitDmg = 10;

        //���ݰ���
        [SerializeField] private bool attacking;
        private float atkTime = 0.65f;
        private WaitForSeconds wfs_atk;

        //�ε巯�� ��ġ ����ȭ
        private Vector3 curPos;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                curPos = (Vector3)stream.ReceiveNext();
            }
        }
        public void OnInputDown_Atk()
        {
            if (!attacking && !hitting)
            {
                AtkCycle = atkCycle();
                StartCoroutine(AtkCycle);
            }
        }
        public void OnInputDown_Run()
        {
            nowSpeed = runSpeed;
        }
        public void OnInputUp_Run()
        {
            nowSpeed = walkSpeed;
        }
        public void OnDirection2DInput_Run(Vector2 dir)
        {
            if (dir == Vector2.zero)
            {
                //����
                rigidbody.velocity = Vector2.zero;
            }
            else if (canMove) 
            {
                //�̵�
                rigidbody.velocity = dir * nowSpeed;
                photonView.RPC("ChangeLookPoint", RpcTarget.AllBuffered, dir);
            }
        }

        private void Awake()
        {
            isMine = photonView.IsMine;
            OnInit(isMine);
        }
        private void Update()
        {
            OnUpdate(isMine);
        }
        private void OnInit(bool isMine)
        {
            healthPoint = new ZB.Point();
            runPoint = new ZB.Point();
            healthPoint.InitPoint(100);
            runPoint.InitPoint(100);

            if (isMine)
            {
                CameraManager.instance.CamForMyPlayerFocusThis(transform);
                UserInputSender.instance.PlayerConnect(this);

                //ü������Ʈ
                healthPointShower = GameObject.Find("HealthPoint").GetComponent<UIPointShower>();
                healthPointShower.PointConnect(healthPoint);
                if (healthPoint.pointVariableEvent == null) healthPoint.pointVariableEvent = new UnityEngine.Events.UnityEvent();
                healthPoint.pointVariableEvent.AddListener(healthPointShower.UpdateGuage);
                if (healthPoint.pointMinEvent == null) healthPoint.pointMinEvent = new UnityEngine.Events.UnityEvent();
                healthPoint.pointMinEvent.AddListener(OnDie);
                healthPointShower.UpdateGuage();

                //�޸�������Ʈ
                runPointShower = GameObject.Find("RunPoint").GetComponent<UIPointShower>();
                runPointShower.PointConnect(runPoint);
                if (runPoint.pointVariableEvent == null) runPoint.pointVariableEvent = new UnityEngine.Events.UnityEvent();
                runPoint.pointVariableEvent.AddListener(runPointShower.UpdateGuage);
                runPointShower.UpdateGuage();
            }
            else
            {
                healthPointNotMine.Active();
                healthPointNotMine.UpdateGuage(1, 1);
            }
            atkArea.SetActive(false);

            //�г��� ����
            playerNameShow.SetName(
                isMine ? PhotonNetwork.NickName : photonView.Owner.NickName,
                isMine);

        }
        private void OnUpdate(bool isMine)
        {
            if (isMine)
            {
                //�ǰ�
                if (!isInvincible && !hitting && hitAreaEntering)
                {
                    AtkCancel();
                    //healthPoint.PointVariable(-hitDmg);
                    photonView.RPC("HealthPointVariable", RpcTarget.AllBuffered, -hitDmg);
                    StartCoroutine(hitCycle());

                    //���
                    if (healthPoint.NowPoint <= 0)
                    {
                        Network.NetworkManager.instance.RespawnWindowActive();
                    }
                }

                //�޸�������� ����Ʈ ����
                if (nowSpeed >= runSpeed && canMove)
                {
                    runPoint.PointVariable(-runPointVariable * Time.deltaTime);
                    if (runPoint.NowPoint <= 0)
                        nowSpeed = walkSpeed;
                }
                else
                {
                    runPoint.PointVariable(+runPointVariable * Time.deltaTime);
                }
            }
            else
            {
                //�ٸ����� ��ġ����ȭ
                if ((transform.position - curPos).sqrMagnitude >= 100)
                    transform.position = curPos;
                else
                    transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);

                //ü�¹� ����
                healthPointNotMine.UpdateGuage(healthPoint.MaxPoint, healthPoint.NowPoint);
            }

            //�Ӹ� �� ���� ��ġ ������Ʈ
            headInfoTransform.position = transform.position + Vector3.up * headInfoDist;
        }

        [PunRPC]
        private void AtkAreaActive(bool isActive)
        {
            atkArea.SetActive(isActive);
        }
        [PunRPC]
        private void ChangeLookPoint(Vector2 position)
        {
            lookPoint.localPosition = position.normalized * 1;
            lookPoint.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg);
        }
        [PunRPC]
        private void HealthPointVariable(float value)
        {
            healthPoint.PointVariable(-hitDmg);
        }
        [PunRPC]
        private void Die()
        {
            UserInputSender.instance.PlayerDisconnect();
            Destroy(transform.parent.gameObject);
        }

        public void OnDie()
        {
            photonView.RPC("Die", RpcTarget.AllBuffered);
        }

        private IEnumerator HitCycle;
        private IEnumerator hitCycle()
        {
            hitting = true;
            yield return new WaitForSeconds(hitTime);
            hitting = false;
            StartCoroutine(invincibleCycle());
        }
        private void AtkCancel()
        {
            if (AtkCycle != null)
                StopCoroutine(AtkCycle);
            attacking = false;
            photonView.RPC("AtkAreaActive", RpcTarget.AllBuffered, false);
        }
        private IEnumerator AtkCycle;
        private IEnumerator atkCycle()
        {
            attacking = true;
            photonView.RPC("AtkAreaActive", RpcTarget.AllBuffered, true);
            yield return new WaitForSeconds(atkTime);
            attacking = false;
            photonView.RPC("AtkAreaActive", RpcTarget.AllBuffered, false);
        }
        private IEnumerator invincibleCycle()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibleTime);
            isInvincible = false;
        }

        private void OnDestroy()
        {
            if (isMine)
            {
                //ü������Ʈ
                healthPoint.pointVariableEvent.RemoveListener(healthPointShower.UpdateGuage);
                healthPoint.pointMinEvent.RemoveListener(OnDie);

                //�޸�������Ʈ
                runPoint.pointVariableEvent.RemoveListener(runPointShower.UpdateGuage);
            }

            Destroy(transform.parent.gameObject);
        }
        private void OnApplicationQuit()
        {
            OnDie();
        }
    }
}