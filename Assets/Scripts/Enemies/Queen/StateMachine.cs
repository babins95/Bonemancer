using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class IState
{
    public abstract void Enter();
    public abstract void Execute();

    public abstract void Exit();
}
public class StateMachine
{
    IState statoCorrente;


    public StateMachine(IState statoDefault)
    {
        SetState(statoDefault);
    }

    public void StateUpdate()
    {
        if (statoCorrente != null)
        {
            statoCorrente.Execute();
        }
    }

    public void SetState(IState nuovoStato)
    {
        if (statoCorrente != null)
        {
            statoCorrente.Exit();
        }

        statoCorrente = nuovoStato;

        if (statoCorrente != null)
        {
            statoCorrente.Enter();
        }
    }
}
