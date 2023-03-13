using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IPropertyEvaluator
    {
        Task<Result> evaluate(PropertyProfile propertyProfile);
    }
}