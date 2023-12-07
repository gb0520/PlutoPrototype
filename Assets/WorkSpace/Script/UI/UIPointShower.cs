using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB.UsingTweening.Guage;

public class UIPointShower : MonoBehaviour
{
    [SerializeField] private TweeningGuage guage;
    private ZB.Point point;

    public void PointConnect(ZB.Point point)
    {
        this.point = point;
    }
    public void UpdateGuage()
    {
        guage.Change(point.NowPoint / point.MaxPoint);
    }
}
