﻿using System.Collections.Generic;
using Systems;
using Zenject;

namespace Core
{
    public class EntryPoint : ITickable, ILateTickable
    {
        private readonly List<IUpdateSystem> _updateSystems = new();
        private readonly List<ILateSystem> _lateSystems = new();

        public EntryPoint(List<ISystem> systems)
        {
            foreach (var system in systems)
            {
                if (system is IUpdateSystem updateSystem)
                    _updateSystems.Add(updateSystem);
                
                if (system is ILateSystem lateSystem)
                    _lateSystems.Add(lateSystem);
            }
        }

        public void Tick()
        {
            foreach (var updateSystem in _updateSystems)
            {
                updateSystem.Update();
            }
        }

        public void LateTick()
        {
            foreach (var lateSystem in _lateSystems)
            {
                lateSystem.OnLate();
            }
        }
    }
}