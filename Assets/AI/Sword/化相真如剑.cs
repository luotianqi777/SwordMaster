using UnityEngine;

namespace AI.Sword
{
    class 化相真如剑 : SwordAI
    {
        float scale;
        float distance;
        public override float GetColdTime()
        {
            return 10;
        }

        protected override void BeforPlan()
        {
            scale = 5;
            distance = 20;
            Speed *= scale;
            RotateSpeed = 360 * 3;
            SetKinematic(true);
        }

        protected override void Plan()
        {
            AddAction(0, () => transform.position = AttackTarget - Vector3.up * distance);
            AddAction(0, () => LookAttack(AttackTarget));
            AddAction(1, () => AddScale(scale * Vector3.one + scale * Vector3.right));
            AddAction(0, () =>
            {
                Vector3 local = transform.position;
                local.y = AttackTarget.y;
                // 分裂两个小剑
                Split(0, (sword) => Attack(sword, local + (transform.forward + transform.right / 2) * scale));
                Split(0, (sword) => Attack(sword, local + (transform.forward - transform.right / 2) * scale));
            });
            AddAction(distance / Speed, () => Move(transform.forward));
            AddAction(0, () => SetKinematic(false));
        }

        private void Attack(SwordAI sword, Vector3 local)
        {
            sword.isAttackedFix = false;
            sword.SetKinematic(true);
            sword.SetScale(Vector3.one);
            sword.AddAction(0, () => sword.transform.position = local);
            sword.AddAction(0.5f, () => sword.AddScale(Vector3.one * scale));
            sword.AddAction(0, () => sword.SetKinematic(false));
            bool rotateLeft = Vector3.Dot(transform.right, local - sword.transform.position) > 0;
            sword.AddAction(130 / RotateSpeed, () => sword.Rotate(sword.HeadLocation, (rotateLeft? -1 : 1) * sword.transform.up));
        }

    }
}
