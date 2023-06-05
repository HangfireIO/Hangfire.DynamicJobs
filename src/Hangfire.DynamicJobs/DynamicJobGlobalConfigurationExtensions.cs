// This file is part of the Hangfire Core extension set. Copyright © 2023 Hangfire OÜ.
// Please see the LICENSE file for the licensing details.

using System.Text;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Dashboard;

namespace Hangfire
{
    public static class DynamicJobGlobalConfigurationExtensions
    {
        public static IGlobalConfiguration UseDynamicJobs(
            [NotNull] this IGlobalConfiguration configuration)
        {
            configuration.UseJobDetailsRenderer(-1000, DynamicJobRenderer);
            return configuration.UseFilterProvider(new DynamicJobFilterProvider());
        }

        private static NonEscapedString DynamicJobRenderer(JobDetailsRendererDto dto)
        {
            if (dto.JobDetails.Job?.Type != typeof(DynamicJob)) return null;

            var builder = new StringBuilder();
            var html = new HtmlHelper(dto.Page);
            builder.AppendLine("<h3>Dynamic Job</h3>");

            foreach (var arg in dto.JobDetails.Job.Args)
            {
                if (!(arg is DynamicJob dynamicJob)) continue;

                builder.Append("<div class=\"job-snippet\"><div class=\"job-snippet-code\"><pre><code>");
                var typeName = DynamicJobDisplayNameAttribute.ExtractTypeName(dynamicJob.Type, out var typeNamespace, out var typeAssembly);
                builder.Append("<span class=\"type\"");
                if (typeNamespace != null || typeAssembly != null)
                {
                    builder.Append($" title=\"{html.HtmlEncode(typeNamespace)} namespace, {html.HtmlEncode(typeAssembly)} assembly\"");
                }

                builder.Append($">{html.HtmlEncode(typeName)}</span>");
                builder.Append(".");
                builder.Append(html.HtmlEncode(dynamicJob.Method));
                builder.Append("(");

                if (dynamicJob.ParameterTypes != null)
                {
                    var parameterTypes = SerializationHelper.Deserialize<string[]>(dynamicJob.ParameterTypes, SerializationOption.Internal);
                    for (var i = 0; i < parameterTypes.Length; i++)
                    {
                        var parameterType = DynamicJobDisplayNameAttribute.ExtractTypeName(parameterTypes[i], out var ns, out var assembly);
                        builder.Append("<span class=\"type\"");
                        if (ns != null || assembly != null)
                        {
                            builder.Append($" title=\"{html.HtmlEncode(ns)} namespace, {html.HtmlEncode(assembly)} assembly\"");
                        }

                        builder.Append($">{html.HtmlEncode(parameterType)}</span>");
                        if (i < parameterTypes.Length - 1) builder.Append(", ");
                    }
                }

                builder.Append(")");
                builder.AppendLine("</code></pre></div></div>");

                builder.AppendLine("<h4>Encoded Arguments</h4>");
                builder.AppendLine($"<pre><code>{html.HtmlEncode(dynamicJob.Args)}</code></pre>");

                if (dynamicJob.Filters != null && dynamicJob.Filters.Length > 0)
                {
                    builder.AppendLine("<h4>Filters</h4>");
                    builder.AppendLine("<ul>");

                    foreach (var filter in dynamicJob.Filters)
                    {
                        builder.AppendLine($"<li><span title=\"{html.HtmlEncode(filter.GetType().FullName)}\">{html.HtmlEncode(filter.ToString())}</span></li>");
                    }

                    builder.AppendLine("</ul>");
                }
            }

            return new NonEscapedString(builder.ToString());
        }
    }
}