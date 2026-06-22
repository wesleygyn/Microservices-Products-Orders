namespace Customer.Application.DTOs;

public record UpdateCustomerDto(
    string Name,
    string Email,
    bool Active
);