// This file is part of the Hangfire Core extension set. Copyright © 2023 Hangfire OÜ.
// Please see the LICENSE file for the licensing details.

using System;
using System.Diagnostics.CodeAnalysis;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage;
using Newtonsoft.Json;

namespace Hangfire
{
    public sealed class DynamicJob
    {
        public DynamicJob(
            [NotNull] string type,
            [NotNull] string method,
            [CanBeNull] string parameterTypes,
            [CanBeNull] string args,
            [CanBeNull] JobFilterAttribute[] filters,
            [CanBeNull] JobDisplayNameAttribute displayName)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Method = method ?? throw new ArgumentNullException(nameof(method));
            ParameterTypes = parameterTypes;
            Args = args;
            Filters = filters;
            DisplayName = displayName;
        }

        [NotNull]
        [JsonProperty("t")]
        public string Type { get; }

        [NotNull]
        [JsonProperty("m")]
        public string Method { get; }

        [CanBeNull]
        [JsonProperty("p", NullValueHandling = NullValueHandling.Ignore)]
        public string ParameterTypes { get; }

        [CanBeNull]
        [JsonProperty("a", NullValueHandling = NullValueHandling.Ignore)]
        public string Args { get; }

        [CanBeNull]
        [JsonProperty("f",  NullValueHandling = NullValueHandling.Ignore, ItemTypeNameHandling = TypeNameHandling.Auto)]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
        [SuppressMessage("Security", "CA2326:Do not use TypeNameHandling values other than None")]
        public JobFilterAttribute[] Filters { get; }

        //TODO: Check if is better save display name as resolved string (maybe when you use location resources can be cause crash?)
        [CanBeNull]
        [JsonProperty("dn", NullValueHandling = NullValueHandling.Ignore, ItemTypeNameHandling = TypeNameHandling.Auto)]
        public JobDisplayNameAttribute DisplayName { get; }

        [PublicAPI]
        [DynamicJobDisplayName]
        public static object Execute(DynamicJob dynamicJob, PerformContext context)
        {
            if (dynamicJob == null) throw new ArgumentNullException(nameof(dynamicJob));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var invocationData = new InvocationData(
                dynamicJob.Type,
                dynamicJob.Method,
                dynamicJob.ParameterTypes ?? String.Empty,
                dynamicJob.Args);

            var job = invocationData.DeserializeJob();

            return context.Performer.Perform(new PerformContext(
                context.Storage,
                context.Connection,
                new BackgroundJob(context.BackgroundJob.Id, job, context.BackgroundJob.CreatedAt),
                context.CancellationToken));
        }
    }
}