using UnityEngine;

namespace AI.Sword
{
    class 上清破云剑 : SwordAI
    {
        float high;
        float scale;

        public override float GetColdTime()
        {
            return 10;
        }

        protected override void BeforPlan()
        {
            high = 30;
            scale = 5;
            Speed *= scale;
            SetKinematic(true);
        }

        protected override void Plan()
        {
            // 移动到目标上方
            AddAction(0, () => transform.position = AttackTarget + target.up * high);
            // 瞄准
            AddAction(0, () => LookAttack(AttackTarget));
            // 放大
            AddAction(1, () => AddScale(scale * Vector3.one + scale * Vector3.right));
            // 攻击
            AddAction(0, () => SetKinematic(false));
            AddAction(Vector3.Distance(transform.position, AttackTarget) / Speed, () => Move(transform.forward, Speed*2));
        }
    }
}
