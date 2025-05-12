namespace CarRentalApi.Core.Repositories
{
    /// <summary>
    /// Repository for master data management (init, clean, status).
    /// </summary>
    public interface IMasterDataRepository
    {
        Task InitializeAsync();

        Task CleanAsync();

        Task<bool> IsInitializedAsync();
    }
}
