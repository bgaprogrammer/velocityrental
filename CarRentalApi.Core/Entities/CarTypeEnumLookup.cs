namespace CarRentalApi.Core.Entities;

/// <summary>
/// Lookup entity for <see cref="CarTypeEnum"/> values, used to seed and expose the available car types in the database.
/// This table is not intended to be referenced by other entities via foreign keys; instead, the enum <see cref="CarTypeEnum"/> is used directly in the domain and persisted as an int.
/// The contents of this table are tightly coupled to the <see cref="CarTypeEnum"/> definition: if the enum changes, this table must be updated accordingly.
/// Its main purpose is to provide a normalized, queryable list of car types for UI or API consumers, while keeping the domain model strongly typed.
/// </summary>
public class CarTypeEnumLookup
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
