using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB
{
    [System.Serializable]
    public class Point
    {
        public float NowPoint { get => nowPoint; }
        public float MaxPoint { get => maxPoint; }
        public float MinPoint { get => minPoint; }
        public UnityEvent pointMaxEvent;
        public UnityEvent pointMinEvent;
        public UnityEvent pointVariableEvent;

        [SerializeField] private float minPoint;
        [SerializeField] private float maxPoint;
        [SerializeField] private float nowPoint;

        public void InitPoint(float maxPoint, float minPoint = 0)
        {
            this.maxPoint = maxPoint;
            this.minPoint = minPoint;
            nowPoint = maxPoint;
        }

        public void PointVariable(float point)
        {
            nowPoint = Mathf.Clamp(nowPoint + point, 0, maxPoint);

            if (pointVariableEvent != null)
                pointVariableEvent.Invoke();
            if (nowPoint >= maxPoint &&
                pointMaxEvent != null) 
            {
                pointMaxEvent.Invoke();
            }
            if (nowPoint <= 0 &&
                pointMinEvent != null)
            {
                pointMinEvent.Invoke();
            }
        }
    }
}