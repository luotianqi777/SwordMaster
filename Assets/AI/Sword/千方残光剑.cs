using UnityEngine;

namespace AI.Sword
{
    class 千方残光剑 : SwordAI
    {
        public float count;
        public float high;
        public float waitTime;
        protected override void BeforPlan()
        {
            count = 1000;
            high = 30;
            waitTime = 10;
            Speed = 20;
            RotateSpeed = 360;
            SetKinematic(true);
        }

        protected override void Plan()
        {
            Vector3 vector = AttackTarget - transform.position + Vector3.up * high ;
            float time = (vector.magnitude - high) / Speed;
            AddAction(high / Speed, () => Move(Vector3.up));
            AddAction(time, () =>
            {
                Move(vector);
            });
            AddAction(waitTime, () => { 
                Split(waitTime/count, Attack);
            });
            AddAction(0, () => {
                SetKinematic(false);
                LookAttack(AttackTarget);
                });
            AddAction(1, () => Move(transform.forward));
        }

        private void Attack(SwordAI sword)
        {
            // 空中盘旋
            // sword.AddAction(waitTime/2, () => SubAttack(sword));
            sword.AddAction(Random.Range(waitTime/2, waitTime), () => SubAttack(sword));
            // 瞄准
            sword.AddAction(0, () =>
            {
                sword.SetKinematic(false);
                sword.LookAttack(AttackTarget);
                // 修正角度
                float value = 5;
                Vector3 fix = new Vector3(Random.Range(-value, value), Random.Range(-value, value), 0);
                sword.transform.Rotate(fix, Space.Self);
            });
            sword.AddAction((sword.transform.position - AttackTarget).magnitude / Speed, () => sword.Move(sword.transform.forward, sword.Speed * 2));
        }

        private void SubAttack(SwordAI sword)
        {
            Vector3 center = AttackTarget + Vector3.up * high;
            sword.Move(center - sword.transform.position, high/Speed);
            // 修正水平位置
            Vector3 fix = sword.transform.position;
            fix.y = center.y;
            sword.transform.position = fix;
            sword.LookAttack(AttackTarget);
            // 修正角度
            sword.transform.Rotate(Vector3.right, sword.transform.rotation.x / 2, Space.Self);
            sword.Rotate(AttackTarget, Vector3.up);
        }

        public override float GetColdTime()
        {
            return 25;
        }
    }
}
