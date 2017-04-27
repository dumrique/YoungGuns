using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungGuns.Shared
{
    public interface ICalcService : IService
    {
        void LoadTaxSystem(string taxSystemId);

        Task<CalcResult> Calculate(CalcChangeset changeset);

    }
}
