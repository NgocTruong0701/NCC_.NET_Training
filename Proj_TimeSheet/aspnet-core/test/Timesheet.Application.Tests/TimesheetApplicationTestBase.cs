using System;
using System.Collections.Generic;
using System.Text;
using Timesheet.Tests;
using Xunit;

namespace Timesheet.Application.Tests
{
    [Collection("Sequential")]
    public abstract class TimesheetApplicationTestBase : TesBase<TimesheetApplicationTestModule>
    {
    }
}
