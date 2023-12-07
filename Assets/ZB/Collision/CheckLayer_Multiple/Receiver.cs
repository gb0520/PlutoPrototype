using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.CollisionCheck
{
    public class Receiver : MonoBehaviour
    {
        public List<Sender> Senders { get => senders; }
        public bool touching { get => senders.Count > 0; }

        [SerializeField] private List<Sender> senders;

        public Sender NearSender()
        {
            Sender result = null;
            if (senders.Count > 0)
            {
                result = senders[0];
                for (int i = 1; i < senders.Count; i++)
                {
                    if (Vector3.Distance(transform.position, senders[i].transform.position) <
                        Vector3.Distance(transform.position, result.transform.position))
                    {
                        result = senders[i];
                    }
                }
            }
            return result;
        }
        public void ReceiveEnter(Sender sender)
        {
            senders.Add(sender);
        }
        public void ReceiveExist(Sender sender)
        {
            senders.Remove(sender);
        }

        private void Awake()
        {
            senders = new List<Sender>();
        }
    }
}