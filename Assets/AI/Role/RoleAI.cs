using AI.AIManager;
using UnityEngine;

namespace AI.Role
{
    [RequireComponent(typeof(MoveAI))]
    public abstract class RoleAI : BaseAIManager
    {
        // 剑预制体
        public Transform SwordPrefab;
        protected MoveAI Move;
        protected override void BeforPlan() { Move = GetComponent<MoveAI>(); }
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
                if (nowDistance > newDistance) { target = role.transform; }
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
            AddAction(action.ColdTime, () => { });
            return sword;
        }
    }
}
