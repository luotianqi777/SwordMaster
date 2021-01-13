using UnityEngine;

namespace AI.Role
{
    class 李逍遥 : RoleAction
    {

        protected override void BeforPlan()
        {
        }

        protected override void Plan()
        {
            Transform target = GetNearRole();
            if (target)
            {
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance > Speed * 10)
                {
                    AddAction(0.5f, MoveForword);
                }
                else if (distance < Speed * 5)
                {
                    AddAction(0.5f, MoveBack);
                }
                else
                {
                    AddAction(0.2f, Idle);
                    RotateToTarget(target);
                    Attack(target);
                }
            }
            else
            {
                MoveRandom();
            }
        }

        public void Attack(Transform target)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    GetSword<Sword.万剑诀>(target);
                    break;
                case 1:
                    GetSword<Sword.逍遥神剑>(target);
                    break;
                case 2:
                    GetSword<Sword.无名剑法>(target);
                    break;
            }
        }
    }
}
