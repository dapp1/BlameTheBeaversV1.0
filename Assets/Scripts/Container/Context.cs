using UnityEngine;

namespace DIContainer
{
    public class Context : MonoBehaviour
    {
        public Container Container { get; protected set; }

        protected virtual void Awake()
        {
            Container = new Container();
            InstallBindings();
        }

        protected virtual void InstallBindings() { }
    }
}