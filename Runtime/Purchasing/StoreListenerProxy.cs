#nullable enable

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Forwards transaction information to Unity Analytics.
    /// </summary>
    internal class StoreListenerProxy : IInternalStoreListener
    {
        private readonly IStoreListener m_ForwardTo;
        private readonly IExtensionProvider m_Extensions;

        public StoreListenerProxy(IStoreListener forwardTo, IExtensionProvider extensions)
        {
            m_ForwardTo = forwardTo;
            m_Extensions = extensions;
        }

        public void OnInitialized(IStoreController controller)
        {
            m_ForwardTo.OnInitialized(controller, m_Extensions);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string? message)
        {
            m_ForwardTo.OnInitializeFailed(error, message);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            return m_ForwardTo.ProcessPurchase(e);
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            m_ForwardTo.OnPurchaseFailed(i, p);
        }
    }
}
