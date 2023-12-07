using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.CollisionCheck
{
    public class ReceiverTestLog : MonoBehaviour
    {
        Receiver receiver;

        private void Awake()
        {
            transform.TryGetComponent(out receiver);
        }
        void Update()
        {
            if (receiver.NearSender() != null) 
                Debug.Log($"{receiver.gameObject.name} 와 가장 가까운 Sender {receiver.NearSender().gameObject.name}");
        }
    }
}