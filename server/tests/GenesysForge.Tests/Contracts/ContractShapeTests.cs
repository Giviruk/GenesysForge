using System.Text.Json;
using System.Text.Json.Serialization;
using GenesysForge.Contracts.Auth;
using GenesysForge.Contracts.Characters;
using GenesysForge.Contracts.Pdf;
using GenesysForge.Contracts.Rules;
using GenesysForge.Contracts.Validation;

namespace GenesysForge.Tests.Contracts;

public sealed class ContractShapeTests
{
    [Fact]
    public void SharedContractsCanBeCreated()
    {
        var user = new UserProfileDto(
            Guid.NewGuid(),
            "player@example.com",
            "Player");

        var session = new AuthSessionResponse(
            "token",
            DateTimeOffset.UtcNow.AddHours(1),
            user);

        var draft = new CreateCharacterDraftRequest(
            "Asha Vorn",
            Guid.NewGuid());

        var catalog = new RuleCatalogResponse(
            [new RulesetDto(draft.RulesetId, "Genesys Demo", "1.0", null)],
            [],
            [],
            [],
            []);

        var validation = new ValidationResultResponse(
            true,
            [new ValidationMessageDto("demo.ok", "Valid", ValidationSeverity.Warning, null)]);

        var pdf = new ExportCharacterPdfResponse(
            Guid.NewGuid(),
            "asha-vorn.pdf",
            "application/pdf");

        Assert.Equal("Player", session.User.DisplayName);
        Assert.Equal("Asha Vorn", draft.Name);
        Assert.Single(catalog.Rulesets);
        Assert.True(validation.IsValid);
        Assert.Equal("application/pdf", pdf.ContentType);
    }

    [Fact]
    public void ContractEnumsSerializeAsStringsForTypeScriptMirrors()
    {
        var response = new CharacterSummaryResponse(
            Guid.NewGuid(),
            "Asha Vorn",
            CharacterStatus.Draft,
            Guid.NewGuid(),
            DateTimeOffset.UtcNow);

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());

        var json = JsonSerializer.Serialize(response, options);

        Assert.Contains("\"status\":\"Draft\"", json);
    }
}
