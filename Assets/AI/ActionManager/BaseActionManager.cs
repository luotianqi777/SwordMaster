using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AI.ActionManager
{
    public abstract class BaseActionManager : MonoBehaviour
    {
        // 行为
        private class Actions{
            public float time;
            public Action action;
            public Actions(float time, Action action)
            {
                this.time = time;
                this.action = action;
            }
        }
        // 计时器
        protected float timeout;
        // 行为队列
        private readonly Queue<Actions> queue = new Queue<Actions>();

        public void Start()
        {
            timeout = 0;
            BeforPlan();
            Plan();
            AfterPlan();
        }

        public void Update()
        {
            if (queue.Count == 0) { return; }
            timeout += Time.deltaTime;
            Actions action = queue.Peek();
            if (timeout >= action.time)
            {
                queue.Dequeue();
                timeout = 0;
            }
            action.action();
        }

        // 初始化任务
        protected abstract void Plan();
        protected abstract void BeforPlan();
        protected abstract void AfterPlan();

        /// <summary>
        /// 添加计划任务
        /// </summary>
        /// <param name="time">维持时间</param>
        /// <param name="func">任务</param>
        public void AddAction(float time, Action func)
        {
            queue.Enqueue(new Actions(time, func));
        }

        // 清空所有任务
        public void ClearAllAction()
        {
            queue.Clear();
        }

    }
}