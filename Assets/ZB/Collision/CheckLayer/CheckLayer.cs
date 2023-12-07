using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.CollisionCheck
{
    public class CheckLayer : MonoBehaviour
    {
        public UnityEvent uEvent_Enter;
        public UnityEvent uEvent_Exit;

        public Transform tf_LastTouched { get => tf_lastTouched; }
        public bool Touching { get => touching; }

        [SerializeField] LayerMask targetLayer;
        [SerializeField] Transform tf_lastTouched;
        [SerializeField] bool touching;

        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                tf_lastTouched = collision.transform;
                touching = true;
                uEvent_Enter.Invoke();
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & targetLayer) != 0)
            {
                touching = false;
                uEvent_Exit.Invoke();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) != 0) 
            {
                tf_lastTouched = other.transform;
                touching = true;
                uEvent_Enter.Invoke();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) != 0)
            {
                touching = false;
                uEvent_Exit.Invoke();
            }
        }
    }
}