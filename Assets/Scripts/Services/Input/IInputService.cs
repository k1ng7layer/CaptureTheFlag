using UnityEngine;

namespace Services.Input
{
    public interface IInputService
    {
        void SetInput(Vector3 input);
        Vector3 Input { get; }
    }
}