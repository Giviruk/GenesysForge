namespace GenesysForge.Contracts.Pdf;

public sealed record ExportCharacterPdfResponse(
    Guid CharacterId,
    string FileName,
    string ContentType);
