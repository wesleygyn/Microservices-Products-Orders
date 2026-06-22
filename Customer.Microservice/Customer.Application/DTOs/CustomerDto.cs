namespace Customer.Application.DTOs;

public record CustomerDto(
    Guid Id,
    string Name,
    string Email,
    string Cpf,
    bool Active,
    DateTime CreatedAt,
    DateTime UpdatedAt
);