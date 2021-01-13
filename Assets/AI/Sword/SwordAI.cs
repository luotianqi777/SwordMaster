using UnityEngine;
using System;
using AI.AIManager;

namespace AI.Sword
{
    public abstract class SwordAI : BaseAIManager
    {
        // 冷却时间
        public float ColdTime = 3;
        // 攻击目标
        protected Transform target;
        // 攻击目标位置
        protected Vector3 AttackTarget { get=> target? target.position + Vector3.up:transform.forward * 5 + transform.position; }
        // 速度(m/s)
        public float Speed = 10;
        // 转动速度(°/s)
        public float RotateSpeed = 360;
        // 最后一次分裂的时间
        private float lastSplitTime = 0;
        // 剑柄坐标
        public Vector3 HeadLocation { get => transform.position - transform.forward * 0.75f; }
        // 剑尾坐标
        public Vector3 TailLocation { get => transform.position + transform.forward * 0.75f; }

        /// <summary>
        /// 设置攻击目标
        /// </summary>
        /// <param name="target">目标坐标</param>
        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        /// <summary>
        /// 分裂
        /// </summary>
        /// <param name="time">分裂间隔</param>
        /// <param name="action">新对象的行为</param>
        public void Split(float time,Action<SwordAI> action)
        {
            if (timeout - lastSplitTime >= time)
            {
                GameObject newObject = Instantiate(gameObject);
                SwordAI sword = newObject.GetComponent<SwordAI>();
                action.Invoke(sword);
                // 计划任务：清空所有计划后添加销毁任务
                // 清空是因为在执行action后还有克隆对象本身的Init任务
                // 添加顺序为 action=>Clear+Destroy=>Init
                // 这里添加了Clear计划就保证执行的仅有action+Destroy
                sword.AddAction(0, () =>
                {
                    sword.ClearAllAction();
                    sword.DestroyAction();
                });
                lastSplitTime = timeout;
            }
        }

        /// <summary>
        /// 绕某一轴旋转(自身轴)
        /// </summary>
        /// <param name="axis">旋转轴</param>
        public void Rotate(Vector3 axis)
        {
            transform.Rotate(axis * Time.deltaTime * RotateSpeed);
        }

        /// <summary>
        /// 绕某一点的轴旋转(世界坐标)
        /// </summary>
        /// <param name="point">围绕点</param>
        /// <param name="axis">旋转</param>
        public void Rotate(Vector3 point, Vector3 axis)
        {
            transform.RotateAround(point, axis, Time.deltaTime * RotateSpeed);
        }

        /// <summary>
        /// 向一个位置移动
        /// </summary>
        /// <param name="vector">方向向量(世界坐标)</param>
        /// <param name="speed">速度</param>
        public void Move(Vector3 vector, float speed) 
        {
            transform.Translate(vector.normalized*speed*Time.deltaTime, Space.World);
        }

        /// <summary>
        /// 向一个位置移动
        /// </summary>
        /// <param name="vector">方向向量(世界坐标)</param>
        public void Move(Vector3 vector) 
        {
            Move(vector, Speed);
        }

        // 攻击朝向
        public void LookAttack(Vector3 vector)
        {
            transform.LookAt(vector);
        }

        // 防御朝向
        public void LookDefence(Vector3 vector)
        {
            // TODO
        }

        // 设置Kinematic是否开启
        public void SetKinematic(bool state)
        {
            GetComponent<Rigidbody>().isKinematic = state;
        }

        // 获取Kinematic设置
        public bool GetKinematic()
        {
            return GetComponent<Rigidbody>().isKinematic;
        }

        /// <summary>
        /// 添加倍率
        /// </summary>
        /// <param name="scale">倍率</param>
        public void AddScale(Vector3 scale)
        {
            transform.localScale += scale*Time.deltaTime;
        }

        /// <summary>
        /// 设置倍率
        /// </summary>
        /// <param name="scale">倍率</param>
        public void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
        }

        // 销毁任务
        private void DestroyAction()
        {
            // 等待一段时间后销毁
            AddAction(10, ()=> { });
            AddAction(0, () => { Destroy(gameObject); });
        }

        private bool isColled = false;
        // 碰撞
        private void OnCollisionEnter(Collision collision)
        {
            if (isColled || GetKinematic() || gameObject.tag == collision.gameObject.tag) { return; }
            isColled = true;
            transform.Translate(Vector3.forward * UnityEngine.Random.Range(0, 0.5f), Space.Self);
            if (collision.gameObject.tag != "Group")
            {
                transform.SetParent(collision.transform, true);
            }
            ClearAllAction();
            // 额外等待一段时间
            DestroyAction();
        }

        protected override void AfterPlan() {
            DestroyAction();
        }

    }
}
