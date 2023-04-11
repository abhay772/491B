using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces
{
    public interface IPropertyEvaluator
    {
        Task<Result> evaluate(PropertyProfile propertyProfile);
    }
}