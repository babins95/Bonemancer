using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    public virtual void OnInstanceCreated() { }
    public virtual void OnCloneActivated() { }
}
