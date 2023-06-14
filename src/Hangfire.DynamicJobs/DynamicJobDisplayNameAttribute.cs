// This file is part of the Hangfire Core extension set. Copyright © 2023 Hangfire OÜ.
// Please see the LICENSE file for the licensing details.

using System;
using System.Globalization;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Dashboard;

namespace Hangfire
{
    public class DynamicJobDisplayNameAttribute : JobDisplayNameAttribute
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
                    //create display name from JobDisplayNameAttribute
                    if (dynamicJob.DisplayName != null)
                    {
                        var displayName = GenerateDisplayName(dynamicJob);
                        return displayName;
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



        internal static string GenerateDisplayName(DynamicJob job)
        {

            var text = job.DisplayName.DisplayName;

            //code from format method of JobDisplayNameAttribute with private properties (cache/resources)
            //if (ResourceType != null)
            //{
            //    text = _resourceManagerCache.GetOrAdd(ResourceType, InitResourceManager).GetString(DisplayName, CultureInfo.CurrentUICulture);
            //    if (string.IsNullOrEmpty(text))
            //    {
            //        text = DisplayName;
            //    }
            //}
            string[] arguments = SerializationHelper.Deserialize<string[]>(job.Args);
            return string.Format(CultureInfo.CurrentCulture, text, arguments);
        }

    }
}