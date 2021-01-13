using UnityEngine;

namespace AI.Sword
{
    class 万剑诀 : SwordAI
    {
        // 分裂书
        public int count;
        // 范围
        public float rangeSize;
        // 高度
        public float high;

        public override float GetColdTime()
        {
            return 6;
        }

        protected override void BeforPlan()
        {
            count = 200;
            rangeSize = 5;
            Speed = 30;
            high = 30;
            SetKinematic(true);
        }

        protected override void Plan()
        {
            // 竖直起飞
            LookAttack(transform.position - Vector3.up);
            AddAction(high/Speed, () => Move(-transform.forward));
            AddAction(0, () =>
            {
                // 瞬移到头上
                Vector3 targetPosition = AttackTarget;
                targetPosition.y = transform.position.y;
                transform.position = targetPosition;
            });
            SetKinematic(false);
            // 分裂后下降
            AddAction(0, () =>
            {
                for (int i = 0; i < count; i++)
                {
                    Split(0, (sword) =>
                    {
                        Vector3 range = Random.Range(-rangeSize, rangeSize) * Vector3.right + Random.Range(-rangeSize, rangeSize) * Vector3.forward;
                        sword.transform.position += range;
                        // 随机等待一段时间后发射
                        sword.AddAction(Random.Range(0, 2f), () => { });
                        sword.AddAction(high/Speed, () => sword.Move(sword.transform.forward, Speed * 2));
                    });
                }
                Destroy(gameObject);
            });
        }
    }
}
