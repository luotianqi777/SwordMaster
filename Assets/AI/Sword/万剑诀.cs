using UnityEngine;

namespace AI.Sword
{
    class 万剑诀 : SwordAction
    {
        // 分裂书
        public int count;
        // 范围
        public float rangeSize;

        protected override void BeforPlan()
        {
            count = 50;
            rangeSize = 5;
            Speed = 20;
            SetKinematic(true);
        }

        protected override void Plan()
        {
            // 竖直起飞
            LookAttack(transform.position - Vector3.up);
            AddAction(2, () => Move(-transform.forward));
            AddAction(0, () =>
            {
                // 瞬移到头上
                Vector3 targetPosition = target;
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
                        sword.AddAction(3, () => sword.Move(sword.transform.forward, Speed * 3));
                    });
                }
                Destroy(gameObject);
            });
        }
    }
}
