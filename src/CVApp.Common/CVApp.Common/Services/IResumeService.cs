﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVApp.Common.Services
{
    public interface IResumeService
    {
        Task CreateResume(string userId);
    }
}
