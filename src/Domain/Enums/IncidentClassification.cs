using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum IncidentClassification
    {
        Unknown = 0,
        FalsePositive = 1,
        RealDefect = 2,
        EnvironmentIssue = 3,
        TestScriptProblem = 4
    }
}
