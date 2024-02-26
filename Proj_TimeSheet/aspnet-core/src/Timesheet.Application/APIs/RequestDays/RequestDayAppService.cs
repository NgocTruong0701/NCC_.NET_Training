using Abp.Authorization;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Ncc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timesheet.APIs.MyAbsenceDays.Dto;
using Timesheet.Entities;
using static Ncc.Entities.Enum.StatusEnum;
using System.Linq;
using Ncc.Authorization.Users;
using Abp.BackgroundJobs;
using Timesheet.BackgroundJob;
using Ncc.Entities;
using Ncc.Configuration;
using Timesheet.DomainServices.Dto;
using Timesheet.Uitls;
using Timesheet.DomainServices;
using Timesheet.APIs.RequestDays.Dto;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Ncc.IoC;
using Timesheet.Services.Komu;

namespace Timesheet.APIs.RequestDays
{
    [AbpAuthorize]
    public class RequestDayAppService : AppServiceBase
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ITimekeepingServices _timeKeepingService;
        private readonly KomuService _komuService;

        public RequestDayAppService(IBackgroundJobManager backgroundJobManager, KomuService komuService,
            ITimekeepingServices timeKeepingService, IWorkScope workScope) : base(workScope)
        {
            _backgroundJobManager = backgroundJobManager;
            _timeKeepingService = timeKeepingService;
            _komuService = komuService;
        }

        [HttpPost]
        [AbpAuthorize(Ncc.Authorization.PermissionNames.AbsenceDayByProject_View)]
        public async Task<List<GetRequestDto>> GetAllRequest(InputRequestDto input)
        {
            var projectPMs = await WorkScope.GetAll<ProjectUser>()
                .Where(s => s.UserId == AbpSession.UserId)
                .Where(s => s.Project.Status == ProjectStatus.Active)
                .Where(s => s.Type == ProjectUserType.PM)
                .Select(s => s.ProjectId)
                .ToListAsync();

            if (input.projectIds.Except(projectPMs).Count() > 0)
            {
                throw new UserFriendlyException(string.Format("You aren't the PM of the selected project"));
            }
            var projectMember = await WorkScope.GetAll<ProjectUser>().Where(s => input.projectIds.Contains(s.ProjectId))
                .Where(s => s.Type != ProjectUserType.DeActive)
                .Select(s => s.UserId).Distinct().ToListAsync();

            RequestStatus[] arrayAbsenceStatus = new RequestStatus[] { RequestStatus.Pending, RequestStatus.Pending, RequestStatus.Approved, RequestStatus.Rejected };

            var query = WorkScope.All<AbsenceDayDetail>()
                  .Where(s => s.DateAt >= input.startDate)
                  .Where(s => s.DateAt <= input.endDate)
                  .Where(s => !input.status.HasValue || input.status.Value < 0 ||
                           (input.status.Value == 0 ? (s.Request.Status == RequestStatus.Pending || s.Request.Status == RequestStatus.Approved) :
                           s.Request.Status == arrayAbsenceStatus[input.status.Value]))
                  .Where(s => !input.type.HasValue || input.type.Value < 0 || s.Request.Type == input.type.Value)
                  .Where(s => string.IsNullOrWhiteSpace(input.name) || s.Request.User.EmailAddress.Contains(input.name))
                  .Where(s => projectMember.Contains(s.Request.UserId))
                  .Where(s => input.dayOffTypeId < 0 || s.Request.DayOffTypeId == input.dayOffTypeId)
                  .Select(s => new GetRequestDto
                  {
                      Id = s.Request.Id,
                      UserId = s.Request.UserId,
                      AvatarPath = s.Request.User.AvatarPath,
                      Sex = s.Request.User.Sex,
                      FullName = s.Request.User.FullName,
                      Name = s.Request.User.Name,
                      //Level = s.Request.User.Level,
                      Type = s.Request.User.Type,
                      DateAt = s.DateAt,
                      DateType = s.DateType,
                      DayOffName = s.Request.DayOffType.Name,
                      Hour = s.Hour,
                      Reason = s.Request.Reason,
                      Status = s.Request.Status,
                      ShortName = s.Request.User.Name,
                      LeavedayType = s.Request.Type,
                      BranchDisplayName = s.Request.User.Branch.DisplayName,
                      BranchColor = s.Request.User.Branch.Color,
                      AbsenceTime = s.AbsenceTime
                  });

            return await query.ToListAsync();
        }
        //TODO: test GetAllRequestForUser funtion
        [HttpPost]
        public async Task<List<GetRequestDto>> GetAllRequestForUser(InputRequestDto input)
        {
            var activeMemberIds = await WorkScope.GetAll<ProjectUser>()
                .Where(s => input.projectIds.Contains(s.ProjectId))
                .Where(s => s.Type != ProjectUserType.DeActive)
                .Select(s => s.UserId).Distinct().ToListAsync();

            RequestStatus[] arrayAbsenceStatus = new RequestStatus[] { RequestStatus.Pending, RequestStatus.Pending, RequestStatus.Approved, RequestStatus.Rejected };

            var qUser = WorkScope.All<User>().Select(s => new
            {
                s.Id,
                s.FullName
            });

            var query = from s in WorkScope.GetAll<AbsenceDayDetail>()
                  .Where(s => s.DateAt >= input.startDate)
                  .Where(s => s.DateAt.Date <= input.endDate)
                  .Where(s => activeMemberIds.Contains(s.Request.UserId))
                  .Where(s => !input.status.HasValue || input.status.Value < 0 ||
                           (input.status.Value == 0 ? (s.Request.Status == RequestStatus.Pending || s.Request.Status == RequestStatus.Approved) :
                           s.Request.Status == arrayAbsenceStatus[input.status.Value]))
                  .Where(s => !input.type.HasValue || input.type.Value < 0 || s.Request.Type == input.type.Value)
                  .Where(s => string.IsNullOrWhiteSpace(input.name) || s.Request.User.EmailAddress.Contains(input.name))
                  .Where(s => input.dayOffTypeId < 0 || s.Request.DayOffTypeId == input.dayOffTypeId)
                  
                    join u in qUser on s.Request.LastModifierUserId equals u.Id into updatedUser
                    join cu in qUser on s.CreatorUserId equals cu.Id into cuu
                    select new GetRequestDto
                  {
                      Id = s.Request.Id,
                      UserId = s.Request.UserId,
                      AvatarPath = s.Request.User.AvatarPath,
                      Sex = s.Request.User.Sex,
                      FullName = s.Request.User.FullName,
                      Name = s.Request.User.Name,
                      //Level = s.Request.User.Level,
                      Type = s.Request.User.Type,
                      DateAt = s.DateAt,
                      DateType = s.DateType,
                      DayOffName = s.Request.DayOffType.Name,
                      Hour = s.Hour,
                      Status = s.Request.Status,
                      ShortName = s.Request.User.Name,
                      LeavedayType = s.Request.Type,
                      BranchDisplayName = s.Request.User.Branch.DisplayName,
                      BranchColor = s.Request.User.Branch.Color,
                      AbsenceTime = s.AbsenceTime,
                      CreateTime = s.CreationTime,
                      CreateBy = cuu.Select(x => x.FullName).FirstOrDefault(),
                      LastModificationTime = s.Request.LastModificationTime,
                      LastModifierUserName = updatedUser.Select(x => x.FullName).FirstOrDefault(), 
                    };

            return await query.ToListAsync();
        }

        private async Task<NotifyUserInfoDto> getNotifyUserInfoDto(long userId)
        {
            return await WorkScope.GetAll<User>().Where(s => s.Id == userId)
                .Select(user => new NotifyUserInfoDto
                {
                    Branch = user.BranchOld,
                    EmailAddress = user.EmailAddress,
                    FullName = user.FullName,
                    KomuUserId = user.KomuUserId,
                    Type = user.Type,
                    UserId = user.Id,
                    BranchDisplayName = user.Branch.DisplayName,
                }).FirstOrDefaultAsync();
        }

        [AbpAuthorize(Ncc.Authorization.PermissionNames.MyAbsenceDay_SendRequest)]
        public async Task<MyRequestDto> SubmitToPending(MyRequestDto input)
        {
            var userId = AbpSession.UserId.Value;

            var user = await WorkScope.GetAll<User>()
               .Where(s => s.Id == userId)
               .Select(s => new
               {
                   NotiUserInfo = new NotifyUserInfoDto
                   {
                       Branch = s.BranchOld,
                       EmailAddress = s.EmailAddress,
                       FullName = s.FullName,
                       Type = s.Type,
                       KomuUserId = s.KomuUserId,
                       UserId = s.Id,
                       BranchDisplayName = s.Branch.DisplayName
                   },
                   MorningWorking = s.MorningWorking.HasValue ? s.MorningWorking.Value : 3.5,
                   AfternoonWorking = s.AfternoonWorking.HasValue ? s.AfternoonWorking.Value : 4.5,
               }).FirstOrDefaultAsync();


            var allowInternToWorkRemote = await SettingManager.GetSettingValueAsync(AppSettingNames.AllowInternToWorkRemote);

            if (allowInternToWorkRemote != "true" && user.NotiUserInfo.Type == Usertype.Internship && input.Type == RequestType.Remote)
            {
                throw new UserFriendlyException("Intern is not allow to work REMOTE at this time");
            }

            var requestDateAts = input.Absences.Select(s => s.DateAt.Date).ToList();
            var dbRequests = await WorkScope.GetAll<AbsenceDayDetail>()
                .Where(s => s.Request.UserId == userId)
                .Select(s => new { Detail = s, s.Request })
                .ToListAsync();

            var dbPendingOrApprovedDateAts = dbRequests.Where(s => s.Request.Status != RequestStatus.Rejected)
                .Select(s => s.Detail.DateAt.Date).ToList();

            var isAlreadyExist = input.Absences.Any(s => dbPendingOrApprovedDateAts.Contains(s.DateAt));
            if (isAlreadyExist)
            {
                throw new UserFriendlyException("Some Date in the request already exist in DB. Refresh to load new data.");
            }



            var requester = user.NotiUserInfo;

            await validateOffType(input, requester.Type);

            var setDayOffSetting = WorkScope.GetAll<DayOffSetting>()
                .Select(s => s.DayOff.Date)
                .ToHashSet();

            int MAX_ALLOW_REMOTE_DAY = 2;

            int.TryParse(await SettingManager.GetSettingValueAsync(AppSettingNames.WFHSetting), out MAX_ALLOW_REMOTE_DAY);

            var mapDateAtToRequestCount = new Dictionary<DateTime, int>();

            foreach (var abs in input.Absences)
            {
                if (setDayOffSetting.Contains(abs.DateAt))
                {
                    throw new UserFriendlyException(string.Format("{0} is not a working day.", DateTimeUtils.ToString(abs.DateAt)));
                }
                //insert absence day request
                var absencedayRequest = new AbsenceDayRequest
                {
                    UserId = userId,
                    DayOffTypeId = input.DayOffTypeId,
                    Type = input.Type,
                    Reason = input.Reason,
                    Status = RequestStatus.Pending
                };
                if (abs.DateType == DayType.Custom && (abs.Hour >= 7.5 || abs.Hour <= 0))
                {
                    throw new UserFriendlyException(string.Format("You can't submit absence hour > 7.5h  or absence hour = 0h "));
                }

                //insert absence day detail
                var absenceDayDetail = new AbsenceDayDetail
                {
                    RequestId = 0,
                    DateAt = abs.DateAt,
                    DateType = abs.DateType,
                    Hour = abs.Hour,
                    AbsenceTime = abs.AbsenceTime
                };

                if (abs.DateType != DayType.Custom)
                {
                    if (abs.DateType == DayType.Fullday) absenceDayDetail.Hour = 8;
                    else if (abs.DateType == DayType.Morning)
                    {
                        absenceDayDetail.Hour = user.MorningWorking;
                    }
                    else
                    {
                        absenceDayDetail.Hour = user.AfternoonWorking;
                    }
                }

                //Check is Rejected
                var rejectedAbsDetail = dbRequests.Where(s => s.Detail.DateAt.Date == abs.DateAt.Date)
                    .Where(s => s.Request.Status == RequestStatus.Rejected)
                    .Select(s => s.Detail)
                    .FirstOrDefault();

                if (rejectedAbsDetail != null)
                {
                    await WorkScope.GetRepo<AbsenceDayRequest>().DeleteAsync(rejectedAbsDetail.RequestId);
                    await WorkScope.GetRepo<AbsenceDayDetail>().DeleteAsync(rejectedAbsDetail.Id);
                }

                if (input.Type == RequestType.Remote)
                {
                    var monday = DateTimeUtils.FirstDayOfWeek(abs.DateAt);
                    var numberRemoteDayInWeek = 0;
                    if (mapDateAtToRequestCount.ContainsKey(monday))
                    {
                        numberRemoteDayInWeek = mapDateAtToRequestCount[monday];
                    }
                    else
                    {
                        numberRemoteDayInWeek = CountRemoteDayOfUserInWeek(abs.DateAt, userId);
                        mapDateAtToRequestCount.Add(monday, numberRemoteDayInWeek);
                    }

                    if (numberRemoteDayInWeek >= MAX_ALLOW_REMOTE_DAY)
                    {
                        absencedayRequest.Status = RequestStatus.Rejected;
                    }
                    else
                    {
                        mapDateAtToRequestCount[monday] = numberRemoteDayInWeek + 1;
                    }
                }
                var requestId = await WorkScope.InsertAndGetIdAsync(absencedayRequest);
                absenceDayDetail.RequestId = requestId;

                await WorkScope.InsertAsync(absenceDayDetail);
                CurrentUnitOfWork.SaveChanges();
                //if (input.Type == RequestType.Off)
                //{
                //    await processTimekeepingForRequestOff(requestUser, abs);
                //}
                abs.Status = absencedayRequest.Status;
            }

            await notify(requester, input);
            return input;
        }

        [AbpAuthorize(Ncc.Authorization.PermissionNames.MyAbsenceDay_SendRequest)]
        public async Task<MyRequestDto> SubmitToPendingNew(MyRequestDto input)
        {
            var userId = AbpSession.UserId.Value;

            var requester = GetSessionUserInfoDto();

            var allowInternToWorkRemote = await SettingManager.GetSettingValueAsync(AppSettingNames.AllowInternToWorkRemote);

            if (allowInternToWorkRemote != "true" && requester.Type == Usertype.Internship && input.Type == RequestType.Remote)
            {
                throw new UserFriendlyException("Intern is not allow to work REMOTE at this time");
            }

            if(input.Type != RequestType.Remote && string.IsNullOrEmpty(input.Reason))
            {
                throw new UserFriendlyException("Reason is require!");
            }    

            var requestDateAts = input.Absences.Select(s => s.DateAt.Date);

            var dbRequests = (from r in WorkScope.GetAll<AbsenceDayRequest>()
                              join d in WorkScope.GetAll<AbsenceDayDetail>()
                                  .Where(s => s.Request.UserId == userId)
                                  .Where(s => requestDateAts.Contains(s.DateAt.Date))
                              on r.Id equals d.RequestId
                              select new RequestInfoDto
                              {
                                  Type = r.Type,
                                  AbsenceTime = d.AbsenceTime,
                                  Date = d.DateAt.Date,
                                  DateType = d.DateType,
                                  Hour = d.Hour,
                                  Id = d.Id,
                                  RequestId = r.Id,
                                  Status = r.Status
                              }).ToList();

            validateRequests(userId, input, dbRequests);          

            await validateOffType(input, requester.Type);

            var setDayOffSetting = WorkScope.GetAll<DayOffSetting>()
                .Select(s => s.DayOff.Date)
                .ToHashSet();

            int MAX_ALLOW_REMOTE_DAY = 3;

            int.TryParse(await SettingManager.GetSettingValueAsync(AppSettingNames.WFHSetting), out MAX_ALLOW_REMOTE_DAY);

            var mapDateAtToRequestCount = new Dictionary<DateTime, int>();

            foreach (var abs in input.Absences)
            {
                if (setDayOffSetting.Contains(abs.DateAt))
                {
                    throw new UserFriendlyException(string.Format("{0} is not a working day.", DateTimeUtils.ToString(abs.DateAt)));
                }
                //insert absence day request
                var absencedayRequest = new AbsenceDayRequest
                {
                    UserId = userId,
                    DayOffTypeId = input.DayOffTypeId,
                    Type = input.Type,
                    Reason = input.Reason,
                    Status = RequestStatus.Pending
                };

                //insert absence day detail
                var absenceDayDetail = new AbsenceDayDetail
                {
                    RequestId = 0,
                    DateAt = abs.DateAt,
                    DateType = abs.DateType,
                    Hour = abs.Hour,
                    AbsenceTime = abs.AbsenceTime
                };

                if (abs.DateType != DayType.Custom)
                {
                    if (abs.DateType == DayType.Fullday) absenceDayDetail.Hour = 8;
                    else if (abs.DateType == DayType.Morning)
                    {
                        absenceDayDetail.Hour = requester.MorningWorking;
                    }
                    else
                    {
                        absenceDayDetail.Hour = requester.AfternoonWorking;
                    }
                }

                //Check is Rejected
                var rejectedAbsDetail = dbRequests.Where(s => s.Date == abs.DateAt.Date)
                                                  .Where(s => s.Status == RequestStatus.Rejected)
                                                  .Select(s => new {s.RequestId, s.Id})
                                                  .FirstOrDefault();

                if (rejectedAbsDetail != null)
                {
                    await WorkScope.GetRepo<AbsenceDayRequest>().DeleteAsync(rejectedAbsDetail.RequestId);
                    await WorkScope.GetRepo<AbsenceDayDetail>().DeleteAsync(rejectedAbsDetail.Id);
                }

                if (input.Type == RequestType.Remote)
                {
                    var monday = DateTimeUtils.FirstDayOfWeek(abs.DateAt);
                    var numberRemoteDayInWeek = 0;
                    if (mapDateAtToRequestCount.ContainsKey(monday))
                    {
                        numberRemoteDayInWeek = mapDateAtToRequestCount[monday];
                    }
                    else
                    {
                        numberRemoteDayInWeek = CountRemoteDayOfUserInWeek(abs.DateAt, userId);
                        mapDateAtToRequestCount.Add(monday, numberRemoteDayInWeek);
                    }

                    if (numberRemoteDayInWeek > MAX_ALLOW_REMOTE_DAY - 1)
                    {
                        absencedayRequest.Status = RequestStatus.Rejected;
                    }
                    else
                    {
                        mapDateAtToRequestCount[monday] = numberRemoteDayInWeek + 1;
                    }
                }
                var requestId = await WorkScope.InsertAndGetIdAsync(absencedayRequest);
                absenceDayDetail.RequestId = requestId;

                await WorkScope.InsertAsync(absenceDayDetail);
                CurrentUnitOfWork.SaveChanges();

                abs.Status = absencedayRequest.Status;
            }
            
            await notify(requester, input);
            return input;
        }
        private void validateRequests(long userId, MyRequestDto input, IEnumerable<RequestInfoDto> dbRequests)
        {
            // Không có request

            if (input.Absences.Count() == 0)
            {
                throw new UserFriendlyException("Requests không hợp lệ");
            }    


            // Đi muộn về sớm nhưng không phải request off
           
            if (!input.IsValidDiMuonVeSom())
            {
                throw new UserFriendlyException($"Request đi muộn/về sớm các ngày sau không đúng định dạng: {input.GetInValidDiMuonVeSom()}" );
            }

            var dbMyRequestDetail = dbRequests
                                    .Where(s => s.Status != RequestStatus.Rejected);

            // Thời gian đi muộn + về sớm vượt quá

            var MaxDiMuonVeSomHour = double.Parse(SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.TotalTimeAbsenceTime).Result);

            var dicDiMuonVeSomDB = dbMyRequestDetail.Where(s => s.Type == RequestType.Off)
                .Where(s => s.DateType == DayType.Custom)
                .GroupBy(s => s.Date)
                .ToDictionary(s => s.Key, s => s.Sum(x => x.Hour));

            var arrDiMuonVeSomOver = input.Absences.Where(s => s.DateType == DayType.Custom)
                .Select(s => new
            {
                s.DateAt.Date,
                Hour= s.Hour + (dicDiMuonVeSomDB.ContainsKey(s.DateAt.Date) ? dicDiMuonVeSomDB[s.DateAt.Date] : 0)
            }).Where(s => s.Hour > MaxDiMuonVeSomHour)
            .Select(s => s.Date.ToString("dd-MM-yyyy")).ToArray();

            if (arrDiMuonVeSomOver.Count() > 0)
            {
                throw new UserFriendlyException(string.Format($"Tổng thời gian đi muộn/về sớm vượt quá số giờ quy định ({MaxDiMuonVeSomHour}h): " +
                    $"{string.Join(", ", arrDiMuonVeSomOver)}"));
            }    
                

            // validate 
            var inputRequests = input.Absences.Select(s => new
            {
                s.DateAt,
                Request = new MyRequest
                {
                    Type = input.Type,
                    DateType = s.DateType,
                    AbsenceTime = s.AbsenceTime
                }
            }).AsEnumerable();
                                    

            var dbMyRequestDays = dbMyRequestDetail
                                    .GroupBy(s => s.Date)
                                    .ToDictionary(s => s.Key, 
                                                  s => s.Select(x => new MyRequest
                                                  {
                                                      Type = x.Type,
                                                      AbsenceTime = x.AbsenceTime,
                                                      DateType = x.DateType
                                                  })
                                    );

            foreach (var dayRequest in inputRequests)
            {
                //if(dayRequest.Request == null)
                //    throw new UserFriendlyException(string.Format("Request null at " + dayRequest.DateAt));

                if (dbMyRequestDays.ContainsKey(dayRequest.DateAt) && !RequestUtils.checkValidate(dbMyRequestDays[dayRequest.DateAt], dayRequest.Request))
                    throw new UserFriendlyException($"Request invalid on {dayRequest.DateAt.Date.ToString("dd-MM-yyyy")}");
            }    

        }

        private async System.Threading.Tasks.Task validateOffType(MyRequestDto input, Usertype? userType)
        {
            if (input.Type == RequestType.Off)
            {
                //check length Absenceday
                double countDay = 0;
                foreach (var abs in input.Absences)
                {
                    if (abs.DateType == DayType.Fullday) countDay++;
                    else countDay += 0.5;
                }
                var dayOffType = await WorkScope.GetAll<DayOffType>()
                    .Where(s => s.Id == input.DayOffTypeId)
                    .Select(s => new { s.Status, s.Length }).FirstOrDefaultAsync();

                if (dayOffType.Status == OffTypeStatus.CoPhep && (!userType.HasValue || userType != Usertype.Staff))
                {//ko mat ngay phep
                    throw new UserFriendlyException(string.Format("Your Type is not STAFF. The Off type is allow for STAFF Only!"));
                }

                if (countDay > dayOffType.Length && dayOffType.Status == OffTypeStatus.CoPhep)
                    throw new UserFriendlyException(string.Format("Bạn KHÔNG thể nghỉ quá số ngày phép cho phép!"));

            }
        }

        private async System.Threading.Tasks.Task processTimekeepingForRequestOff(User requestUser, AbsenceDayDetailDto abs)
        {
            if (abs.DateAt > DateTimeUtils.GetNow().Date)
            {
                return;
            }
            var userId = requestUser.Id;
            var userInfo = ObjectMapper.Map<UserDto>(requestUser);

            var timeKeeping = await WorkScope.GetAll<Timekeeping>()
           .Where(s => s.DateAt.Date == abs.DateAt.Date && s.UserId == userId)
           .FirstOrDefaultAsync();

            if (timeKeeping != default && userInfo != default)
            {
                Dictionary<long, MapAbsenceUserDto> mapAbsenceUsers = new Dictionary<long, MapAbsenceUserDto>();
                mapAbsenceUsers.Add(userId, new MapAbsenceUserDto { UserId = userId, DateType = abs.DateType, AbsenceTime = abs.AbsenceTime, Hour = abs.Hour });
                TimesheetUserDto user = new TimesheetUserDto
                {
                    UserId = userId,
                    AfternoonEndAt = requestUser.AfternoonEndAt,
                    AfternoonStartAt = requestUser.AfternoonStartAt,
                    MorningWorking = requestUser.MorningWorking,
                    AfternoonWorking = requestUser.AfternoonWorking,
                    MorningEndAt = requestUser.MorningEndAt,
                    MorningStartAt = requestUser.MorningStartAt
                };

                var t = _timeKeepingService.CaculateCheckInOutTime(mapAbsenceUsers, user);

                timeKeeping.RegisterCheckIn = t.CheckIn;
                timeKeeping.RegisterCheckOut = t.CheckOut;
                _timeKeepingService.CheckIsPunished(timeKeeping);

                await WorkScope.GetRepo<Timekeeping>().UpdateAsync(timeKeeping);
            }

        }
        private int CountRemoteDayOfUserInWeek(DateTime day, long userId)
        {
            DateTime startDayOfWeek = DateTimeUtils.FirstDayOfWeek(day);
            DateTime endDayOfWeek = DateTimeUtils.LastDayOfWeek(day);

            return WorkScope.All<AbsenceDayDetail>()
                .Include(s => s.Request)
                .Where(s => s.Request.UserId == userId)
                .Where(s => s.Request.Type == RequestType.Remote)
                .Where(s => s.Request.Status != RequestStatus.Rejected)
                .Where(s => s.DateAt >= startDayOfWeek && s.DateAt.Date <= endDayOfWeek)
                .Select(s => s.DateAt.Date)
                .Distinct()
                .Count();
        }

        private async System.Threading.Tasks.Task notify(NotifyUserInfoDto requester, MyRequestDto input)
        {
            var receivers = await getReceiverSendRequestList(requester.UserId);
            var offTypeName = "";
            if (input.Type == RequestType.Off)
            {
                offTypeName = await WorkScope.GetAll<DayOffType>()
                    .Where(s => s.Id == input.DayOffTypeId)
                    .Select(s => s.Name)
                    .FirstOrDefaultAsync();
            }

            await notifyEmailWhenSubmitRequest(requester, input, receivers, offTypeName);
            await notifyKomuWhenSubmitRequest(requester, input, receivers, offTypeName);
        }

        private async System.Threading.Tasks.Task notifyKomuWhenSubmitRequest(NotifyUserInfoDto requester, MyRequestDto input, List<ProjectPMDto> receivers, string offTypeName)
        {
            var enableNotify = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.SendKomuRequest);
            if (enableNotify != "true")
            {
                Logger.Info("notifyKomuWhenSubmitRequest() SendKomuRequest=" + enableNotify + ", AbpSessionUserId=" + AbpSession.UserId);
                return;
            }
            var alreadySentToPMIds = new HashSet<long>();
            foreach (var project in receivers)
            {
                if (!project.IsNotifyKomu)
                {
                    Logger.Info($"notifyKomuWhenSubmitRequest() projectId={project.ProjectId}, IsNotifyKomu={project.IsNotifyKomu}, KomuChannelId={project.KomuChannelId}");
                }
                else
                {
                    var komuMessage = $"PM {project.KomuPMsTag(alreadySentToPMIds)}: {requester.KomuAccountInfo} " +
                        $"has sent a request **{input.GetRequestName(offTypeName)}** " +
                        $"for following dates:\n ```{input.ToKomuStringRequestDates()}```" +
                        $"Reason: ```{input.Reason}```";

                    _komuService.NotifyToChannel(komuMessage, project.KomuChannelId);
                    processAlreadySentToPMs(alreadySentToPMIds, project.PMs);
                }
            }
        }

        private async System.Threading.Tasks.Task notifyEmailWhenSubmitRequest(NotifyUserInfoDto requester, MyRequestDto input, List<ProjectPMDto> receivers, string offTypeName)
        {
            var enableNotify = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.SendEmailRequest);
            if (enableNotify != "true")
            {
                Logger.Info("notifyEmailWhenSubmitRequest() SendEmailRequest=" + enableNotify + ", AbpSessionUserId=" + AbpSession.UserId);
                return;
            }

            var emailSubject = $"{requester.ToEmailString()} sent a {input.GetRequestName(offTypeName)} request {input.ListDay()}";

            var emailBody = $@"<h3>{input.GetRequestName(offTypeName)} request is waiting to be approved or rejected!</h3>
                                <hr>
                                   <p>Name: {requester.FullName}</p>
                                   <p>Reason: {input.Reason}</p>
                                   {(input.Type == RequestType.Off ? $"<p>Off type: {offTypeName}</p>" : "")}
                                <div>
                                    <table> 
                                            <tr style='font-size: 13px'>
                                                <td>Time: </td>
                                                <td>{input.ToEmailString()}</td>
                                            </tr>
                                    </table>
                                 </div>";

            var targetEmails = getPMEmails(receivers);
            var hrEmails = await getHREmails();

            targetEmails.AddRange(hrEmails);
            targetEmails = targetEmails.Distinct().ToList();

            await _backgroundJobManager.EnqueueAsync<EmailBackgroundJob, EmailBackgroundJobArgs>(new EmailBackgroundJobArgs
            {
                TargetEmails = targetEmails,
                Body = emailBody,
                Subject = emailSubject
            }, BackgroundJobPriority.High, new TimeSpan(TimeSpan.TicksPerMinute));
        }

        private List<string> getPMEmails(List<ProjectPMDto> PMs)
        {
            var result = new List<string>();
            foreach (var pm in PMs)
            {
                if (pm.IsNotifyEmail)
                {
                    result.AddRange(pm.PMs.Select(s => s.EmailAddress).ToList());
                }
            }
            return result;
        }

        private async Task<List<string>> getHREmails()
        {
            var result = new List<string>();

            var emailHR = await SettingManager.GetSettingValueAsync(AppSettingNames.EmailHR);
            result.Add(Convert.ToString(emailHR));

            return result;
        }

        public async Task<List<ProjectPMDto>> getReceiverSendRequestList(long requesterId)
        {
            var qrequesterInProjectIds = WorkScope.GetAll<ProjectUser>()
            .Where(s => s.UserId == requesterId)
            .Where(s => s.Project.Status == ProjectStatus.Active)
            .Where(s => s.Type != ProjectUserType.DeActive)
            .Select(s => s.ProjectId)
            .Distinct();

            var queryPMs = WorkScope.GetAll<ProjectUser>()
            .Where(s => s.Project.Status == ProjectStatus.Active)
            .Where(s => s.Type == ProjectUserType.PM)
            .Select(s => new
            {
                s.User.KomuUserId,
                s.User.FullName,
                s.User.EmailAddress,
                s.UserId,
                s.ProjectId,
                s.Project.KomuChannelId,
                s.Project.IsNotifyToKomu,
                s.Project.IsNoticeKMRequestOffDate,
            });

            var result = await (from projectId in qrequesterInProjectIds
                                join pm in queryPMs on projectId equals pm.ProjectId
                                select pm)
                              .GroupBy(s => new { s.ProjectId, s.KomuChannelId, s.IsNoticeKMRequestOffDate})
                              .Select(s => new ProjectPMDto
                              {
                                  ProjectId = s.Key.ProjectId,
                                  KomuChannelId = s.Key.KomuChannelId,
                                  IsNotifyKomu = s.Key.IsNoticeKMRequestOffDate,
                                  IsNotifyEmail = false,
                                  PMs = s.Select(x => new NotifyUserInfoDto
                                  {
                                      EmailAddress = x.EmailAddress,
                                      FullName = x.FullName,
                                      KomuUserId = x.KomuUserId,
                                      UserId = x.UserId
                                  }).ToList()
                              }).ToListAsync();
            return result;
        }

        public async Task<List<ProjectPMDto>> getReceiverApproveRejectList(long requesterId)
        {
            var qrequesterInProjectIds = WorkScope.GetAll<ProjectUser>()
            .Where(s => s.UserId == requesterId)
            .Where(s => s.Project.Status == ProjectStatus.Active)
            .Where(s => s.Type != ProjectUserType.DeActive)
            .Select(s => s.ProjectId)
            .Distinct();

            var queryPMs = WorkScope.GetAll<ProjectUser>()
            .Where(s => s.Project.Status == ProjectStatus.Active)
            .Where(s => s.Type == ProjectUserType.PM)
            .Select(s => new
            {
                s.User.KomuUserId,
                s.User.FullName,
                s.User.EmailAddress,
                s.UserId,
                s.ProjectId,
                s.Project.KomuChannelId,
                s.Project.IsNotifyToKomu,
                s.Project.IsNoticeKMApproveRequestOffDate,
            });

            var result = await (from projectId in qrequesterInProjectIds
                                join pm in queryPMs on projectId equals pm.ProjectId
                                select pm)
                              .GroupBy(s => new { s.ProjectId, s.KomuChannelId, s.IsNoticeKMApproveRequestOffDate })
                              .Select(s => new ProjectPMDto
                              {
                                  ProjectId = s.Key.ProjectId,
                                  KomuChannelId = s.Key.KomuChannelId,
                                  IsNotifyKomu = s.Key.IsNoticeKMApproveRequestOffDate,
                                  IsNotifyEmail = false,
                                  PMs = s.Select(x => new NotifyUserInfoDto
                                  {
                                      EmailAddress = x.EmailAddress,
                                      FullName = x.FullName,
                                      KomuUserId = x.KomuUserId,
                                      UserId = x.UserId
                                  }).ToList()
                              }).ToListAsync();
            return result;
        }

        [AbpAuthorize(Ncc.Authorization.PermissionNames.MyAbsenceDay_View)]
        public async Task<List<GetMyRequestDto>> GetAllMyRequest(DateTime? startDate, DateTime? endDate, RequestType? type)
        {
            var query = from u in WorkScope.GetAll<AbsenceDayRequest>().Where(s => !type.HasValue || s.Type == type)
                        join t in WorkScope.GetAll<AbsenceDayDetail>()
                        .Where(s => !startDate.HasValue || s.DateAt >= startDate)
                        .Where(s => !endDate.HasValue || s.DateAt <= endDate)
                        .Where(s => s.Request.UserId == AbpSession.UserId.Value)
                        on u.Id equals t.RequestId
                        select new GetMyRequestDto
                        {
                            Id = u.Id,
                            DayOffName = u.DayOffType.Name,
                            FullName = u.User.FullName,
                            UserId = u.UserId,
                            Reason = u.Reason,
                            Status = u.Status,
                            Type = u.Type,
                            Detail = new RequestDetailDto
                            {
                                Id = t.Id,
                                DateAt = t.DateAt.ToString("yyyy-MM-dd"),
                                DateType = t.DateType,
                                Hour = t.Hour,
                                AbsenceTime = t.AbsenceTime,
                            }
                        };
            return await query.ToListAsync();
        }
        [AbpAuthorize(Ncc.Authorization.PermissionNames.MyAbsenceDay_View)]
        public List<GetListMyRequestByDayDto> GetAllMyRequestNew(DateTime? startDate, DateTime? endDate, RequestType? type)
        {
            var query = from u in WorkScope.GetAll<AbsenceDayRequest>()
                            .Where(s => !type.HasValue || s.Type == type)
                        join t in WorkScope.GetAll<AbsenceDayDetail>()
                            .Where(s => !startDate.HasValue || s.DateAt >= startDate)
                            .Where(s => !endDate.HasValue || s.DateAt <= endDate)
                            .Where(s => s.Request.UserId == AbpSession.UserId.Value)
                        on u.Id equals t.RequestId
                        select new
                        {
                            DateAt = t.DateAt,
                            Requests = new GetMyRequestDto
                            {
                                Id = u.Id,
                                DayOffName = u.DayOffType.Name,
                                FullName = u.User.FullName,
                                UserId = u.UserId,
                                Reason = u.Reason,
                                Status = u.Status,
                                Type = u.Type,
                                Detail = new RequestDetailDto
                                {
                                    Id = t.Id,
                                    DateAt = t.DateAt.ToString("yyyy-MM-dd"),
                                    DateType = t.DateType,
                                    Hour = t.Hour,
                                    AbsenceTime = t.AbsenceTime,
                                }
                            }
                        };

            var result = query.GroupBy(s => s.DateAt)
                              .Select(s => new GetListMyRequestByDayDto
                              {
                                  DateAt = s.Key,
                                  Requests = s.Select(x => x.Requests).ToList()
                              });

            return result.ToList();
        }
        [AbpAuthorize(Ncc.Authorization.PermissionNames.MyAbsenceDay_CancelRequest)]
        public async System.Threading.Tasks.Task CancelMyRequest(long requestId)
        {

            var requestDetail = await WorkScope.GetAll<AbsenceDayDetail>().Include(s => s.Request)
                                                     .Where(x => x.RequestId == requestId)
                                                     .FirstOrDefaultAsync();

            if (requestDetail?.DateAt.Date < DateTimeUtils.GetNow())
            {
                throw new UserFriendlyException("You can't cancel request in the PAST! Contact your PM to reject it");
            }

            if (requestDetail.Request.UserId != AbpSession.UserId.Value)
            {
                throw new UserFriendlyException("The request is not your");
            }

            requestDetail.Request.Status = RequestStatus.Rejected;
            await WorkScope.UpdateAsync(requestDetail.Request);

            await notifyKomuWhenCancelMyRequest(requestDetail);
        }

        private async System.Threading.Tasks.Task notifyKomuWhenCancelMyRequest(AbsenceDayDetail requestDetail)
        {
            var enableNotify = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.SendKomuRequest);
            if (enableNotify != "true")
            {
                Logger.Info("notifyKomuWhenApproveOrRejectRequest() SendKomuRequest=" + enableNotify + ", AbpSessionUserId=" + AbpSession.UserId);
                return;
            }
            var receivers = await getReceiverSendRequestList(requestDetail.Request.UserId);
            var offTypeName = "";
            if (requestDetail.Request.Type == RequestType.Off)
            {
                offTypeName = await WorkScope.GetAll<DayOffType>()
                    .Where(s => s.Id == requestDetail.Request.DayOffTypeId)
                    .Select(s => s.Name)
                    .FirstOrDefaultAsync();
            }
            var requester = await getNotifyUserInfoDto(AbpSession.UserId.Value);

            var alreadySentToPMIds = new HashSet<long>();

            foreach (var project in receivers)
            {
                if (!project.IsNotifyKomu)
                {
                    Logger.Info($"notifyKomuWhenCancelMyRequest(): {project.ProjectId}: IsNotifyKomu={project.IsNotifyKomu}, KomuChannelId={project.KomuChannelId}");
                }
                else
                {
                    var pmsTag = project.KomuPMsTag(alreadySentToPMIds);
                    pmsTag = string.IsNullOrEmpty(pmsTag) ? "" : $"PM {pmsTag}: ";

                    var komuMessage = $"{pmsTag}{requester.KomuAccountInfo} " +
                        $"has **cancelled** the request: **{CommonUtils.RequestTypeToString(requestDetail.Request.Type, offTypeName)}** " +
                        $"on {DateTimeUtils.ToString(requestDetail.DateAt)}";

                    _komuService.NotifyToChannel(komuMessage, project.KomuChannelId);
                }
                processAlreadySentToPMs(alreadySentToPMIds, project.PMs);
            }
        }

        private async Task<bool> CheckSessionUserIsPMOfUser(long userId)
        {
            var qprojectIdsOfSessionUser = WorkScope.GetAll<ProjectUser>()
                .Where(s => s.UserId == AbpSession.UserId.Value && s.Type == ProjectUserType.PM)
                .Where(s => s.Project.Status == ProjectStatus.Active)
                .Select(s => s.ProjectId);

            var qprojectIdsOfUser = WorkScope.GetAll<ProjectUser>()
                .Where(s => s.UserId == userId && s.Type != ProjectUserType.DeActive)
                .Where(s => s.Project.Status == ProjectStatus.Active)
                .Select(s => s.ProjectId);

            return await (from p in qprojectIdsOfSessionUser
                          join p2 in qprojectIdsOfUser on p equals p2
                          select p).AnyAsync();

        }
        [HttpPost]
        public async System.Threading.Tasks.Task ApproveRequest(long requestId)
        {
            var request = await WorkScope.GetAsync<AbsenceDayRequest>(requestId);
            if (!(await CheckSessionUserIsPMOfUser(request.UserId)))
            {
                throw new UserFriendlyException("You are not PM of UserId " + request.UserId);
            }

            request.Status = RequestStatus.Approved;
            await WorkScope.UpdateAsync<AbsenceDayRequest>(request);



            await notifyKomuWhenApproveOrRejectRequest(request, true);

        }

        [HttpPost]
        public async System.Threading.Tasks.Task RejectRequest(long requestId)
        {
            var request = await WorkScope.GetAsync<AbsenceDayRequest>(requestId);
            if (!(await CheckSessionUserIsPMOfUser(request.UserId)))
            {
                throw new UserFriendlyException("You are not PM of UserId " + request.UserId);
            }

            request.Status = RequestStatus.Rejected;
            await WorkScope.UpdateAsync<AbsenceDayRequest>(request);

            await notifyKomuWhenApproveOrRejectRequest(request, false);
            
        }

        private async System.Threading.Tasks.Task notifyKomuWhenApproveOrRejectRequest(AbsenceDayRequest request, bool isApprove)
        {
            var enableNotify = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.SendKomuRequest);
            if (enableNotify != "true")
            {
                Logger.Info("notifyKomuWhenApproveOrRejectRequest() SendKomuRequest=" + enableNotify + ", AbpSessionUserId=" + AbpSession.UserId);
                return;
            }
            var offTypeName = "";
            if (request.Type == RequestType.Off)
            {
                offTypeName = await WorkScope.GetAll<DayOffType>()
                    .Where(s => s.Id == request.DayOffTypeId)
                    .Select(s => s.Name)
                    .FirstOrDefaultAsync();
            }

            var approver = await getNotifyUserInfoDto(AbpSession.UserId.Value);
            var receivers = await getReceiverApproveRejectList(request.UserId);
            var requester = await getNotifyUserInfoDto(request.UserId);

            var requestDetail = await WorkScope.GetAll<AbsenceDayDetail>()
                .Where(s => s.RequestId == request.Id)
                .Select(s => new AbsenceDayDetailDto
                {
                    Id = s.Id,
                    RequestId = s.RequestId,
                    AbsenceTime = s.AbsenceTime,
                    DateAt = s.DateAt,
                    DateType = s.DateType,
                    Hour = s.Hour
                }).FirstOrDefaultAsync();

            var alreadySentToPMIds = new HashSet<long>();
            foreach (var project in receivers)
            {
                if (!project.IsNotifyKomu)
                {
                    Logger.Info($"notifyKomuWhenApproveRequest() projectId={project.ProjectId}: IsNotifyKomu={project.IsNotifyKomu}, KomuChannelId={project.KomuChannelId}");
                }
                else
                {
                    var pmsTag = project.KomuPMsTag(alreadySentToPMIds);
                    pmsTag = string.IsNullOrEmpty(pmsTag) ? "" : $"PM {pmsTag}:";

                    var komuMessage = $"{pmsTag} **{approver.FullName}** " +
                        $"has **{(isApprove ? "approved" : "rejected")}** the request: {requester.KomuAccountInfo} " +
                        $"**{GetRequestName(request, requestDetail, offTypeName)}** {requestDetail.ToKomuString()}";

                    _komuService.NotifyToChannel(komuMessage, project.KomuChannelId);
                    processAlreadySentToPMs(alreadySentToPMIds, project.PMs);
                }

            }
        }

        private string GetRequestName(AbsenceDayRequest request, AbsenceDayDetailDto absenceDayDetail, string offTypeName)
        {
            if (request.Type == RequestType.Off && absenceDayDetail.DateType == DayType.Custom)
            {
                return "Đi muộn/Về sớm";
            }
            return CommonUtils.RequestTypeToString(request.Type, offTypeName);
        }

        private void processAlreadySentToPMs(HashSet<long> alreadySentToPMIds, List<NotifyUserInfoDto> PMs)
        {
            foreach (var pm in PMs)
            {
                if (!alreadySentToPMIds.Contains(pm.UserId))
                {
                    alreadySentToPMIds.Add(pm.UserId);
                }
            }
        }

        [HttpGet]
        public async Task<List<GetMyRequestDto>> GetAllRequestByUserIdForTeamMember(DateTime? startDate, DateTime? endDate, int userId)
        {
            var query = from u in WorkScope.GetAll<AbsenceDayRequest>()
                        join t in WorkScope.GetAll<AbsenceDayDetail>()
                        .Where(s => !startDate.HasValue || s.DateAt >= startDate)
                        .Where(s => !endDate.HasValue || s.DateAt <= endDate)
                        .Where(s => s.Request.UserId == userId)
                        on u.Id equals t.RequestId
                        select new GetMyRequestDto
                        {
                            Id = u.Id,
                            DayOffName = u.DayOffType.Name,
                            FullName = u.User.FullName,
                            UserId = u.UserId,
                            Reason = u.Reason,
                            Status = u.Status,
                            Type = u.Type,
                            Detail = new RequestDetailDto
                            {
                                Id = t.Id,
                                DateAt = t.DateAt.ToString("yyyy-MM-dd"),
                                DateType = t.DateType,
                                Hour = t.Hour,
                                AbsenceTime = t.AbsenceTime
                            }
                        };
            return await query.ToListAsync();
        }

        private async System.Threading.Tasks.Task checkPMOfUser(long PMId, long userId)
        {
            var userInProjectIds = await WorkScope.GetAll<ProjectUser>()
                .Where(s => s.UserId == userId)
                .Select(s => s.ProjectId).Distinct()
                .ToListAsync();

            var pmInProjectIds = await WorkScope.GetAll<ProjectUser>()
                .Where(s => s.UserId == PMId && s.Type == ProjectUserType.PM)
                .Select(s => s.ProjectId)
                .Distinct()
                .ToListAsync();
            if (!pmInProjectIds.Intersect(userInProjectIds).Any())
            {
                throw new UserFriendlyException(string.Format("You aren't PM of this user"));
            }
        }

        [HttpGet]
        public async Task<List<GetMyRequestDto>> GetAllRequestByUserId(DateTime? startDate, DateTime? endDate, int userId)
        {
            await checkPMOfUser(AbpSession.UserId.Value, userId);
            return await GetAllRequestByUserIdForTeamMember(startDate, endDate, userId);
        }

        [HttpGet]
        public List<GetProjectAbsenceDayRequestDto> GetUserRequestByDate(DateTime dateAt, int userId)
        {
            var query = WorkScope.GetAll<AbsenceDayDetail>()
                  .Where(s => s.DateAt == dateAt)
                  .Where(s => s.Request.UserId == userId)
                  .Select(s => new GetProjectAbsenceDayRequestDto
                  {
                      Id = s.Request.Id,
                      UserId = s.Request.UserId,
                      AvatarPath = s.Request.User.AvatarPath,
                      Branch = s.Request.User.Branch != null ? s.Request.User.Branch.DisplayName : "",
                      Sex = s.Request.User.Sex,
                      FullName = s.Request.User.FullName,
                      Name = s.Request.User.Name,
                      //Level = s.Request.User.Level,
                      Type = s.Request.User.Type,
                      DateAt = s.DateAt,
                      DateType = s.DateType,
                      DayOffName = s.Request.DayOffType.Name,
                      Hour = s.Hour,
                      Reason = s.Request.Reason,
                      Status = s.Request.Status,
                      ShortName = s.Request.User.Name,
                      LeavedayType = s.Request.Type,
                      AbsenceTime = s.AbsenceTime
                  });
            return query.ToList();
        }

        public async Task<List<GetRequestOfUserDto>> GetAllRequestOfUserByDate(DateTime dateAt)
        {
            var query = WorkScope.All<AbsenceDayDetail>()
                  .Where(s => s.DateAt == dateAt)
                  .Where(s => s.Request.UserId == AbpSession.UserId.Value)
                  .Select(s => new GetRequestOfUserDto
                  {
                      Id = s.Request.Id,
                      UserId = s.Request.UserId,
                      DateAt = s.DateAt,
                      DateType = s.DateType,
                      DayOffName = s.Request.DayOffType.Name,
                      Hour = s.Hour,
                      Reason = s.Request.Reason,
                      Status = s.Request.Status,
                      LeavedayType = s.Request.Type,
                      AbsenceTime = s.AbsenceTime,
                  });

            return await query.ToListAsync();
        }
        [HttpDelete]
        public async System.Threading.Tasks.Task CancelRequest(EntityDto<long> input)
        {
            var absDetail = await WorkScope.GetAll<AbsenceDayDetail>()
                    .Where(s => s.RequestId == input.Id)
                      .Select(s=> new
                      {
                          s.Id,
                          s.RequestId
                      }).FirstOrDefaultAsync();

            if (absDetail != null)
            {
                await WorkScope.GetRepo<AbsenceDayRequest>().DeleteAsync(absDetail.RequestId);
                await WorkScope.GetRepo<AbsenceDayDetail>().DeleteAsync(absDetail.Id);
            }
        }

    }

}


