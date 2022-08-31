using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OutlineController))]
public abstract class InteractiveObject : MonoBehaviour
{
    public abstract void OnInteract();
}
