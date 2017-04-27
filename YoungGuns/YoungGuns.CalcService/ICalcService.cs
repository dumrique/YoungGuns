using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoungGuns.Data;
using YoungGuns.Shared;

namespace YoungGuns.CalcService
{
    public interface ICalcService
    {
        TaxSystem GetLoadedTaxSystem();
        void Calculate(CalcChangeset changeset);
    }
}
