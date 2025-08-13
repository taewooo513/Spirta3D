using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iInteraction
{
    public void InteractionAction();
    public string GetName();
    public string GetDesc();
}

public class InteractionObject : MonoBehaviour, iInteraction
{
    [SerializeField]
    public string _name;
    public string desc;


    public string GetName()
    {
        return _name;
    }

    public string GetDesc()
    {
        string str = "F´­·¯ " + desc;
        return str;
    }

    virtual public void InteractionAction()
    {
    }
}
