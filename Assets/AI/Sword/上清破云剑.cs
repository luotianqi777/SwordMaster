using UnityEngine;

namespace AI.Sword
{
    class 上清破云剑 : SwordAI
    {
        public override float GetColdTime()
        {
            return 10;
        }

        protected override void BeforPlan()
        {
            Speed = 40;
        }

        protected override void Plan()
        {
            throw new System.NotImplementedException();
        }
    }
}
