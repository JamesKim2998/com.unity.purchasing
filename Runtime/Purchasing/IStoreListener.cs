using System;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Implemented by Application developers using Unity Purchasing.
    /// </summary>
    public interface IStoreListener
    {
        /// <summary>
        /// Purchasing failed to initialise for a non recoverable reason.
        /// </summary>
        /// <param name="error"> The failure reason. </param>
        /// <param name="message"> More detail on the error : for example the GoogleBillingResponseCode. </param>
        void OnInitializeFailed(InitializationFailureReason error, string message);

        /// <summary>
        /// A purchase succeeded.
        /// </summary>
        /// <param name="purchaseEvent"> The <c>PurchaseEventArgs</c> for the purchase event. </param>
        /// <returns> The result of the successful purchase </returns>
        PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent);

        /// <summary>
        /// A purchase failed with specified reason.
        /// </summary>
        /// <param name="product"> The product that was attempted to be purchased. </param>
        /// <param name="failureReason"> The failure reason. </param>
        [Obsolete("Use IDetailedStoreListener.OnPurchaseFailed for more detailed callback.", false)]
        void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason);

        /// <summary>
        /// Purchasing initialized successfully.
        ///
        /// The <c>IStoreController</c> and <c>IExtensionProvider</c> are
        /// available for accessing purchasing functionality.
        /// </summary>
        /// <param name="controller"> The <c>IStoreController</c> created during initialization. </param>
        /// <param name="extensions"> The <c>IExtensionProvider</c> created during initialization. </param>
        void OnInitialized(IStoreController controller, IExtensionProvider extensions);
    }
}
