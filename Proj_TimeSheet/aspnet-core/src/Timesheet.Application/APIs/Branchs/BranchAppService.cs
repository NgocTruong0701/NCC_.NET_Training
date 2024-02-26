using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ncc;
using Ncc.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Timesheet.APIs.Branchs.Dto;
using Timesheet.Entities;
using Timesheet.Extension;
using Timesheet.Paging;
using Ncc.Configuration;
using Timesheet.Uitls;
using System.Collections.Generic;
using Ncc.Authorization.Users;
using Ncc.IoC;

namespace Timesheet.APIs.Branchs
{
    public class BranchAppService : AppServiceBase
    {
        public BranchAppService(IWorkScope workScope) : base(workScope)
        {
        }

        

        [HttpGet]
        public async Task<List<BranchCreateEditDto>> GetAllNotPagging()
        {
            return await WorkScope.GetAll<Branch>()
                 .Select(s => new BranchCreateEditDto
                 {
                     Id = s.Id,
                     Name = s.Name,
                     DisplayName = s.DisplayName,
                     AfternoonWorking = s.AfternoonWorking,
                     AfternoonEndAt = s.AfternoonEndAt,
                     AfternoonStartAt = s.AfternoonStartAt,
                     MorningWorking = s.MorningWorking,
                     MorningStartAt = s.MorningStartAt,
                     MorningEndAt = s.MorningEndAt
                 }).ToListAsync();

        }

       

        [HttpGet]
        public async Task<List<BranchDto>> GetAllBranchFilter(bool isAll = false)
        {
            var branchId = await WorkScope.GetAll<Timesheet.Entities.Branch>().Select(s => s.Id).FirstOrDefaultAsync();
            var query = await WorkScope.GetAll<Branch>()
                 .Select(s => new BranchDto
                 {
                     Id = s.Id,
                     Name = s.Name,
                     DisplayName = s.DisplayName
                 }).ToListAsync();
            if (isAll)
            {
                query.Add(new BranchDto
                {
                    Name = "All",
                    DisplayName = "All",
                    Id = 0,
                });
            }
            return query.OrderBy(s => s.Id).ToList();
        }
    }
}