using Mirror;
using UnityEngine;

namespace Systems
{
    public class TestSystem : IUpdateSystem
    {
        public void Update()
        {
            Debug.Log($"TestSystem: {NetworkServer.activeHost}");
        }
    }
}