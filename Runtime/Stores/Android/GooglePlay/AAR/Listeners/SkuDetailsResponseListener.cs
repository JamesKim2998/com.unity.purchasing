#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Uniject;
using UnityEngine.Purchasing.Models;
using UnityEngine.Scripting;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// This is C# representation of the Java Class SkuDetailsResponseListener
    /// <a href="https://developer.android.com/reference/com/android/billingclient/api/SkuDetailsResponseListener">See more</a>
    /// </summary>
    class SkuDetailsResponseListener : AndroidJavaProxy
    {
        const string k_AndroidSkuDetailsResponseListenerClassName = "com.android.billingclient.api.SkuDetailsResponseListener";
        readonly Action<IGoogleBillingResult, List<AndroidJavaObject>> m_OnSkuDetailsResponse;
        readonly IUtil m_Util;

        internal SkuDetailsResponseListener(
            Action<IGoogleBillingResult, List<AndroidJavaObject>> onSkuDetailsResponseAction, IUtil util)
            : base(k_AndroidSkuDetailsResponseListenerClassName)
        {
            m_OnSkuDetailsResponse = onSkuDetailsResponseAction;
            m_Util = util;
        }

        [Preserve]
        internal void onSkuDetailsResponse(AndroidJavaObject billingResult, AndroidJavaObject? skuDetails)
        {
            if (skuDetails == null)
            {
                return;
            }

            try
            {
                m_Util.RunOnMainThread(() =>
                {
                    var skuDetailsList = skuDetails.Enumerate<AndroidJavaObject>().ToList();
                    m_OnSkuDetailsResponse(new GoogleBillingResult(billingResult), skuDetailsList);
                });
            }
            catch (Exception ex)
            {
            }
        }
    }
}
