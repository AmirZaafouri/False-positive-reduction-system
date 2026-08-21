using Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ITicketPayloadParser
    {
        string ProviderName { get; }

        IncidentIntake Parse(string rawPayload);
    }
}
