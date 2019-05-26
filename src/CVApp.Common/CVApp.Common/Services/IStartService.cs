using CVApp.ViewModels.Start;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVApp.Common.Services
{
    public interface IStartService
    {
        StartOutViewModel GetStartInfoByUserName(string userName);
    }
}
