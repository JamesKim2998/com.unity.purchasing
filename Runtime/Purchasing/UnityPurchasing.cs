using System;
using System.Collections.Generic;
#if IAP_ANALYTICS_SERVICE_ENABLED || IAP_ANALYTICS_SERVICE_ENABLED_WITH_SERVICE_COMPONENT
using Unity.Services.Analytics;
using Unity.Services.Core;
#endif
using UnityEngine.Purchasing.Extension;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// The core abstract implementation for Unity Purchasing.
    /// </summary>
    public abstract class UnityPurchasing
    {
        /// <summary>
        /// The main initialization call for Unity Purchasing.
        /// </summary>
        /// <param name="listener"> The <c>IStoreListener</c> to receive callbacks for future transactions </param>
        /// <param name="builder"> The <c>ConfigurationBuilder</c> containing the product definitions mapped to stores </param>
        [Obsolete("Use Initialize(IDetailedStoreListener, ConfigurationBuilder)", false)]
        public static void Initialize(IStoreListener listener, ConfigurationBuilder builder)
        {
            var logger = Debug.unityLogger;

            Initialize(listener, builder, logger, Application.persistentDataPath,
                builder.factory.GetCatalogProvider());
        }

        /// <summary>
        /// The main initialization call for Unity Purchasing.
        /// </summary>
        /// <param name="listener"> The <c>IDetailedStoreListener</c> to receive callbacks for future transactions </param>
        /// <param name="builder"> The <c>ConfigurationBuilder</c> containing the product definitions mapped to stores </param>
        public static void Initialize(IDetailedStoreListener listener, ConfigurationBuilder builder)
        {
            var logger = Debug.unityLogger;

            Initialize(listener, builder, logger, Application.persistentDataPath,
                builder.factory.GetCatalogProvider());
        }

        /// <summary>
        /// This is useful in certain test scenarios, such as repeatedly testing
        /// an App's behaviour when purchases are restored.
        ///
        /// This is a static method since developers may wish to clear the log before
        /// initialising IAP.
        /// </summary>
        public static void ClearTransactionLog()
        {
            var log = new TransactionLog(Debug.unityLogger, Application.persistentDataPath);
            log.Clear();
        }

        /// <summary>
        /// Created for integration testing.
        /// </summary>
        internal static void Initialize(IStoreListener listener, ConfigurationBuilder builder,
            ILogger logger, string persistentDatapath,
            ICatalogProvider catalog)
        {
            var transactionLog = new TransactionLog(logger, persistentDatapath);
            var manager = new PurchasingManager(transactionLog, logger, builder.factory.service,
                builder.factory.storeName);

            // Proxy the PurchasingManager's callback interface to forward Transactions to Analytics.
            var proxy = new StoreListenerProxy(listener, builder.factory);
            FetchAndMergeProducts(builder.useCatalogProvider, builder.products, catalog, response =>
            {
                manager.Initialize(proxy, response);
            });
        }

        internal static void FetchAndMergeProducts(bool useCatalog,
            HashSet<ProductDefinition> localProductSet, ICatalogProvider catalog, Action<HashSet<ProductDefinition>> callback)
        {
            if (useCatalog && catalog != null)
            {
                catalog.FetchProducts(cloudProducts =>
                {
                    var updatedProductSet = new HashSet<ProductDefinition>(localProductSet);

                    foreach (var product in cloudProducts)
                    {
                        // Products are hashed by id, so this should remove the local product with the same id before adding the cloud product
                        updatedProductSet.Remove(product);
                        updatedProductSet.Add(product);
                    }

                    callback(updatedProductSet);
                });
            }
            else
            {
                callback(localProductSet);
            }
        }
    }
}
