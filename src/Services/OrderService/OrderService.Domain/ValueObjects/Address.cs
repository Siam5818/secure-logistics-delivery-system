namespace OrderService.Domain.ValueObjects;

public sealed record Address
{
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string Country { get; }

    private Address()
    {
        Street = null!;
        City = null!;
        PostalCode = null!;
        Country = null!;
    }

    public Address(string street, string city, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Street is required.", nameof(street));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(postalCode)) throw new ArgumentException("Postal code is required.", nameof(postalCode));
        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country is required.", nameof(country));

        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }
}
