﻿namespace Project.States
{
    public interface IState
    {
        public void Enter();
        public void Update();
        public void Exit();
    }
}
