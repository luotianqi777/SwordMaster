using UnityEngine;

namespace AI.Sword
{
    class 无名剑法 : SwordAI
    {

        protected override void BeforPlan()
        {
            Speed = 20;
            RotateSpeed = 360;
            ColdTime = 5;
            SetKinematic(true);
        }

        protected override void Plan()
        {
            Vector3 vector = AttackTarget + Vector3.up * 3 - transform.position;
            float time = vector.magnitude / Speed;
            AddAction(time, () =>
            {
                Rotate(Vector3.up);
                Move(vector);
                Split(0.05f, Attack);
            });
            AddAction(0, () => {
                SetKinematic(false);
                LookAttack(AttackTarget);
                });
            AddAction(1, () => Move(transform.forward));
        }

        public void Attack(SwordAI sword)
        {
            sword.AddAction(2, () =>
            {
                sword.Rotate(Vector3.up);
                sword.Rotate(AttackTarget, Vector3.up);
            });
            sword.AddAction(0, () =>
            {
                sword.SetKinematic(false);
                sword.LookAttack(AttackTarget);
            });
            sword.AddAction(Random.Range(0, 1f), () => { });
            sword.AddAction(3, () => sword.Move(sword.transform.forward));
        }
    }
}
