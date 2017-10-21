using System;
using Network.Timer;

namespace Network
{
    public class StateBase
    {
        UInt64 createTime = 0;
        protected int id = 0;

        public int ID { get { return id; } }

        public StateBase()
        {
            createTime = TickerPolicy.Ticker.GetTick();
        }

        public float ExistTime()
        {
            return (TickerPolicy.Ticker.GetTick() - createTime) / 1000.0f;
        }

        public virtual bool IsBlocked(object obj) { return false; }
        public virtual bool Transition(Object obj,StateBase targetState) { return true; }
        public virtual bool Prepare(Object obj) { return true; }

        public virtual void Renew(object obj, StateBase newState) { }
        public virtual void Refresh(object obj) { }
        public virtual void OnEnter(Object obj) { }
        public virtual void OnLeave(Object obj) { }
        public virtual void Update(Object obj) { }
    }

    public class StateMachine
    {
        StateBase currState = null;
        bool preparePending = false;

        public StateBase CurrentState { get { return currState; } }

        #region public method
        public bool Enter(Object obj,StateBase targetState)
        {
			if(currState != null && !currState.Transition(obj,targetState))
				return false;
			ForceEnter(obj,targetState);
            return true;
        }

        public void ForceEnter(Object obj, StateBase targetState)
        {
            if (currState == null)
            {
                currState = targetState;
                currState.OnEnter(obj);
                preparePending = !currState.Prepare(obj);
                return;
            }

            if (currState.ID == targetState.ID)
            {
                currState.Renew(obj, targetState);
                return;
            }

            currState.OnLeave(obj);
            currState = targetState;
            currState.OnEnter(obj);

            // 经OnEnter处理后当前状态是否仍为目标状态
            if (currState == targetState)
                preparePending = !currState.Prepare(obj);
        }

        public void Refresh(Object obj)
        {
            currState.Refresh(obj);
        }

        public void Update(Object obj)
        {
            if (preparePending)
            {
                preparePending = !currState.Prepare(obj);
            }

            if (preparePending)
                return;
            currState.Update(obj);
        }

        public void OnDestory(Object obj)
        {
            if(currState != null)
                currState.OnLeave(obj);
            currState = null;
        }
        #endregion
    }
}
