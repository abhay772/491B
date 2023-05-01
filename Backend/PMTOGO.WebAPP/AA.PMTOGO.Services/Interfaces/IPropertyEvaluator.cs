using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces
{
    public interface IPropertyEvaluator
    {
        Task<Result> Evaluate(PropertyProfile propertyProfile);
        Task<Result> LoadProfileAsync(string username);
        Task<Result> SaveProfileAsync(string username, PropertyProfile propertyProfile);

    }
}