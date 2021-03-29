using UnityEngine;

class FollowState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        Player.Instance.AddAttacker(parent);
        this.parent = parent;
    }

    public void Exit()
    {
        parent.MyDirection = Vector2.zero;
    }

    public void Update()
    {
        if (parent.MyTarget != null)
        {
            //Find the target's direction
            parent.MyDirection = (parent.MyTarget.transform.position - parent.transform.position).normalized;

            //Moves the enemy towards the target
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MyTarget.transform.position, parent.MySpeed * Time.deltaTime);

            float distance = Vector2.Distance(parent.MyTarget.position,parent.transform.position);

            if(distance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        if (!parent.InRange)
        {
            parent.ChangeState(new EvadeState());
        }
       
        
    }
}
