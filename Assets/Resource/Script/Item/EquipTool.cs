using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool isAttack;
    public float attackDist;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamagae;
    public int damage;
    private Camera camera;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    // Update is called once per frame
    public float useStamina;
    void Update()
    {

    }

    public override void OnAttackInput()
    {
        if (!isAttack)
        {
            if (CharacterManager.Instance.player.condition.UseStamina(useStamina))
            {
                isAttack = true;
                animator.SetTrigger("Attack");
                Invoke("OnIsAttack", attackRate);
            }
        }
    }

    void OnIsAttack()
    {
        isAttack = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackDist))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource res))
            {
                res.Gether(hit.point, hit.normal);
            }
        }
    }
}
