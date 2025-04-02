using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIToolTip : MouseMoveToolTip
{
    public void Enable(IPointOfInterest poi)
    {
        toolTipPresenter.Enable(poi.GetInfo.nameOfPoint, poi.GetInfo.type.ToString(), poi.GetInfo.desc);
    }
}
