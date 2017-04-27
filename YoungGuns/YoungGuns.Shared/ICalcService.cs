using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoungGuns.Data;

namespace YoungGuns.Shared
{
    public interface ICalcService : IService
    {
        Task LoadTaxSystem(string taxSystemId);

        void Calculate(CalcChangeset changeset);

    }
}
