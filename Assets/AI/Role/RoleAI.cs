using AI.AIManager;
using UnityEngine;

namespace AI.Role
{
    public abstract class RoleAI : BaseAIManager
    {
        // 速度(m/s)
        public float Speed = 2;
        // 转动速度
        public float RotateSpeed { get => Speed * 45; }
        // 剑预制体
        public Transform SwordPrefab;
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

        protected override void AfterPlan() { AddAction(0, () => { Plan(); AfterPlan(); }); }

        /// <summary>
        /// 获取距离自身最近的目标
        /// </summary>
        /// <param name="self">自身的GameObject</param>
        /// <returns>目标的GameObject</returns>
        protected Transform GetNearRole()
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
                if (nowDistance < newDistance) { target = role.transform; }
            }
            return target;
        }

        /// <summary>
        /// 获取距离自身最近的目标的最佳攻击Position
        /// </summary>
        /// <param name="role">目标</param>
        /// <returns>目标的位置</returns>
        protected Vector3 GetRoleAttackPostion(Transform role = null)
        {
            role = role ? role : GetNearRole();
            if (role) { return role.position + Vector3.up; }
            else { return transform.position + Vector3.up + transform.forward * 10; }
        }

        /// <summary>
        /// 创建一把没有行为的剑，位于角色正前方
        /// </summary>
        /// <returns>剑的位置</returns>
        protected Transform GetSword()
        {
            Transform sword = Instantiate(SwordPrefab);
            sword.position = transform.position + transform.forward + transform.up;
            sword.LookAt(sword.position - Vector3.up);
            return sword;
        }

        /// <summary>
        /// 获取一把有行为的剑
        /// </summary>
        /// <typeparam name="T">行为</typeparam>
        /// <typeparam name="target">目标</typeparam>
        /// <returns>剑的位置</returns>
        protected Transform GetSword<T>(Transform target) where T : Sword.SwordAI
        {
            Transform sword = GetSword();
            sword.gameObject.AddComponent<T>();
            T action = sword.GetComponent<T>();
            action.SetTarget(target);
            AddAction(action.coldTime, () => { });
            return sword;
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
            transform.Rotate(Vector3.up, Time.deltaTime * RotateSpeed , Space.Self);
        }

        /// <summary>
        /// 转动到目标
        /// </summary>
        /// <param name="target">目标位置</param>
        public void RotateToTarget(Transform target)
        {
            Vector3 dis = target.position - transform.position;
            float time = Vector3.Angle(transform.forward, dis)/RotateSpeed;
            if (Vector3.Cross(transform.forward,dis).y>0)
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
