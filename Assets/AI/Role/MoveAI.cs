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

        protected override void AfterPlan() {}

        protected override void BeforPlan() { }

        protected override void Plan() { 
            Transform target = GetNearRole();
            if (target)
            {
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance > Speed * 10)
                {
                    AddAction(0.5f, MoveForword);
                }
                else if (distance < Speed * 5)
                {
                    AddAction(0.5f, MoveBack);
                }
                else
                {
                    AddAction(1, Idle);
                }
            }
            else
            {
                MoveRandom();
            }
        }

        /// <summary>
        /// 获取距离自身最近的目标
        /// </summary>
        /// <param name="self">自身的GameObject</param>
        /// <returns>目标的GameObject</returns>
        public Transform GetNearRole()
        {
            Transform target = null;
            float nowDistance;
            float newDistance;
            foreach (GameObject role in GameObject.FindGameObjectsWithTag("Role"))
            {
                if (role == gameObject) { continue; }
                if (target == null) { target = role.transform; continue; }
                nowDistance = Vector3.Distance(transform.position, target.transform.position);
                newDistance = Vector3.Distance(transform.position, role.transform.position);
                if (nowDistance > newDistance) { target = role.transform; }
            }
            return target;
        }

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
