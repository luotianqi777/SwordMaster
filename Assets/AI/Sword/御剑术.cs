using UnityEngine;

namespace AI.Sword
{
    class 御剑术 : SwordAI
    {

        // 分裂数
        private int count;
        // 分裂时间
        private float time;
        // 距离
        private float distance;
        public override float GetColdTime()
        {
            return 5;
        }

        protected override void BeforPlan()
        {
            count = 10;
            Speed = 20;
            time = 1;
            distance = 3;
            RotateSpeed = 360 * 4;
        }

        protected override void Plan()
        {
            AddAction(0, () =>
            {
                // 移动到目标上方随机位置
                transform.position = AttackTarget + (target.up + Random.Range(-1, 1) * target.right + Random.Range(-1, 1) * target.forward) * distance;
                // 瞄准目标
                LookAttack(AttackTarget);
            });
            // 旋转一圈
            AddAction(360 / RotateSpeed, () => Rotate(Vector3.up));
            // 分裂几个发射
            AddAction(time, () =>
            {
                Split(time / count, (sword) =>
                {
                    Attack(sword);
                });
            });
            DestroyAction(0);
        }

        private void Attack(SwordAI sword)
        {
            sword.Move(sword.transform.forward*0.2f + Random.Range(-1, 1f) * sword.transform.right);
            float time = Vector3.Distance(sword.transform.position, AttackTarget) / sword.Speed;
            sword.AddAction(time, () => sword.Move(sword.transform.forward, sword.Speed * 2));
        }
    }
}
