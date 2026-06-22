namespace Payment.Application.DTOs;

public record WebhookResponseDto(
    bool Success,
    string Message
);