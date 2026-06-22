namespace Customer.Application.DTOs;

public record CreateCustomerDto(
    string Name,
    string Email,
    string Cpf
);