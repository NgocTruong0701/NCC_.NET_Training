﻿using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Amazon.Runtime.Internal.Transform;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ncc.Configuration;
using Ncc.IoC;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Timesheet.APIs.MyAbsenceDays.Dto;
using Timesheet.APIs.RequestDays;
using Timesheet.APIs.RequestDays.Dto;
using Timesheet.DomainServices;
using Timesheet.Entities;
using Timesheet.Services.FaceIdService;
using Timesheet.Services.Komu;
using Timesheet.Services.Tracker;
using static Ncc.Entities.Enum.StatusEnum;

namespace Timesheet.Application.Tests.API.RequestDays
{
    public class RequestDayAppServiceTestBase : TimesheetApplicationTestBase
    {
        protected readonly IWorkScope workScope;
        public RequestDayAppServiceTestBase() 
        {
            workScope = Resolve<IWorkScope>();
        }

        public RequestDayAppService InstanceRequestDayAppService() 
        {
            var komuServiceConfigOptions = new Dictionary<string, string>
                {
                    {"KomuService:ChannelIdDevMode", ""},
                    {"KomuService:EnableKomuNotification", "true"},
                    {"KomuService:BaseAddress", "http://www.myserver.com"},
                    {"KomuService:SecurityCode", "secretCode"}
                };

            var komuServiceConfiguration = new ConfigurationBuilder()
              .AddInMemoryCollection(komuServiceConfigOptions)
              .Build();
            
            var trackedServiceConfigOptions = new Dictionary<string, string>
                {
                    {"TrackerService:BaseAddress", "http://tracker.komu.vn:5600"},
                    {"TrackerService:SecurityCode" , "1BCD4F3799EE95C4"}
                };

            var trackerServiceConfiguration = new ConfigurationBuilder()
              .AddInMemoryCollection(trackedServiceConfigOptions)
              .Build();

            var faceIdServiceConfigOptions = new Dictionary<string, string>
                {
                    {"FaceIdService:BaseAddress","https://checkin.nccsoft.vn/v1/"},
                    {"FaceIdService:SecurityCode","GHstHchTdpn9L83e"},
                    {"FaceIdService:PathImage", "https://checkin.nccsoft.vn/v1/upload-image?pathValue="},
                };
            var faceIdServiceConfiguration = new ConfigurationBuilder()
              .AddInMemoryCollection(faceIdServiceConfigOptions)
              .Build();

            var httpClient = Resolve<HttpClient>();
            var httpClientFaceId = Resolve<HttpClient>();
            var httpClientTrackerService = Resolve<HttpClient>();

            var komuServiceLogger = Resolve<ILogger<KomuService>>();
            var trackerServiceLogger = Resolve<ILogger<TrackerService>>();
            var faceIdServiceLogger = Resolve<ILogger<FaceIdService>>();

            var backgroundJobManager = Resolve<IBackgroundJobManager>();

            var komuService = Substitute.For<KomuService>(
                httpClient,
                komuServiceLogger,
                komuServiceConfiguration);

            var trackerService = Substitute.For<TrackerService>(
                httpClientTrackerService,
                trackerServiceConfiguration,
                trackerServiceLogger);

            var settingManager = Substitute.For<ISettingManager>();
            settingManager.GetSettingValueAsync(AppSettingNames.CheckInInternalAccount).Returns(Task.FromResult("dd0f2097-ad1a-4575-be15-a8bba7b559f2"));
            settingManager.GetSettingValueAsync(AppSettingNames.CheckInInternalUrl).Returns(Task.FromResult("https://checkin.nccsoft.vn/"));
            settingManager.GetSettingValueAsync(AppSettingNames.CheckInInternalXSecretKey).Returns(Task.FromResult("9fqKUaGGF9vLvcCj"));
            settingManager.GetSettingValueAsync(AppSettingNames.LimitedMinutes).Returns(Task.FromResult("15"));
            settingManager.GetSettingValueAsync(AppSettingNames.CheckInCheckOutPunishmentSetting).Returns(Task.FromResult("[{\"id\":0,\"name\":\"Không bị phạt\",\"note\":\"Nhân viên check in và check out đúng\",\"money\":0},{\"id\":1,\"name\":\"Đi muộn\",\"note\":\"Nhân viên đi muộn có check in và không check out(tracker đủ) hoặc có check out\",\"money\":20000},{\"id\":2,\"name\":\"Không CheckIn\",\"note\":\"Nhân viên không check in, không check out(tracker đủ)\",\"money\":30000},{\"id\":3,\"name\":\"Không CheckOut\",\"note\":\"Nhân viên check in đúng giờ và không checkout(tracker không đủ)\",\"money\":30000},{\"id\":4,\"name\":\"Đi muộn và Không CheckOut\",\"note\":\"Nhân viên có check in muộn và không có check out(tracker không đủ)\",\"money\":50000},{\"id\":5,\"name\":\"Không CheckIn và không CheckOut\",\"note\":\"Nhân viên không check in và không check out(tracker không đủ)\",\"money\":100000}]"));
            settingManager.GetSettingValueForApplicationAsync(AppSettingNames.TotalTimeAbsenceTime).Returns(Task.FromResult("2"));
            var faceIdService = Substitute.For<FaceIdService>(
               httpClientFaceId,
               faceIdServiceLogger,
               settingManager,
               workScope);

            var timeKeepingService = Substitute.For<TimekeepingServices>(
                komuService,
                trackerService,
                workScope,
                faceIdService);

            var requestDayAppService = new RequestDayAppService(
                backgroundJobManager,
                komuService,
                timeKeepingService,
                workScope);
            requestDayAppService.AbpSession = Resolve<IAbpSession>();
            requestDayAppService.SettingManager = settingManager;
            requestDayAppService.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();

            return requestDayAppService;
        }

        public InputRequestDto InputRequestDto()
        {
            return new InputRequestDto
            {
                startDate = new DateTime(2022, 1, 1),
                endDate = new DateTime(2023, 1, 1),
                projectIds = new List<long> { 1, 5, 6 },
                dayOffTypeId = 1,
                status = 2,
                type = RequestType.Off
            };
        }

        public AbsenceDayDetailDto AbsenceDayDetailDto()
        {
            return new AbsenceDayDetailDto
            {
                Id = 1,
                DateAt = new DateTime(2022, 1, 1),
                DateType = DayType.Fullday,
                AbsenceTime = OnDayType.DiMuon,
                Status = RequestStatus.Approved
            };
        }

        public MyRequestDto MyRequestDto()
        {
            var absenceDayDetailDto = AbsenceDayDetailDto();
            var listAbsenceDayDetailDto = new List<AbsenceDayDetailDto> { absenceDayDetailDto };

            return new MyRequestDto 
            {
                DayOffTypeId = 1,
                Status = RequestStatus.Approved,
                Reason = "Xin off",
                Type= RequestType.Off,
                Absences= listAbsenceDayDetailDto,
            };
        }

        public AbsenceDayDetail AbsenceDayDetail()
        {
            return new AbsenceDayDetail
            {
                Id = 99,
                DateAt = new DateTime(2022, 6, 1),
                DateType = DayType.Fullday,
                AbsenceTime = OnDayType.DiMuon,
                RequestId = 101
            };
        }

        public AbsenceDayRequest AbsenceDayRequest()
        {
            return new AbsenceDayRequest
            {
                Id = 101,
                UserId = 1,
                Type = RequestType.Off,
            };
        }

        public DayOffSetting DayOffSetting()
        {
            return new DayOffSetting
            {
                Id = 101,
                DayOff = new DateTime(2022, 1, 1)
            };
        }
    }
}
