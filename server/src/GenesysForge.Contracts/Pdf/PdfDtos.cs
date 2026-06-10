namespace GenesysForge.Contracts.Pdf;

public sealed record ExportCharacterPdfRequest(
    Guid CharacterId);

public sealed record ExportCharacterPdfResponse(
    Guid CharacterId,
    string FileName,
    string ContentType);
