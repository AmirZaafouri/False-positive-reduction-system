using NetArchTest.Rules;

namespace Architecture.Tests
{
    /// <summary>
    /// Enforces Clean Architecture dependency rules for the IncidentValidationEngine.
    ///
    /// Dependency direction:
    ///   Domain  ←  Application  ←  Infrastructure  ←  Api
    ///
    /// Each outer layer may depend on inner layers, but inner layers must never
    /// depend on outer layers.
    /// </summary>
    public class ArchitectureTests
    {
        // Assembly name constants — derived from the project root namespaces
        private const string DomainAssembly = "Domain";
        private const string ApplicationAssembly = "Application";
        private const string InfrastructureAssembly = "Infrastructure";
        private const string ApiAssembly = "Api";

        // ─── Domain ──────────────────────────────────────────────────────────

        [Fact]
        public void Domain_Should_Not_HaveDependencyOn_Application()
        {
            var result = Types.InAssembly(typeof(Domain.Entities.Incident).Assembly)
                .ShouldNot()
                .HaveDependencyOn(ApplicationAssembly)
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Domain must not reference Application. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        [Fact]
        public void Domain_Should_Not_HaveDependencyOn_Infrastructure()
        {
            var result = Types.InAssembly(typeof(Domain.Entities.Incident).Assembly)
                .ShouldNot()
                .HaveDependencyOn(InfrastructureAssembly)
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Domain must not reference Infrastructure. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        [Fact]
        public void Domain_Should_Not_HaveDependencyOn_Api()
        {
            var result = Types.InAssembly(typeof(Domain.Entities.Incident).Assembly)
                .ShouldNot()
                .HaveDependencyOn(ApiAssembly)
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Domain must not reference Api. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        // ─── Application ─────────────────────────────────────────────────────

        [Fact]
        public void Application_Should_Not_HaveDependencyOn_Infrastructure()
        {
            var result = Types.InAssembly(typeof(Application.Interfaces.IIncidentIntakeService).Assembly)
                .ShouldNot()
                .HaveDependencyOn(InfrastructureAssembly)
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Application must not reference Infrastructure. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOn_Api()
        {
            var result = Types.InAssembly(typeof(Application.Interfaces.IIncidentIntakeService).Assembly)
                .ShouldNot()
                .HaveDependencyOn(ApiAssembly)
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Application must not reference Api. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        [Fact]
        public void Application_Should_Not_HaveDirectDependencyOn_Jira()
        {
            // Application must not know about Jira specifically — that detail belongs in Infrastructure.
            var result = Types.InAssembly(typeof(Application.Interfaces.IIncidentIntakeService).Assembly)
                .ShouldNot()
                .HaveDependencyOn("Infrastructure.Ticketing.Jira")
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Application must not reference Jira infrastructure directly. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        // ─── Infrastructure ───────────────────────────────────────────────────

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOn_Api()
        {
            var result = Types.InAssembly(typeof(Infrastructure.Ticketing.Jira.JiraPayloadParser).Assembly)
                .ShouldNot()
                .HaveDependencyOn(ApiAssembly)
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Infrastructure must not reference Api. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDirectDependencyOn_Domain()
        {
            // Infrastructure references Application, which references Domain.
            // Infrastructure must NOT bypass Application and reference Domain directly.
            var result = Types.InAssembly(typeof(Infrastructure.Ticketing.Jira.JiraPayloadParser).Assembly)
                .ShouldNot()
                .HaveDependencyOn(DomainAssembly)
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Infrastructure must not reference Domain directly (use Application interfaces). Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        // ─── Api ──────────────────────────────────────────────────────────────

        [Fact]
        public void Api_Controllers_Should_Not_Contain_BusinessLogic()
        {
            // Controllers must depend only on Application interfaces, never on
            // Infrastructure concretions or Domain entities directly.
            var result = Types.InAssembly(typeof(Api.Endpoints.JiraWebhookController).Assembly)
                .That()
                .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
                .ShouldNot()
                .HaveDependencyOn(InfrastructureAssembly)
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Controllers must not depend on Infrastructure directly. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        [Fact]
        public void Api_Controllers_Should_ResideIn_EndpointsNamespace()
        {
            var result = Types.InAssembly(typeof(Api.Endpoints.JiraWebhookController).Assembly)
                .That()
                .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
                .Should()
                .ResideInNamespace("Api.Endpoints")
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"All controllers must live in Api.Endpoints. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        // ─── Naming conventions ───────────────────────────────────────────────

        [Fact]
        public void Interfaces_In_Application_Should_StartWith_I()
        {
            var result = Types.InAssembly(typeof(Application.Interfaces.IIncidentIntakeService).Assembly)
                .That()
                .ResideInNamespace("Application.Interfaces")
                .Should()
                .HaveNameStartingWith("I")
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"All types in Application.Interfaces must start with 'I'. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }

        [Fact]
        public void Domain_Entities_Should_ResideIn_EntitiesNamespace()
        {
            var result = Types.InAssembly(typeof(Domain.Entities.Incident).Assembly)
                .That()
                .ResideInNamespace("Domain.Entities")
                .Should()
                .ResideInNamespace("Domain.Entities")
                .GetResult();

            Assert.True(result.IsSuccessful,
                $"Domain entities must reside in Domain.Entities. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
        }
    }
}
