using UnityEngine;

namespace AI.Sword
{
    class 逍遥神剑 : SwordAI
    {
        // 分裂数
        public int count;
        // 角度范围
        public float range;

        public override float GetColdTime()
        {
            return 5;
        }

        protected override void BeforPlan()
        {
            count = 10;
            range = 100;
            RotateSpeed = 360 * 10;
            Speed = 20;
            SetKinematic(true);
        }

        protected override void Plan()
        {
            Vector3 vector = AttackTarget - transform.position;
            float time = vector.magnitude / Speed;
            AddAction(0, () => LookAttack(AttackTarget));
            // 前进
            AddAction(time, () => Move(vector));
            // 分裂
            AddAction(0, () =>
            {
                for (int i = 0; i < count; i++)
                {
                    Split(0, (sword) =>
                    {
                        // 调整角度
                        sword.transform.Rotate(new Vector3(0, i * (range / count) - range / 2, 0));
                        SubAction(sword);
                    });
                }
            });
            AddAction(0, () => { SetKinematic(false); });
            // 继续前进
            AddAction(time, () => Move(transform.forward, Speed));
            // 销毁
            DestroyAction();
        }

        private void SubAction(SwordAI sword)
        {
            float time = 1;
            Vector3 vector = sword.transform.forward;
            // 边转边退
            sword.AddAction(time, () =>
            {
                sword.Rotate(Vector3.up);
                sword.Move(-vector,sword.Speed/2);
            });
            // 瞄准
            sword.AddAction(0, () =>
            {
                sword.LookAttack(AttackTarget);
                sword.SetKinematic(false);
            });
            // 前进
            sword.AddAction(time, () =>
            {
                sword.Move(vector, sword.Speed * 2);
            });
        }

    }
}
