using UnityEngine;
using AI.AIManager;

namespace AI.Role
{
    public class MoveAI : BaseAIManager
    {
        // 速度(m/s)
        public float Speed = 2;
        // 转动速度
        public float RotateSpeed { get => Speed * 45; }
        private Animator _ani;
        protected Animator Animator
        {
            get
            {
                if (_ani == null)
                {
                    _ani = GetComponent<Animator>();
                    if (_ani == null)
                    {
                        Debug.LogError("animation is null");
                    }
                }
                return _ani;
            }
        }

        protected override void AfterPlan() { }

        protected override void BeforPlan() { }

        protected override void Plan() { }

        /// <summary>
        /// 播放动画
        /// </summary>
        protected void PlayAnimation()
        {
            Animator.SetBool("IsRun", true);
        }

        // 随机走动
        public void MoveRandom()
        {
            if (Random.Range(0, 2) == 0) { AddAction(1, RotateLeft); }
            else { AddAction(1, () => RotateRight()); }
            if (Random.Range(0, 2) == 0) { AddAction(1, MoveForword); }
            else { AddAction(0.5f, Idle); }
        }

        public void Idle()
        {
            Animator.SetBool("IsRun", false);
        }

        public void MoveForword()
        {
            PlayAnimation();
            transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
        }
        public void MoveBack()
        {
            PlayAnimation();
            transform.Translate(-transform.forward * Speed / 2 * Time.deltaTime, Space.World);
        }
        public void RotateLeft()
        {
            PlayAnimation();
            transform.Rotate(Vector3.up, -Time.deltaTime * RotateSpeed, Space.Self);
        }

        public void RotateRight()
        {
            PlayAnimation();
            transform.Rotate(Vector3.up, Time.deltaTime * RotateSpeed, Space.Self);
        }

        /// <summary>
        /// 转动到目标
        /// </summary>
        /// <param name="target">目标位置</param>
        public void RotateToTarget(Transform target)
        {
            Vector3 dis = target.position - transform.position;
            float time = Vector3.Angle(transform.forward, dis) / RotateSpeed;
            if (Vector3.Cross(transform.forward, dis).y > 0)
            {
                AddAction(time, RotateRight);
            }
            else
            {
                AddAction(time, RotateLeft);
            }
        }

    }
}
