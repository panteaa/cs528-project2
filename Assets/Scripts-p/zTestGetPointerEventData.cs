using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zinnia.Cast;
using Zinnia.Data.Type;
using Zinnia.Extension;
using Zinnia.Visual;
using Zinnia.Process;

public class TestGetPointerEventData : MonoBehaviour
{
    public GameObject g;

    public void OnSelect(Zinnia.Pointer.ObjectPointer.EventData data)
    {
        g = data.CollisionData.transform.gameObject;
        g.SetActive(false);
    }


}



