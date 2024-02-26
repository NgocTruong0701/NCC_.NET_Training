using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Extensions;
using Abp.UI;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ncc;
using Ncc.Authorization.Users;
using Ncc.Entities;
using Ncc.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Timesheet.APIs.Projects.Dto;
using Timesheet.Entities;
using Timesheet.Extension;
using static Ncc.Entities.Enum.StatusEnum;

namespace Timesheet.APIs.Projects
{
    public class ProjectAppService : AppServiceBase
    {
        public ProjectAppService(IWorkScope workScope) : base(workScope)
        {
        }
        /// <summary>
        /// Get the quantity of projects grouped by their status.
        /// </summary>
        /// <returns>A list of quantity data for projects grouped by status.</returns>
        [HttpGet]
        public async Task<List<QuantityProjectDto>> GetQuantityProject()
        {
            var listQuantityProject = await (from p in WorkScope.GetAll<Project>()
                                             group p by p.Status into gr
                                             select new QuantityProjectDto
                                             {
                                                 Status = gr.Key,
                                                 Quantity = gr.Count()
                                             }).OrderBy(gr => gr.Status).ToListAsync();
            return listQuantityProject;
        }

        /// <summary>
        /// Get a list of projects based on specified status and search criteria.
        /// </summary>
        /// <param name="status">The status of projects to filter by.</param>
        /// <param name="search">The search keyword to filter projects by name or customer name.</param>
        /// <returns>A list of project data transfer objects.</returns>
        [HttpGet]
        public async Task<List<ProjectDto>> getAll([FromQuery] ProjectStatus? status, [FromQuery] string search)
        {

            var listProjects = await (from pro in WorkScope.GetAll<Project>()
                                      where pro.Status == status || status == null
                                      join c in WorkScope.GetAll<Customer>() on pro.CustomerId equals c.Id
                                      where (search == null) || c.Name.ToLower().Contains(search.ToLower()) || pro.Name.ToLower().Contains(search.ToLower())

                                      select new ProjectDto
                                      {
                                          Id = pro.Id,
                                          Name = pro.Name,
                                          CustomerName = c.Name.ToLower(),
                                          Code = pro.Code,
                                          Status = pro.Status,
                                          ProjectType = pro.ProjectType,
                                          TimeStart = pro.TimeStart,
                                          TimeEnd = pro.TimeEnd,
                                          ActiveMember = WorkScope.GetAll<ProjectUser>()
                                                        .Where(u => u.Type != ProjectUserType.DeActive && u.ProjectId == pro.Id)
                                                        .Count(),
                                          Pms = WorkScope.GetAll<ProjectUser>()
                                                        .Where(pu => pu.ProjectId == pro.Id && pu.Type == ProjectUserType.PM)
                                                        .Select(pu => pu.User.FullName)
                                                        .OrderBy(name => name).ToList(),
                                      }).ToListAsync();
            if (status != null)
                listProjects = listProjects.OrderBy(p => p.Id).ToList();
            return listProjects;
        }

        [HttpPost]
        public async Task<ProjectCreateEditDto> Save(ProjectCreateEditDto input)
        {
            // check data validation
            var isNameExist = await WorkScope.GetAll<Project>().AnyAsync(p => p.Name == input.Name && p.Id != input.Id);
            var isCodeExist = await WorkScope.GetAll<Project>().AnyAsync(p => p.Code == input.Code && p.Id != input.Id);

            if (isNameExist)
            {
                throw new UserFriendlyException(string.Format("Project name: {0} is exists", input.Name));
            }
            if (isCodeExist)
            {
                throw new UserFriendlyException(string.Format("Project code: {0} is exists", input.Code));
            }

            if (input.Id <= 0)
            {
                // Add new Project
                var projectAdd = ObjectMapper.Map<Project>(input);
                projectAdd.Status = ProjectStatus.Active;
                input.Id = await WorkScope.InsertAndGetIdAsync(projectAdd);

                // Add new Project Task
                foreach (var item in input.Tasks)
                {
                    var itemAdd = ObjectMapper.Map<ProjectTask>(item);

                    itemAdd.ProjectId = input.Id;

                    await WorkScope.InsertAsync(itemAdd);
                }

                // Add new Project User
                foreach (var item in input.Users)
                {
                    var itemAdd = ObjectMapper.Map<ProjectUser>(item);

                    itemAdd.ProjectId = input.Id;

                    await WorkScope.InsertAsync(itemAdd);
                }

                // Add new Project Target User
                foreach (var item in input.ProjectTargetUsers)
                {
                    var itemAdd = ObjectMapper.Map<ProjectTargetUser>(item);

                    itemAdd.ProjectId = input.Id;

                    await WorkScope.InsertAsync(itemAdd);
                }
            }
            else
            {
                var itemEdit = await WorkScope.GetAsync<Project>(input.Id);
                if (itemEdit == null)
                    throw new UserFriendlyException(string.Format("Project Id: {0} is not found", input.Id));

                if (itemEdit.Name != input.Name || itemEdit.TimeStart != input.TimeStart || itemEdit.TimeEnd != input.TimeEnd || itemEdit.Status != input.Status || itemEdit.Code != input.Code || itemEdit.ProjectType != input.ProjectType || itemEdit.Note != input.Note || itemEdit.CustomerId != input.CustomerId || itemEdit.IsNotifyToKomu != input.IsNotifyToKomu || itemEdit.IsNoticeKMSubmitTS != input.IsNoticeKMSubmitTS || itemEdit.IsNoticeKMRequestOffDate != input.IsNoticeKMRequestOffDate || itemEdit.IsNoticeKMApproveRequestOffDate != input.IsNoticeKMApproveRequestOffDate || itemEdit.IsNoticeKMRequestChangeWorkingTime != input.IsNoticeKMRequestChangeWorkingTime || itemEdit.IsNoticeKMApproveChangeWorkingTime != input.IsNoticeKMApproveChangeWorkingTime || itemEdit.isAllUserBelongTo != input.isAllUserBelongTo)
                {
                    ObjectMapper.Map(input, itemEdit);
                    await WorkScope.UpdateAsync(itemEdit);
                }

                // Get ListProjectUserOld, ProjectTaskOld, ProjectTargetUserOld
                var listProjectUserOld = await WorkScope.GetAll<ProjectUser>().Where(pu => pu.ProjectId == input.Id).ToListAsync();
                var listProjectTaskOld = await WorkScope.GetAll<ProjectTask>().Where(pu => pu.ProjectId == input.Id).ToListAsync();
                var listProjectTargetUserOld = await WorkScope.GetAll<ProjectTargetUser>().Where(pu => pu.ProjectId == input.Id).ToListAsync();


                // Get ProjectUserNew, ProjectTaskNew, ProjectTargetUserNew
                var listProjectUserNew = input.Users.ToList();
                var listProjectTaskNew = input.Tasks.ToList();
                var listProjectTargetUserNew = input.ProjectTargetUsers.ToList();

                // Get ProjectUserToAdd, ProjectTaskToAdd, ProjectTargetUserToAdd
                var listProjectUserToAdd = listProjectUserNew.Where(punew =>
                {
                    if (!listProjectUserOld.Any(puold => puold.UserId == punew.UserId))
                        return true;
                    return false;
                }).Select(punew => punew);

                var listProjectTaskToAdd = listProjectTaskNew.Where(ptnew =>
                {
                    if (!listProjectTaskOld.Any(ptold => ptold.TaskId == ptnew.TaskId))
                        return true;
                    return false;
                }).Select(ptnew => ptnew);

                var listProjectTargetUserToAdd = listProjectTargetUserNew.Where(ptunew =>
                {
                    if (!listProjectTargetUserOld.Any(ptuold => ptuold.UserId == ptunew.UserId))
                        return true;
                    return false;
                }).Select(ptunew => ptunew);

                // Add ProjectUserToAdd, ProjectTaskToAdd, ProjectTargetUserToAdd to Database
                foreach (var itemNew in listProjectUserToAdd)
                {
                    var itemAdd = ObjectMapper.Map<ProjectUser>(itemNew);
                    itemAdd.ProjectId = input.Id;
                    await WorkScope.InsertAsync(itemAdd);
                }

                foreach (var itemNew in listProjectTaskToAdd)
                {
                    var itemAdd = ObjectMapper.Map<ProjectTask>(itemNew);
                    itemAdd.ProjectId = input.Id;
                    await WorkScope.InsertAsync(itemAdd);
                }

                foreach (var itemNew in listProjectTargetUserToAdd)
                {
                    var itemAdd = ObjectMapper.Map<ProjectTargetUser>(itemNew);
                    itemAdd.ProjectId = input.Id;
                    await WorkScope.InsertAsync(itemAdd);
                }

                // ProjectUserToUpdate/Delete, ProjectTaskToUpdate/Delete, ProjectTargetUserToUpdate/Delete
                foreach (var item in listProjectUserOld)
                {
                    var UserEditDelete = listProjectUserNew.SingleOrDefault(unew => unew.UserId == item.UserId);
                    if (UserEditDelete != null && (UserEditDelete.IsTemp != item.IsTemp || UserEditDelete.Type != item.Type))
                    {
                        item.IsTemp = UserEditDelete.IsTemp;
                        item.Type = UserEditDelete.Type;
                        await WorkScope.UpdateAsync(item);
                    }
                    else if (UserEditDelete == null)
                    {
                        await WorkScope.DeleteAsync(item);
                    }
                    continue;
                }

                foreach (var item in listProjectTaskOld)
                {
                    var TaskEditDelete = listProjectTaskNew.SingleOrDefault(tnew => tnew.TaskId == item.TaskId);
                    if (TaskEditDelete != null && (TaskEditDelete.Billable != item.Billable))
                    {
                        item.Billable = !item.Billable;
                        await WorkScope.UpdateAsync(item);
                    }
                    else if (TaskEditDelete == null)
                    {
                        await WorkScope.DeleteAsync(item);
                    }
                    continue;
                }

                foreach (var item in listProjectTargetUserOld)
                {
                    var TargetUserEditDelete = listProjectTargetUserNew.SingleOrDefault(unew => unew.UserId == item.UserId);
                    if (TargetUserEditDelete != null && (TargetUserEditDelete.RoleName != item.RoleName))
                    {
                        item.RoleName = TargetUserEditDelete.RoleName;
                        await WorkScope.UpdateAsync(item);
                    }
                    else if (TargetUserEditDelete == null)
                    {
                        await WorkScope.DeleteAsync(item);
                    }
                    continue;
                }

            }

            return input;
        }

        [HttpGet]
        public async Task<ProjectCreateEditDto> Get([FromQuery] long input)
        {
            var existProject = await WorkScope.GetAsync<Project>(input);
            if (existProject == null)
                throw new UserFriendlyException(string.Format("This projectId {0} doesn't exist", input));

            var tasks = await WorkScope.GetAll<ProjectTask>().Where(t => t.ProjectId == existProject.Id).Select(t => new ProjectTaskDto
            {
                Id = t.Id,
                TaskId = t.TaskId,
                Billable = t.Billable
            }).ToListAsync();

            var users = await WorkScope.GetAll<ProjectUser>().Where(u => u.ProjectId == existProject.Id).Select(u => new ProjectUserDto
            {
                Id = u.Id,
                UserId = u.UserId,
                Type = u.Type,
                IsTemp = u.IsTemp,
            }).ToListAsync();

            var targetUsers = await WorkScope.GetAll<ProjectTargetUser>().Where(ptu => ptu.ProjectId == existProject.Id).Select(ptu => new ProjectTargetUsersDto
            {
                Id = ptu.Id,
                UserId = ptu.UserId,
                RoleName = ptu.RoleName
            }).ToListAsync();

            var project = new ProjectCreateEditDto()
            {
                Id = existProject.Id,
                Name = existProject.Name,
                Code = existProject.Code,
                Status = existProject.Status,
                TimeStart = existProject.TimeStart,
                TimeEnd = existProject.TimeEnd,
                Note = existProject.Note,
                ProjectType = existProject.ProjectType,
                CustomerId = existProject.CustomerId,
                Tasks = tasks,
                Users = users,
                ProjectTargetUsers = targetUsers,
                KomuChannelId = existProject.KomuChannelId,
                IsNotifyToKomu = existProject.IsNotifyToKomu,
                IsNoticeKMSubmitTS = existProject.IsNoticeKMSubmitTS,
                IsNoticeKMRequestOffDate = existProject.IsNoticeKMRequestOffDate,
                IsNoticeKMApproveRequestOffDate = existProject.IsNoticeKMApproveRequestOffDate,
                IsNoticeKMRequestChangeWorkingTime = existProject.IsNoticeKMRequestChangeWorkingTime,
                IsNoticeKMApproveChangeWorkingTime = existProject.IsNoticeKMApproveChangeWorkingTime,
                isAllUserBelongTo = existProject.isAllUserBelongTo,
            };


            return project;
        }

        [HttpPost]
        public async System.Threading.Tasks.Task Inactive([FromBody] EntityDto<long> input)
        {
            var item = await WorkScope.GetAsync<Project>(input.Id);
            if (item != null)
            {
                item.Status = ProjectStatus.Deactive;
                await WorkScope.UpdateAsync(item);
            }
            else { throw new UserFriendlyException(string.Format("Project Id: {0} is not found", input.Id)); }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task Active([FromBody] EntityDto<long> input)
        {
            var item = await WorkScope.GetAsync<Project>(input.Id);
            if (item != null)
            {
                item.Status = ProjectStatus.Active;
                await WorkScope.UpdateAsync(item);
            }
            else { throw new UserFriendlyException(string.Format("Project Id: {0} is not found", input.Id)); }
        }

        [HttpDelete]
        public async System.Threading.Tasks.Task Delete(long Id)
        {
            var item = await WorkScope.GetAsync<Project>(Id);
            var listProjectTask = await WorkScope.GetAll<ProjectTask>().Where(pt => pt.ProjectId == Id).ToListAsync();
            var listProjectUser = await WorkScope.GetAll<ProjectUser>().Where(pt => pt.ProjectId == Id).ToListAsync();
            var listProjectTargetUser = await WorkScope.GetAll<ProjectTargetUser>().Where(pt => pt.ProjectId == Id).ToListAsync();
            bool existTimeSheet = false;

            foreach (var it in listProjectTask)
            {
                if (await WorkScope.GetAll<MyTimesheet>().AnyAsync(ts => ts.ProjectTaskId == it.Id))
                {
                    existTimeSheet = true;
                    break;
                }
            }

            if (existTimeSheet)
            {
                throw new UserFriendlyException(string.Format("My Timesheet is exist, you can't delete this project"));
            }

            /*await WorkScope.DeleteRangeAsync<ProjectTask>(listProjectTask);
            await WorkScope.DeleteRangeAsync<ProjectUser>(listProjectUser);
            await WorkScope.DeleteRangeAsync<ProjectTargetUser>(listProjectTargetUser);*/

            foreach (var it in listProjectTask)
            {
                if (it.ProjectId == Id)
                {
                    await WorkScope.GetRepo<ProjectTask>().HardDeleteAsync(it);
                }
            }

            foreach (var it in listProjectUser)
            {
                if (it.ProjectId == Id)
                {
                    await WorkScope.GetRepo<ProjectUser>().HardDeleteAsync(it);
                }
            }

            foreach (var it in listProjectTargetUser)
            {
                if (it.ProjectId == Id)
                {
                    await WorkScope.GetRepo<ProjectTargetUser>().HardDeleteAsync(it);
                }
            }

            await WorkScope.GetRepo<Project>().HardDeleteAsync(item);
        }

        [HttpGet]
        public async Task<List<ProjectsIncludingTasksDto>> GetProjectsIncludingTasks()
        {
            var myListProject = await WorkScope.GetAll<ProjectUser>().Where(p => p.UserId == AbpSession.UserId)
                                .Include(pu => pu.Project)
                                .Include(pu => pu.Project.Customer)
                                .Where(pu => pu.Project.Status == ProjectStatus.Active)
                                .Select(pu => new ProjectsIncludingTasksDto
                                {
                                    ProjectName = pu.Project.Name,
                                    CustomerName = pu.Project.Customer.Name,
                                    ProjectCode = pu.Project.Code,
                                    ProjectUserType = pu.Type,
                                    ListPM = WorkScope.GetAll<ProjectUser>().Where(p => p.ProjectId == pu.ProjectId).Include(p => p.User).Where(p => p.Type == ProjectUserType.PM).Select(p => p.User.FullName).ToList(),
                                    Tasks = WorkScope.GetAll<ProjectTask>().Where(pt => pt.ProjectId == pu.ProjectId).Include(pt => pt.Task).OrderBy(pt => pt.Task.Id).Select(pt => new TimeSheetTaskDto
                                    {
                                        ProjectTaskId = pt.Id,
                                        TaskName = pt.Task.Name,
                                        Billable = pt.Billable,
                                        IsDefault = pu.User.DefaultProjectTaskId == pt.Id,
                                    }).ToList(),
                                    TargetUsers = WorkScope.GetAll<ProjectTargetUser>().Where(ptu => ptu.ProjectId == pu.ProjectId).Include(ptu => ptu.User).Select(ptu => new TimeSheetTargetUserDto
                                    {
                                        ProjectTargetUserId = ptu.Id,
                                        UserName = ptu.User.FullName
                                    }).ToList(),
                                    Id = pu.ProjectId
                                }).ToListAsync();

            return myListProject;
        }


        [HttpPost]
        public async System.Threading.Tasks.Task UpdateDefaultProjectTask(EntityDto<long> input)
        {
            var userId = AbpSession.UserId;
            if (userId != null)
            {
                var currentUser = await WorkScope.GetAsync<User>(AbpSession.UserId.Value);
                currentUser.DefaultProjectTaskId = input.Id;
                await WorkScope.UpdateAsync(currentUser);
            }
            else
            {
                throw new UserFriendlyException(string.Format("Please log in before proceeding."));
            }

        }

        [HttpPost]
        public async System.Threading.Tasks.Task ClearDefaultProjectTask()
        {
            var userId = AbpSession.UserId;
            if (userId != null)
            {
                var currentUser = await WorkScope.GetAsync<User>(AbpSession.UserId.Value);
                currentUser.DefaultProjectTaskId = null;
                await WorkScope.UpdateAsync(currentUser);
            }
            else
            {
                throw new UserFriendlyException(string.Format("Please log in before proceeding."));
            }

        }

    }
}
