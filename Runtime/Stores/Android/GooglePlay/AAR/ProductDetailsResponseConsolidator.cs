using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Interfaces;
using UnityEngine.Purchasing.Models;

namespace UnityEngine.Purchasing
{
    class ProductDetailsResponseConsolidator : IProductDetailsResponseConsolidator
    {
        const int k_RequiredNumberOfCallbacks = 2;
        int m_NumberReceivedCallbacks;
        readonly Action<IProductDetailsQueryResponse> m_OnProductDetailsResponseConsolidated;
        readonly IProductDetailsQueryResponse m_Responses = new ProductDetailsQueryResponse();

        internal ProductDetailsResponseConsolidator(Action<IProductDetailsQueryResponse> onProductDetailsResponseConsolidated)
        {
            m_OnProductDetailsResponseConsolidated = onProductDetailsResponseConsolidated;
        }

        public void Consolidate(IGoogleBillingResult billingResult, IEnumerable<AndroidJavaObject> productDetails)
        {
            try
            {
                m_NumberReceivedCallbacks++;

                m_Responses.AddResponse(billingResult, productDetails);

                if (m_NumberReceivedCallbacks >= k_RequiredNumberOfCallbacks)
                {
                    m_OnProductDetailsResponseConsolidated(m_Responses);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
