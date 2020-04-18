using System.Collections;
using System.Collections.Generic;



public abstract class State
{
    protected StateMachine m_sMachine;

    public State(StateMachine sMachine)
    {
        this.m_sMachine = sMachine;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Execute(float time);
}

public abstract class NavAgentState : State
{
    protected UnityEngine.AI.NavMeshAgent m_agent;

    public NavAgentState(StateMachine sMachine, UnityEngine.AI.NavMeshAgent agent) : base(sMachine)
    {
        this.m_agent = agent;
    }

}

public class StateMachine
{
    private State curState;


    public void SetState(State i_state)
    {
        //Tell current state it is about to be left
        if (curState != null)
            curState.Exit();

        curState = i_state;
        curState.Enter();
    }

    public void Update(float i_time)
    {
        if (curState != null)
            curState.Execute(i_time);
    }
}
