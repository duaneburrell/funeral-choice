/*!
 * ASP.NET SignalR JavaScript Library v2.1.2
 * http://signalr.net/
 *
 * Copyright Microsoft Open Technologies, Inc. All rights reserved.
 * Licensed under the Apache 2.0
 * https://github.com/SignalR/SignalR/blob/master/LICENSE.md
 *
 */

/// <reference path="..\..\SignalR.Client.JS\Scripts\jquery-1.6.4.js" />
/// <reference path="jquery.signalR.js" />
(function ($, window, undefined) {
    /// <param name="$" type="jQuery" />
    "use strict";

    if (typeof ($.signalR) !== "function") {
        throw new Error("SignalR: SignalR is not loaded. Please ensure jquery.signalR-x.js is referenced before ~/signalr/js.");
    }

    var signalR = $.signalR;

    function makeProxyCallback(hub, callback) {
        return function () {
            // Call the client hub method
            callback.apply(hub, $.makeArray(arguments));
        };
    }

    function registerHubProxies(instance, shouldSubscribe) {
        var key, hub, memberKey, memberValue, subscriptionMethod;

        for (key in instance) {
            if (instance.hasOwnProperty(key)) {
                hub = instance[key];

                if (!(hub.hubName)) {
                    // Not a client hub
                    continue;
                }

                if (shouldSubscribe) {
                    // We want to subscribe to the hub events
                    subscriptionMethod = hub.on;
                } else {
                    // We want to unsubscribe from the hub events
                    subscriptionMethod = hub.off;
                }

                // Loop through all members on the hub and find client hub functions to subscribe/unsubscribe
                for (memberKey in hub.client) {
                    if (hub.client.hasOwnProperty(memberKey)) {
                        memberValue = hub.client[memberKey];

                        if (!$.isFunction(memberValue)) {
                            // Not a client hub function
                            continue;
                        }

                        subscriptionMethod.call(hub, memberKey, makeProxyCallback(hub, memberValue));
                    }
                }
            }
        }
    }

    $.hubConnection.prototype.createHubProxies = function () {
        var proxies = {};
        this.starting(function () {
            // Register the hub proxies as subscribed
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, true);

            this._registerSubscribedHubs();
        }).disconnected(function () {
            // Unsubscribe all hub proxies when we "disconnect".  This is to ensure that we do not re-add functional call backs.
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, false);
        });

        proxies['administratorHub'] = this.createHubProxy('administratorHub'); 
        proxies['administratorHub'].client = { };
        proxies['administratorHub'].server = {
            providerPortfolioCreated: function (providerPortfolio) {
            /// <summary>Calls the ProviderPortfolioCreated method on the server-side AdministratorHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"providerPortfolio\" type=\"Object\">Server side type is Obsequy.Model.ProviderPortfolio</param>
                return proxies['administratorHub'].invoke.apply(proxies['administratorHub'], $.merge(["ProviderPortfolioCreated"], $.makeArray(arguments)));
             }
        };

        proxies['baseHub'] = this.createHubProxy('baseHub'); 
        proxies['baseHub'].client = { };
        proxies['baseHub'].server = {
        };

        proxies['consumerHub'] = this.createHubProxy('consumerHub'); 
        proxies['consumerHub'].client = { };
        proxies['consumerHub'].server = {
            responseUpdated: function (response, consumerPortfolio, providerPortfolio) {
            /// <summary>Calls the ResponseUpdated method on the server-side ConsumerHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"response\" type=\"Object\">Server side type is Obsequy.Model.Response</param>
            /// <param name=\"consumerPortfolio\" type=\"Object\">Server side type is Obsequy.Model.ConsumerPortfolio</param>
            /// <param name=\"providerPortfolio\" type=\"Object\">Server side type is Obsequy.Model.ProviderPortfolio</param>
                return proxies['consumerHub'].invoke.apply(proxies['consumerHub'], $.merge(["ResponseUpdated"], $.makeArray(arguments)));
             },

            updateIdentity: function (member) {
            /// <summary>Calls the UpdateIdentity method on the server-side ConsumerHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"member\" type=\"Object\">Server side type is Obsequy.Model.ConsumerMember</param>
                return proxies['consumerHub'].invoke.apply(proxies['consumerHub'], $.merge(["UpdateIdentity"], $.makeArray(arguments)));
             }
        };

        proxies['providerHub'] = this.createHubProxy('providerHub'); 
        proxies['providerHub'].client = { };
        proxies['providerHub'].server = {
            responseUpdated: function (response, consumerPortfolio, providerPortfolio) {
            /// <summary>Calls the ResponseUpdated method on the server-side ProviderHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"response\" type=\"Object\">Server side type is Obsequy.Model.Response</param>
            /// <param name=\"consumerPortfolio\" type=\"Object\">Server side type is Obsequy.Model.ConsumerPortfolio</param>
            /// <param name=\"providerPortfolio\" type=\"Object\">Server side type is Obsequy.Model.ProviderPortfolio</param>
                return proxies['providerHub'].invoke.apply(proxies['providerHub'], $.merge(["ResponseUpdated"], $.makeArray(arguments)));
             },

            updateIdentity: function (member) {
            /// <summary>Calls the UpdateIdentity method on the server-side ProviderHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"member\" type=\"Object\">Server side type is Obsequy.Model.ProviderMember</param>
                return proxies['providerHub'].invoke.apply(proxies['providerHub'], $.merge(["UpdateIdentity"], $.makeArray(arguments)));
             }
        };

        return proxies;
    };

    signalR.hub = $.hubConnection("/signalr", { useDefaultPath: false });
    $.extend(signalR, signalR.hub.createHubProxies());

}(window.jQuery, window));