using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthPoint : MonoBehaviour
{
    [SerializeField] private Transform transform;

    private float maxSize = 1;

    public void Active()
    {
        transform.gameObject.SetActive(true);
    }
    public void UpdateGuage(float max, float now)
    {
        transform.localScale = new Vector2((now / max) * maxSize, 0.1f);
    }
}
