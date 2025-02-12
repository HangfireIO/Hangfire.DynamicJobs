// This file is part of the Hangfire Core extension set. Copyright © 2023 Hangfire OÜ.
// Please see the LICENSE file for the licensing details.

using System;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Dashboard;

namespace Hangfire
{
    public sealed class DynamicJobDisplayNameAttribute : JobDisplayNameAttribute
    {
        public DynamicJobDisplayNameAttribute() : base("empty")
        {
        }

        public override string Format([NotNull] DashboardContext context, [NotNull] Job job)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (job == null) throw new ArgumentNullException(nameof(job));

            foreach (var arg in job.Args)
            {
                if (arg is DynamicJob dynamicJob)
                {
                    if (dynamicJob.DisplayName != null)
                    {
                        return dynamicJob.DisplayName;
                    }

                    return $"Dynamic: {ExtractTypeName(dynamicJob.Type, out _, out _)}.{dynamicJob.Method}";
                }
            }

            return $"{job.Type.Name}.{job.Method}";
        }

        internal static string ExtractTypeName(string type, out string @namespace, out string assembly)
        {
            @namespace = null;
            assembly = null;

            var commaIndex = type.IndexOf(",", StringComparison.OrdinalIgnoreCase);
            if (commaIndex >= 0)
            {
                assembly = type.Substring(commaIndex + 1);
                type = type.Substring(0, commaIndex);
            }

            var lastDotIndex = type.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
            if (lastDotIndex >= 0)
            {
                @namespace = type.Substring(0, lastDotIndex);
                type = type.Substring(lastDotIndex + 1);
            }

            if (assembly != null)
            {
                commaIndex = assembly.IndexOf(",", StringComparison.OrdinalIgnoreCase);
                if (commaIndex >= 0)
                {
                    assembly = assembly.Substring(0, commaIndex);
                }
            }

            return type;
        }
    }
}