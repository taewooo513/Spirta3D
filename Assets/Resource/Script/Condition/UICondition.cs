using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition headlth;
    public Condition hunger;
    public Condition stamina;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.player.condition.uiCondition = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
