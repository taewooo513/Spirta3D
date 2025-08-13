using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iInteraction
{
    public void InteractionAction();
    public string GetName();
}

public class InteractionObject : MonoBehaviour, iInteraction
{
    [SerializeField]
    private string name;

    public string GetName()
    {
        return name;
    }

    virtual public void InteractionAction()
    {
    }
}
