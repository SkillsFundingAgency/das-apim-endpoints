using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;

namespace SFA.DAS.ApprenticeApp.Api.Telemetry
{
 

    public class MyApprenticeshipAppMetrics
    {
        private static readonly Meter Meter = new Meter("MyApprenticeshipAppMetrics");
        private static readonly Counter<int> LoginCounter = Meter.CreateCounter<int>("LoginCounter");
        private static readonly Counter<int> ViewProfileCounter = Meter.CreateCounter<int>("ViewProfileCounter");
        private static readonly Counter<int> EnablePushNotificationsCounter = Meter.CreateCounter<int>("EnablePushNotificationsCounter");
        private static readonly Counter<int> DisablePushNotificationsCounter = Meter.CreateCounter<int>("DisablePushNotificationsCounter");


        public static void IncrementViewProfileCounter()
        {
            ViewProfileCounter.Add(1);
        }

        public static void IncrementEnablePushNotificationsCounter()
        {
            EnablePushNotificationsCounter.Add(1);
        }

        public static void IncrementDisablePushNotificationsCounter()
        {
            DisablePushNotificationsCounter.Add(1);
        }
    }


}