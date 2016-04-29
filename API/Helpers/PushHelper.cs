using System;
using System.Linq;
using System.Collections.Generic;

using PushSharp.Apple;

using Newtonsoft.Json.Linq;

using iGO.Domain.Entities;
using PushSharp.Google;

namespace iGO.API.Helpers
{
	public static class PushHelper
	{
		public static void SendNotification(IEnumerable<DeviceToken> deviceTokens, string text, string type)
		{
			if (deviceTokens.Any(x => x.Platform.ToLower() == "android"))
			{
				/*sendGCM(deviceTokens
					.Where (x => x.Platform.ToLower() == "android")
					.Select (y => y.Token).ToArray(),
					text
				);*/
			}

			if (deviceTokens.Any(x => x.Platform.ToLower() == "ios"))
			{
				sendAPNS(deviceTokens
						.Where (x => x.Platform.ToLower() == "ios")
						.Select (y => y.Token).ToArray(),
					text,
					type
				);
			}
		}

		private static void sendGCM(string[] deviceTokens, string text, string type)
		{
			// Configuration
			var config = new GcmConfiguration ("GCM-SENDER-ID", "AUTH-TOKEN", null);

			// Create a new broker
			var gcmBroker = new GcmServiceBroker (config);

			// Wire up events
			gcmBroker.OnNotificationFailed += (notification, aggregateEx) => {

				/*aggregateEx.Handle (ex => {

					// See what kind of exception it was to further diagnose
					if (ex is GcmNotificationException) {
						var notificationException = (GcmNotificationException)ex;

						// Deal with the failed notification
						var gcmNotification = notificationException.Notification;
						var description = notificationException.Description;

						Console.WriteLine ($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
					} else if (ex is GcmMulticastResultException) {
						var multicastException = (GcmMulticastResultException)ex;

						foreach (var succeededNotification in multicastException.Succeeded) {
							Console.WriteLine ($"GCM Notification Failed: ID={succeededNotification.MessageId}");
						}

						foreach (var failedKvp in multicastException.Failed) {
							var n = failedKvp.Key;
							var e = failedKvp.Value;

							Console.WriteLine ($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Description}");
						}

					} else if (ex is DeviceSubscriptionExpiredException) {
						var expiredException = (DeviceSubscriptionExpiredException)ex;

						var oldId = expiredException.OldSubscriptionId;
						var newId = expiredException.NewSubscriptionId;

						Console.WriteLine ($"Device RegistrationId Expired: {oldId}");

						if (!string.IsNullOrWhitespace (newId)) {
							// If this value isn't null, our subscription changed and we should update our database
							Console.WriteLine ($"Device RegistrationId Changed To: {newId}");
						}
					} else if (ex is RetryAfterException) {
						var retryException = (RetryAfterException)ex;
						// If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
						Console.WriteLine ($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
					} else {
						Console.WriteLine ("GCM Notification Failed for some unknown reason");
					}

					// Mark it as handled
					return true;
				});*/
			};

			gcmBroker.OnNotificationSucceeded += (notification) => {
				//Console.WriteLine ("GCM Notification Sent!");
			};

			// Start the broker
			gcmBroker.Start ();

			foreach (var regId in deviceTokens) {
				// Queue a notification to send
				gcmBroker.QueueNotification (new GcmNotification {
					RegistrationIds = new List<string> { 
						regId
					},
					Data = JObject.Parse ("{ \"message\" : \"" + text + "\" }")
				});
			}

			// Stop the broker, wait for it to finish   
			// This isn't done after every message, but after you're
			// done with the broker
			gcmBroker.Stop ();
		}

		private static void sendAPNS(string[] deviceTokens, string text, string type)
		{
			// Configuration (NOTE: .pfx can also be used here)
			var config = new ApnsConfiguration (ApnsConfiguration.ApnsServerEnvironment.Sandbox, 
				"Certificates_Dev_iGoDev.p12", "iGo@2016");

			// Create a new broker
			var apnsBroker = new ApnsServiceBroker (config);

			// Wire up events
			apnsBroker.OnNotificationFailed += (notification, aggregateEx) => {

				aggregateEx.Handle (ex => {

					// See what kind of exception it was to further diagnose
					if (ex is ApnsNotificationException) {
						var notificationException = (ApnsNotificationException)ex;

						// Deal with the failed notification
						var apnsNotification = notificationException.Notification;
						var statusCode = notificationException.ErrorStatusCode;

						Console.WriteLine ($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

					} else {
						// Inner exception might hold more useful information like an ApnsConnectionException           
						Console.WriteLine ($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
					}

					// Mark it as handled
					return true;
				});
			};

			apnsBroker.OnNotificationSucceeded += (notification) => {
				Console.WriteLine ("Apple Notification Sent!");
			};

			// Start the broker
			apnsBroker.Start ();

			foreach (var deviceToken in deviceTokens) {
				// Queue a notification to send
				apnsBroker.QueueNotification (new ApnsNotification {
					DeviceToken = deviceToken,
					Payload = JObject.Parse ("{\"aps\":{\"alert\":\"" + text + "\", \"badge\" : \"1\" }, \"type\": \"" + type + "\"}")
				});
			}

			// Stop the broker, wait for it to finish   
			// This isn't done after every message, but after you're
			// done with the broker
			apnsBroker.Stop ();
		}
	}
}
