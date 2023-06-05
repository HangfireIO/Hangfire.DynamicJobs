// This file is part of the Hangfire Core extension set. Copyright © 2023 Hangfire OÜ.
// Please see the LICENSE file for the licensing details.

using System.Collections.Generic;
using Hangfire.Annotations;
using Hangfire.Common;

namespace Hangfire
{
    public class DynamicRecurringJobOptions : RecurringJobOptions
    {
        [CanBeNull]
        public IEnumerable<JobFilterAttribute> Filters { get; set; }
    }
}