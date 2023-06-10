using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core.Environments.Internal;
using Unity.Services.Core.Internal;

namespace UnityEngine.Purchasing.Registration
{
    class IapCoreInitializeCallback : IInitializablePackage
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register()
        {
            CoreRegistry.Instance.RegisterPackage(new IapCoreInitializeCallback());
        }

        public Task Initialize(CoreRegistry registry)
        {
            CacheInitializedEnvironment(registry);
            return Task.CompletedTask;
        }

        static void CacheInitializedEnvironment(CoreRegistry registry)
        {
            var currentEnvironment = GetCurrentEnvironment(registry);
            CoreServicesEnvironmentSubject.Instance().UpdateCurrentEnvironment(currentEnvironment);
        }

        static string GetCurrentEnvironment(CoreRegistry registry)
        {
            try
            {
                return registry.GetServiceComponent<IEnvironments>().Current;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
