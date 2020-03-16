using System;
using System.Collections.Generic;

namespace Cards
{
    public interface IOptimalFinder
    {
        IReadOnlyCollection<Card> Find(int totalCost, Guid? target, IReadOnlyCollection<Guid> priorityBonuses, IReadOnlyCollection<Guid> nonPriorityBonuses);
    }

    public class OptimalFinder: IOptimalFinder
    {
        public IReadOnlyCollection<Card> Find(int totalCost, Guid? target, IReadOnlyCollection<Guid> priorityBonuses, IReadOnlyCollection<Guid> nonPriorityBonuses)
        {
            throw new NotImplementedException();
        }
    }
}
