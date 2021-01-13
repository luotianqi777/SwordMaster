using AI.AIManager;
using AI.Sword;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace AI.Role
{
    [RequireComponent(typeof(MoveAI))]
    public abstract class RoleAI : BaseAIManager
    {
        // 剑预制体
        public Transform SwordPrefab;
        // 操作列表
        protected MoveAI Move;
        // 可用的剑操作列表
        private List<Action<Transform>> swordList = new List<Action<Transform>>();
        protected override void BeforPlan() { Move = GetComponent<MoveAI>(); AddSwords(); }
        protected override void AfterPlan() { AddAction(0, () => { Plan(); AfterPlan(); }); }
        protected override void Plan()
        {
            Transform target = Move.GetNearRole();
            if (target)
            {
                Move.AddAction(0, () => Move.RotateToTarget(target));
                AddAction(0, () => Attack(target));
            }
        }
        // 添加技能组
        protected abstract void AddSwords();
        // 添加可用的剑
        protected void AddSword<T>()where T:SwordAI
        {
            swordList.Add(GetSword<T>);
        }
        // 攻击目标
        private void Attack(Transform target)
        {
            swordList[UnityEngine.Random.Range(0, swordList.Count)].Invoke(target);
        }

        /// <summary>
        /// 创建一把没有行为的剑，位于角色正前方
        /// </summary>
        /// <returns>剑的位置</returns>
        private Transform GetSword()
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
        private void GetSword<T>(Transform target) where T : SwordAI
        {
            Transform sword = GetSword();
            sword.gameObject.AddComponent<T>();
            T action = sword.GetComponent<T>();
            action.SetTarget(target);
            AddAction(action.GetColdTime(), () => { });
        }
    }
}
