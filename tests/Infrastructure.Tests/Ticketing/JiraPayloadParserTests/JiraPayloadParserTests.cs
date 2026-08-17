using Infrastructure.Ticketing.Jira;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Tests.Ticketing.JiraPayloadParserTests
{
    public class JiraPayloadParserTests
    {
        [Fact]
        public void Parse_ValidPayload_ReturnsIncidentIntake()
        {
            const string payload = """
        {
            "ticketId": "SCRUM-8",
            "summary": "Robot test failed",
            "status": "To Do",
            "issueType": "Bug",
            "environment": "REGALI",
            "createdAt": "2026-08-17T20:00:00Z"
        }
        """;

            var parser = new JiraPayloadParser();

            var result = parser.Parse(payload);

            Assert.Equal("SCRUM-8", result.TicketId);
            Assert.Equal("Robot test failed", result.Summary);
            Assert.Equal("To Do", result.Status);
            Assert.Equal("Bug", result.IssueType);
            Assert.Equal("REGALI", result.Environment);
            Assert.Equal("Jira", result.SourceProvider);
            Assert.Equal(payload, result.RawPayload);
        }

        [Fact]
        public void Parse_MissingOptionalFields_ReturnsNullForThoseFields()
        {
            const string payload = """
        {
            "ticketId": "SCRUM-9",
            "summary": "Robot test failed"
        }
        """;

            var parser = new JiraPayloadParser();

            var result = parser.Parse(payload);

            Assert.Equal("SCRUM-9", result.TicketId);
            Assert.Equal("Robot test failed", result.Summary);
            Assert.Null(result.Status);
            Assert.Null(result.IssueType);
            Assert.Null(result.Environment);
            Assert.Null(result.CreatedAt);
        }

        [Fact]
        public void Parse_InvalidCreatedAt_ReturnsNull()
        {
            const string payload = """
        {
            "ticketId": "SCRUM-10",
            "summary": "Robot test failed",
            "createdAt": "not-a-date"
        }
        """;

            var parser = new JiraPayloadParser();

            var result = parser.Parse(payload);

            Assert.Null(result.CreatedAt);
        }
    }
}
