using UnityEngine;

namespace AI.Role
{
    class 李逍遥 : RoleAI
    {

        protected override void Plan()
        {
            Transform target = GetNearRole();
            if (target)
            {
                Attack(target);
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance > Move.Speed * 10)
                {
                    Move.AddAction(0.5f, Move.MoveForword);
                }
                else if (distance < Move.Speed * 5)
                {
                    Move.AddAction(0.5f, Move.MoveBack);
                }
                else
                {
                    Move.AddAction(0.2f, Move.Idle);
                }
                Move.RotateToTarget(target);
            }
            else
            {
                Move.MoveRandom();
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
