using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ncc.Authorization.Roles;
using Ncc.Authorization.Users;
using Ncc.IoC;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Tasks;
using Timesheet.APIs.HRMv2;
using Timesheet.APIs.HRMv2.Dto;
using Timesheet.APIs.OverTimeHours;
using Timesheet.DomainServices;
using Timesheet.DomainServices.Dto;
using Timesheet.Entities;
using Timesheet.Services.HRMv2;
using Timesheet.Services.Komu;
using Timesheet.Services.Project.Dto;
using Xunit;
using static Ncc.Entities.Enum.StatusEnum;

namespace Timesheet.Application.Tests.API.HRMV2
{
    /// <summary>
    /// 14/20 functions (6 functions call HRMv2 API - can't test)
    /// 21/21 test cases passed
    /// update day 17/01/2023
    /// </summary>

    public class HRMv2AppService_Test : TimesheetApplicationTestBase
    {
        private readonly HRMv2AppService _appService;
        private readonly IWorkScope _workScope;

        private List<MyTimesheet> listMyTimesheet = new List<MyTimesheet>
        {
            new MyTimesheet
            {
                Id=101,
                ProjectTaskId = 14,
                Note="",
                WorkingTime=480,
                TargetUserWorkingTime=0,
                TypeOfWork=TypeOfWork.OverTime,
                IsCharged=false,
                DateAt=new DateTime(2022,12,26),
                Status=TimesheetStatus.Approve,
                ProjectTargetUserId=1,
                IsTemp=false,
                UserId=9,
            },
            new MyTimesheet
            {
                Id=102,
                ProjectTaskId = 14,
                Note="",
                WorkingTime=480,
                TargetUserWorkingTime=0,
                TypeOfWork=TypeOfWork.OverTime,
                IsCharged=false,
                DateAt=new DateTime(2022,12,28),
                Status=TimesheetStatus.Approve,
                ProjectTargetUserId=1,
                IsTemp=false,
                UserId=9,
            },
            new MyTimesheet
            {
                Id=105,
                ProjectTaskId = 14,
                Note="",
                WorkingTime=240,
                TargetUserWorkingTime=0,
                TypeOfWork=TypeOfWork.OverTime,
                IsCharged=false,
                DateAt=new DateTime(2022,12,31),
                Status=TimesheetStatus.Approve,
                ProjectTargetUserId=1,
                IsTemp=false,
                UserId=1,
            },
            new MyTimesheet
            {
                Id=106,
                ProjectTaskId = 14,
                Note="",
                WorkingTime=240,
                TargetUserWorkingTime=0,
                TypeOfWork=TypeOfWork.OverTime,
                IsCharged=false,
                DateAt=new DateTime(2022,12,24),
                Status=TimesheetStatus.Approve,
                ProjectTargetUserId=1,
                IsTemp=false,
                UserId=1,
            },
            new MyTimesheet
            {
                Id=107,
                ProjectTaskId = 14,
                Note="",
                WorkingTime=480,
                TargetUserWorkingTime=0,
                TypeOfWork=TypeOfWork.OverTime,
                IsCharged=false,
                DateAt=new DateTime(2022,12,19),
                Status=TimesheetStatus.Approve,
                ProjectTargetUserId=1,
                IsTemp=false,
                UserId=1,
            }
        };

        public HRMv2AppService_Test()
        {
            var _overTimeHourAppService = Resolve<OverTimeHourAppService>();

            var httpClient = Resolve<HttpClient>();
            var configuration = Substitute.For<IConfiguration>();
            configuration.GetValue<string>("HRMv2Service:BaseAddress").Returns("http://www.myserver.com");
            configuration.GetValue<string>("HRMv2Service:SecurityCode").Returns("secretCode");
            var logger = Resolve<ILogger<HRMv2Service>>();
            var _HRMv2Service = Substitute.For<HRMv2Service>(httpClient, configuration, logger);

            var userManager = Resolve<UserManager>();

            var loggerKomu = Resolve<ILogger<KomuService>>();
            configuration.GetValue<string>("KomuService:BaseAddress").Returns("http://www.myserver.com");
            configuration.GetValue<string>("KomuService:SecurityCode").Returns("secretCode");
            configuration.GetValue<string>("KomuService:DevModeChannelId").Returns("_channelIdDevMode");
            configuration.GetValue<string>("KomuService:EnableKomuNotification").Returns("_isNotifyToKomu");
            var komuService = Substitute.For<KomuService>(httpClient, loggerKomu, configuration);
            var abpSession = Resolve<IAbpSession>();
            var roleManager = Resolve<RoleManager>();
            var objectMapper = Resolve<IObjectMapper>();
            _workScope = Resolve<IWorkScope>();

            var _userServices = Substitute.For<UserServices>(userManager, komuService, abpSession, roleManager, objectMapper, _workScope);
            _userServices.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();

            _appService = new HRMv2AppService(
                    _overTimeHourAppService,
                    _HRMv2Service,
                    _userServices,
                    _workScope
                )
            {
                UnitOfWorkManager = Resolve<IUnitOfWorkManager>()
            };
            foreach (var ts in listMyTimesheet)
            {
                _workScope.InsertAsync(ts);
            }
        }

        [Fact]
        public async Task UpdateAvatarFromHrm_Test()
        {
            UpdateAvatarDto input = new UpdateAvatarDto
            {
                AvatarPath = "abc/abc.jpg",
                EmailAddress = "toai.nguyencong@ncc.asia"
            };

            WithUnitOfWork(() =>
            {
                _appService.UpdateAvatarFromHrm(input);
            });

            WithUnitOfWork(() =>
            {
                var newUserAvtPath = _workScope.GetAll<User>().ToList().Where(u => u.EmailAddress == input.EmailAddress).FirstOrDefault();

                Assert.Equal(newUserAvtPath.AvatarPath, input.AvatarPath);
            });
        }

        [Fact]
        public async Task UpdateAvatarFromHrm_Should_Not_Update_AvartarPath_Not_Exits()
        {
            UpdateAvatarDto input = new UpdateAvatarDto
            {
                AvatarPath = "",
            };

            WithUnitOfWork(() =>
            {
                _appService.UpdateAvatarFromHrm(input);
            });

            WithUnitOfWork(() =>
            {
                var newUserAvtPath = _workScope.GetAll<User>().ToList().Where(u => u.EmailAddress == input.EmailAddress).FirstOrDefault();
                newUserAvtPath.ShouldBeNull();
            });
        }

        [Fact]
        public async Task UpdateAvatarFromHrm_Should_Not_Update_By_EmailAddress_Not_Exits()
        {
            UpdateAvatarDto input = new UpdateAvatarDto
            {
                EmailAddress = ""
            };

            WithUnitOfWork(() =>
            {
                _appService.UpdateAvatarFromHrm(input);
            });

            WithUnitOfWork(() =>
            {
                var newUserAvtPath = _workScope.GetAll<User>().ToList().Where(u => u.EmailAddress == input.EmailAddress).FirstOrDefault();
                newUserAvtPath.ShouldBeNull();
            });
        }

        [Fact]
        public async Task GetPunishmentBasicUserUnlockTS_Test()
        {
            int expectTotalCount = 4;

            await WithUnitOfWorkAsync(async () =>
            {
                //LockUnlockTimesheetType = MyTimesheet;
                var result = await _appService.GetPunishmentBasicUserUnlockTS(2022, 12);
                Assert.Equal(expectTotalCount, result.Count());

                result.Last().Email.ShouldBe("thanh.trantien@ncc.asia");
                result.Last().Money.ShouldBe(120000);
            });
        }

        [Fact]
        public async Task GetPunishmentPMUnlockTS_Test()
        {
            int expectTotalCount = 6;

            await WithUnitOfWorkAsync(async () =>
            {
                //LockUnlockTimesheetType = ApproveRejectTimesheet;
                var result = await _appService.GetPunishmentPMUnlockTS(2022, 12);

                result.Last().Email.ShouldBe("duong.tranduc@ncc.asia");
                result.Last().Money.ShouldBe(140000);
            });
        }

        [Fact]
        public async Task GetPunishtmentCheckin_Test()
        {
            var expectTotalCount = 1;
            var all1 = new List<Timekeeping>();
            var all2 = new List<Timekeeping>();

            WithUnitOfWork(() =>
           {
               _workScope.Insert<Timekeeping>(new Timekeeping
               {
                   UserId = 22,
                   UserEmail = "hien.ngothu@ncc.asia",
                   CheckIn = "10:00",
                   CheckOut = "17:00",
                   DateAt = DateTime.Parse("2022-12-20"),
                   IsLocked = false,
                   IsDeleted = false,
                   StatusPunish = CheckInCheckOutPunishmentType.Late,
                   MoneyPunish = 20000
               });
           });

            WithUnitOfWork(() =>
            {
                var result = _appService.GetPunishtmentCheckin(2022, 12);

                Assert.Equal(expectTotalCount, result.Count());
                result.Last().Email.ShouldBe("hien.ngothu@ncc.asia");
                result.Last().Money.ShouldBe(20000);
            });
        }

        [Fact]
        public async Task GetAllRequestDay_Test()
        {
            InputCollectDataForPayslipDto input = new InputCollectDataForPayslipDto
            {
                Year = 2022,
                Month = 12,
                UpperEmails = new List<string> { "HIEU.TRANTUNG@NCC.ASIA", "TRANG.VUQUYNH@NCC.ASIA" }
            };

            WithUnitOfWork(() =>
            {
                var result = _appService.GetAllRequestDay(input);
                Assert.Single(result);

                result.First().NormalizedEmailAddress.ShouldBe("TRANG.VUQUYNH@NCC.ASIA");
                result.First().OffDates.Count().ShouldBe(3);
                result.First().OffDates.First().DateAt.Date.ShouldBe(new DateTime(2022,12,6).Date);
                result.First().OffDates.First().DayOffTypeId.ShouldBe(2);
                result.First().OffDates.First().DayValue.ShouldBe(1);
                result.First().OffDates.First().LeaveDay.ShouldBe(3);
                result.First().WorkAtHomeOnlyDates.Count().ShouldBe(3);
                result.First().WorkAtHomeOnlyDates.First().Date.ShouldBe(new DateTime(2022, 12, 6).Date);
            });
        }

        [Fact]
        public async Task GetChamCongInfo_Test()
        {
            InputCollectDataForPayslipDto input = new InputCollectDataForPayslipDto
            {
                Year = 2022,
                Month = 12,
                UpperEmails = new List<string> { "HIEU.TRANTUNG@NCC.ASIA", "TRANG.VUQUYNH@NCC.ASIA" }
            };

            WithUnitOfWork(() =>
            {
                var result = _appService.GetChamCongInfo(input);

                result.First().NormalizeEmailAddress.ShouldBe("TRANG.VUQUYNH@NCC.ASIA");
                result.First().NormalWorkingDates.Count().ShouldBe(13);
                result.First().OpenTalkDates.Count().ShouldBe(0);
                result.First().NormalWorkingDates.First().Date.ShouldBe(new DateTime(2022,12,26).Date);
            });
        }

        [Fact]
        public async Task GetSettingOffDates_Test()
        {

            WithUnitOfWork(() =>
            {
                var result = _appService.GetSettingOffDates(2022, 12);

                Assert.Equal(4, result.Count());
                result.First().Date.ShouldBe(new DateTime(2022, 12, 4).Date);
            });
        }

        [Fact]
        public async Task GetOTTimesheets_Test()
        {
            var expectTotalCount = 2;
            var input = new InputCollectDataForPayslipDto
            {
                Month = 12,
                Year = 2022,
                UpperEmails = new List<String> { "ADMIN@ASPNETBOILERPLATE.COM", "LINH.NGUYENTHUY@NCC.ASIA" }
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _appService.GetOTTimesheets(input);
                Assert.Equal(expectTotalCount, result.Count());

                result.First().ListOverTimeHour.Count().ShouldBe(2);
                result.Last().ListOverTimeHour.Count().ShouldBe(3);
                result.Last().ListOverTimeHour.Last().Date.ShouldBe(new DateTime(2022, 12, 19).Date);
                result.Last().ListOverTimeHour.Last().OTHour.ShouldBe(8);
            });
        }

        [Fact]
        public async Task CreateUser_Test()
        {
            var expectId = 0L;
            var expectTotalCount = 0;
            var expectUser = new CreateUpdateByHRMV2Dto
            {
                Name = "Test",
                Surname = "Test",
                EmailAddress = "Test@ncc.asia",
                Sex = Sex.Male,
                WorkingStartDate = DateTime.Now,
                PositionCode = "IT",
                BranchCode = "HN3",
                LevelCode = "Intern_3",
            };

            await WithUnitOfWorkAsync(async () =>
            {
                expectTotalCount = _workScope.GetAll<User>().Count();

                expectId = await _appService.CreateUser(expectUser);
                expectId.ShouldNotBeNull();
            });

            WithUnitOfWork(() =>
            {
                var allUsers = _workScope.GetAll<User>();
                Assert.Equal(expectTotalCount + 1, allUsers.Count());
                var createdUser = allUsers.Where(x => x.Id == expectId).FirstOrDefault();
                createdUser.EmailAddress.ShouldBe(expectUser.EmailAddress);
                createdUser.Name.ShouldBe(expectUser.Name);
                createdUser.Surname.ShouldBe(expectUser.Surname);
                createdUser.Sex.ShouldBe(expectUser.Sex);
                createdUser.Type.ShouldBe(Usertype.Internship);
            });
        }

        [Fact]
        public async Task CreateUser_Should_Not_Create_User_With_EmailAddress_Exist()
        {
            var expectUser = new CreateUpdateByHRMV2Dto
            {
                Name = "Test",
                Surname = "Test",
                EmailAddress = "toai.nguyencong@ncc.asia",
                Sex = Sex.Male,
                Type = Usertype.Staff,
                WorkingStartDate = DateTime.Now,
                PositionCode = "IT",
                BranchCode = "HN3",
                LevelCode = "Intern_3",
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    var expectId = await _appService.CreateUser(expectUser);
                });
                Assert.Equal($"failed to create user from HRM, user with email {expectUser.EmailAddress} is already exist", exception.Message);
            });
        }

        [Fact]
        public async Task UpdateUser_Test()
        {
            var expectUser = new CreateUpdateByHRMV2Dto
            {
                Name = "Test",
                Surname = "Test",
                EmailAddress = "toai.nguyencong@ncc.asia",
                PositionCode = "IT",
                BranchCode = "HN3",
                LevelCode = "Intern_3",
            };

            await WithUnitOfWorkAsync(async () =>
            {
                await _appService.UpdateUser(expectUser);
            });

            WithUnitOfWork(() =>
            {
                var createdUser = _workScope.GetAll<User>().Where(x => x.EmailAddress == expectUser.EmailAddress).FirstOrDefault();
                createdUser.EmailAddress.ShouldBe(expectUser.EmailAddress);
                createdUser.Name.ShouldBe(expectUser.Name);
                createdUser.Surname.ShouldBe(expectUser.Surname);
                createdUser.Sex.ShouldBe(expectUser.Sex);
            });
        }

        [Fact]
        public async Task ConfirmUserQuit_Test()
        {
            var input = new UpdateUserStatusFromHRMDto
            {
                EmailAddress = "hien.ngothu@ncc.asia",
                DateAt = DateTime.Now,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _appService.ConfirmUserQuit(input);
                result.ToString().ShouldBe(input.ToString());

                var updatedUser = _workScope.GetAll<User>()
                .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim()).First();
                updatedUser.IsActive.ShouldBeFalse();
                updatedUser.IsStopWork.ShouldBeTrue();
            });
        }

        [Fact]
        public async Task ConfirmUserQuit_Should_Throw_Exception()
        {
            var input = new UpdateUserStatusFromHRMDto
            {
                EmailAddress = "toai.nguyencon@ncc.asia",
                DateAt = DateTime.Now,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _appService.ConfirmUserQuit(input);
                });
                Assert.Equal("Can't found user with the same email with HRM Tool", exception.Message);
            });
        }

        [Fact]
        public async Task ConfirmUserPause_Test()
        {
            var input = new UpdateUserStatusFromHRMDto
            {
                EmailAddress = "hien.ngothu@ncc.asia",
                DateAt = DateTime.Now,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _appService.ConfirmUserPause(input);
                result.ToString().ShouldBe(input.ToString());

                var updatedUser = _workScope.GetAll<User>()
                .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim()).First();
                updatedUser.IsActive.ShouldBeTrue();
                updatedUser.IsStopWork.ShouldBeTrue();
            });
        }

        [Fact]
        public async Task ConfirmUserPause_Should_Throw_Exception()
        {
            var input = new UpdateUserStatusFromHRMDto
            {
                EmailAddress = "toai.nguyencon@ncc.asia",
                DateAt = DateTime.Now,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _appService.ConfirmUserPause(input);
                });
                Assert.Equal("Can't found user with the same email with HRM Tool", exception.Message);
            });
        }

        [Fact]
        public async Task ConfirmUserMaternityLeave_Test()
        {
            var input = new UpdateUserStatusFromHRMDto
            {
                EmailAddress = "hien.ngothu@ncc.asia",
                DateAt = DateTime.Now,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _appService.ConfirmUserMaternityLeave(input);
                result.ToString().ShouldBe(input.ToString());

                var updatedUser = _workScope.GetAll<User>()
                .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim()).First();
                updatedUser.IsActive.ShouldBeTrue();
                updatedUser.IsStopWork.ShouldBeTrue();
            });
        }

        [Fact]
        public async Task ConfirmUserMaternityLeave_Should_Throw_Exception()
        {
            var input = new UpdateUserStatusFromHRMDto
            {
                EmailAddress = "toai.nguyencon@ncc.asia",
                DateAt = DateTime.Now,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _appService.ConfirmUserMaternityLeave(input);
                });
                Assert.Equal("Can't found user with the same email with HRM Tool", exception.Message);
            });
        }

        [Fact]
        public async Task ConfirmUserBackToWork_Test()
        {
            var input = new UpdateUserStatusFromHRMDto
            {
                EmailAddress = "hien.ngothu@ncc.asia",
                DateAt = DateTime.Now,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _appService.ConfirmUserBackToWork(input);
                result.ToString().ShouldBe(input.ToString());

                var updatedUser = _workScope.GetAll<User>()
                .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim()).First();
                updatedUser.IsActive.ShouldBeTrue();
                updatedUser.IsStopWork.ShouldBeFalse();
            });
        }

        [Fact]
        public async Task ConfirmUserBackToWork_Should_Throw_Exception()
        {
            var input = new UpdateUserStatusFromHRMDto
            {
                EmailAddress = "toai.nguyencon@ncc.asia",
                DateAt = DateTime.Now,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _appService.ConfirmUserBackToWork(input);
                });
                Assert.Equal("Can't found user with the same email with HRM Tool", exception.Message);
            });
        }
    }
}
