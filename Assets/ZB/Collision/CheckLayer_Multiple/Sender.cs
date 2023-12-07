using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.CollisionCheck
{
    public class Sender : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayer;
        private List<Receiver> receivers;

        private void SendEnter(Receiver receiver)
        {
            receivers.Add(receiver);
            receiver.ReceiveEnter(this);
        }
        private void SendExit(Receiver receiver)
        {
            receivers.Remove(receiver);
            receiver.ReceiveExist(this);
        }
        private void ExitAll()
        {
            for (int i = 0; i < receivers.Count; i++)
            {
                receivers[i].ReceiveExist(this);
            }
            receivers.Clear();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                Receiver receiver;
                if (collision.transform.TryGetComponent(out receiver))
                {
                    SendEnter(receiver);
                }
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                Receiver receiver;
                if (collision.transform.TryGetComponent(out receiver))
                {
                    SendExit(receiver);
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) != 0)
            {
                Receiver receiver;
                if (other.transform.TryGetComponent(out receiver))
                {
                    SendEnter(receiver);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) != 0)
            {
                Receiver receiver;
                if (other.transform.TryGetComponent(out receiver))
                {
                    SendExit(receiver);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                Receiver receiver;
                if (collision.transform.TryGetComponent(out receiver))
                {
                    SendEnter(receiver);
                }
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                Receiver receiver;
                if (collision.transform.TryGetComponent(out receiver))
                {
                    SendExit(receiver);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                Receiver receiver;
                if (collision.transform.TryGetComponent(out receiver))
                {
                    SendEnter(receiver);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                Receiver receiver;
                if (collision.transform.TryGetComponent(out receiver))
                {
                    SendExit(receiver);
                }
            }
        }

        private void Awake()
        {
            receivers = new List<Receiver>();
        }
        private void OnDisable()
        {
            ExitAll();
        }
        private void OnDestroy()
        {
            ExitAll();
        }
    }
}