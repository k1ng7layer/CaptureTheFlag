using UnityEngine;

namespace Services.Input
{
    public interface IInputService
    {
        Vector3 Input { get; }
        void SetInput(Vector3 input);
    }
}