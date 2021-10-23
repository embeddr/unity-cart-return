// Object container behavior
//
// This utility script simply holds a single GameObject reference, whether from the scene or
// a prefab. The object can be set by the inspector in Unity and accessed at runtime.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
    public GameObject Object {
        get { return _object; }
        set { _object = value; }
    }

    [Tooltip("Game object to hold")] 
    [SerializeField]
    private GameObject _object;
}