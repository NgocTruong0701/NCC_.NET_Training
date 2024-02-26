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
using Timesheet.APIs.Tasks.Dto;
using Abp.Domain.Uow;
using DocumentFormat.OpenXml.Office2010.Excel;
using Task = Ncc.Entities.Task;
using Abp.Domain.Repositories;

namespace Timesheet.APIs.Tasks
{
    public class TaskAppService : AppServiceBase
    {
        public TaskAppService(IWorkScope workScope) : base(workScope)
        {
        }

        [HttpGet]
        public async Task<List<TaskDto>> GetAll()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete)) 
            {
                var tasks = await (from t in WorkScope.GetAll<Task>()
                                   select new TaskDto
                                   {
                                       Id = t.Id,
                                       Name = t.Name,
                                       Type = t.Type,
                                       IsDeleted = t.IsDeleted,
                                   }).ToListAsync();
                return tasks;
            }
        }

        [HttpPost]
        public async Task<TaskDto> Save(TaskDto input)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                // Validation Task
                var isExistTask = await WorkScope.GetAll<Task>().Where(s => s.Name == input.Name && s.Id != input.Id).AnyAsync();
                if (isExistTask)
                    throw new UserFriendlyException(string.Format("Task - {0} already existed", input.Name));

                // Add new Task
                if (input.Id <= 0)
                {
                    // Mapper TaskDto -> Task
                    var item = ObjectMapper.Map<Ncc.Entities.Task>(input);

                    // Add to Database with Id
                    input.Id = await WorkScope.InsertAndGetIdAsync(item);
                }
                // Update Task
                else
                {
                    var item = await WorkScope.GetAsync<Ncc.Entities.Task>(input.Id);
                    ObjectMapper.Map(input, item);
                    await WorkScope.UpdateAsync(item);
                }

                return input;
            }
        }

        [HttpDelete]
        //[AbpAuthorize(Ncc.Authorization.PermissionNames.Admin_Tasks_ChangeStatus)]
        public async System.Threading.Tasks.Task Archive(EntityDto<long> input)
        {
            using(UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var isInProject = await WorkScope.GetAll<ProjectTask>()
                .Where(p => p.TaskId == input.Id).AnyAsync();

                if (isInProject)
                    throw new UserFriendlyException(
                        string.Format("This taskId {0} is in a project ,You can't delete task", input.Id));

                var task = await WorkScope.GetAsync<Ncc.Entities.Task>(input.Id);
                if (task != null)
                {
                    task.IsDeleted = true;
                    var t = await WorkScope.UpdateAsync(task);
                }
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task DeArchive([FromBody] TaskDto Task)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var task = await WorkScope.GetAsync<Ncc.Entities.Task>(Task.Id);
                task.IsDeleted = false;
                await WorkScope.UpdateAsync(task);
            }
        }

        [HttpDelete]
        public async System.Threading.Tasks.Task Delete([FromQuery] long id)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var isInProject = await WorkScope.GetAll<ProjectTask>().Where(pt => pt.TaskId == id).AnyAsync();

                if(isInProject)
                {
                    throw new UserFriendlyException(
                        string.Format("This taskId {0} is in a project ,You can't delete task", id));
                }

                var task = await WorkScope.GetAsync<Ncc.Entities.Task>(id);
                if (task != null)
                {
                    await WorkScope.GetRepo<Task>().HardDeleteAsync(task);
                }
                else
                {
                    throw new UserFriendlyException(
                        string.Format("This taskId {0} not found", id));
                }
            }
        }
    }
}
