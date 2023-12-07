using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Object
{
    public abstract class FlexbieObject : MonoBehaviour
    {
        Transform origianlParent;

        public void Attach(Transform newParent)
        {
            OnAttach();
            gameObject.SetActive(true);
            transform.SetParent(newParent);
        }
        public void Detach()
        {
            OnDetach();
            transform.SetParent(origianlParent);
        }

        protected abstract void OnAttach();
        protected abstract void OnDetach();
    }
}