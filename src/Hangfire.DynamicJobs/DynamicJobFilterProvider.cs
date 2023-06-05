// This file is part of the Hangfire Core extension set. Copyright © 2023 Hangfire OÜ.
// Please see the LICENSE file for the licensing details.

using System;
using System.Collections.Generic;
using Hangfire.Annotations;
using Hangfire.Common;

namespace Hangfire
{
    public class DynamicJobFilterProvider : IJobFilterProvider
    {
        public IEnumerable<JobFilter> GetFilters([NotNull] Job job)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));

            foreach (var arg in job.Args)
            {
                if (arg is DynamicJob dynamicJob && dynamicJob.Filters != null)
                {
                    foreach (var filter in dynamicJob.Filters)
                    {
                        yield return new JobFilter(filter, JobFilterScope.Method, null);
                    }
                }
            }
        }
    }
}