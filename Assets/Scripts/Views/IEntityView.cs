﻿using System;
using UnityEngine;

namespace Views
{
    public interface IEntityView
    {
        event Action<IEntityView> LocalStarted;
        Vector3 Position { get; }
        bool IsLocal { get; }
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
    }
}