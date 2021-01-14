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
            Speed = 20;
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
                        float value1 = Random.Range(-1, 1f);
                        float value2 = Mathf.Sqrt(1 - value1 * value1);
                        value2 = Random.Range(-value2, value2);
                        Vector3 range = value2 * Vector3.forward + value1 * Vector3.right;
                        range *= rangeSize;
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
