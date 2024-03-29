﻿using Abp.Configuration;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.UI;
using Amazon.Util.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ncc.Configuration;
using Ncc.IoC;
using NSubstitute;
using Shouldly;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Timesheet.APIs.Timekeepings;
using Timesheet.APIs.Timekeepings.Dto;
using Timesheet.Constants;
using Timesheet.DomainServices;
using Timesheet.Entities;
using Timesheet.Paging;
using Timesheet.Services.FaceIdService;
using Timesheet.Services.Komu;
using Timesheet.Services.Tracker;
using Xunit;
using static Ncc.Entities.Enum.StatusEnum;

namespace Timesheet.Application.Tests.API.Timekeepings
{
    /// <summary>
    /// 8/9 functions
    /// 20/21 test cases passed
    /// update day 11/01/2023
    /// </summary>

    public class TimekeepingAppService_Tests : TimesheetApplicationTestBase
    {
        private readonly TimekeepingAppService _timekeeping;
        private readonly TimekeepingServices _timekeepingServices;
        private readonly IWorkScope _workScope;

        public TimekeepingAppService_Tests()
        {
            var httpClient = Resolve<HttpClient>();
            var httpClientFaceId = Resolve<HttpClient>();
            var httpClientTrackerService = Resolve<HttpClient>();
            var configuration = Substitute.For<IConfiguration>();
            configuration.GetValue<string>("TrackerService:BaseAddress").Returns("http://tracker.komu.vn:5600");
            configuration.GetValue<string>("TrackerService:SecurityCode").Returns("1BCD4F3799EE95C4");
            var logger = Resolve<ILogger<TrackerService>>();
            var trackerService = Substitute.For<TrackerService>(httpClientTrackerService, configuration, logger);

            var loggerKomu = Resolve<ILogger<KomuService>>();
            configuration.GetValue<string>("KomuService:BaseAddress").Returns("http://www.myserver.com");
            configuration.GetValue<string>("KomuService:SecurityCode").Returns("secretCode");
            configuration.GetValue<string>("KomuService:DevModeChannelId").Returns("_channelIdDevMode");
            configuration.GetValue<string>("KomuService:EnableKomuNotification").Returns("_isNotifyToKomu");
            var komuService = Substitute.For<KomuService>(httpClient, loggerKomu, configuration);


            var settingManager = Substitute.For<ISettingManager>();
            _workScope = Resolve<IWorkScope>();
            var loggerFaceIdService = Resolve<ILogger<FaceIdService>>();
            settingManager.GetSettingValueAsync(AppSettingNames.CheckInInternalAccount).Returns(Task.FromResult("dd0f2097-ad1a-4575-be15-a8bba7b559f2"));
            settingManager.GetSettingValueAsync(AppSettingNames.CheckInInternalUrl).Returns(Task.FromResult("https://checkin.nccsoft.vn/"));
            settingManager.GetSettingValueAsync(AppSettingNames.CheckInInternalXSecretKey).Returns(Task.FromResult("9fqKUaGGF9vLvcCj"));
            settingManager.GetSettingValueAsync(AppSettingNames.LimitedMinutes).Returns(Task.FromResult("15"));
            settingManager.GetSettingValueAsync(AppSettingNames.CheckInCheckOutPunishmentSetting).Returns(Task.FromResult("[{\"id\":0,\"name\":\"Không bị phạt\",\"note\":\"Nhân viên check in và check out đúng\",\"money\":0},{\"id\":1,\"name\":\"Đi muộn\",\"note\":\"Nhân viên đi muộn có check in và không check out(tracker đủ) hoặc có check out\",\"money\":20000},{\"id\":2,\"name\":\"Không CheckIn\",\"note\":\"Nhân viên không check in, không check out(tracker đủ)\",\"money\":30000},{\"id\":3,\"name\":\"Không CheckOut\",\"note\":\"Nhân viên check in đúng giờ và không checkout(tracker không đủ)\",\"money\":30000},{\"id\":4,\"name\":\"Đi muộn và Không CheckOut\",\"note\":\"Nhân viên có check in muộn và không có check out(tracker không đủ)\",\"money\":50000},{\"id\":5,\"name\":\"Không CheckIn và không CheckOut\",\"note\":\"Nhân viên không check in và không check out(tracker không đủ)\",\"money\":100000}]"));

            configuration.GetValue<string>("FaceIdService:BaseAddress").Returns("https://checkin.nccsoft.vn/v1/");
            configuration.GetValue<string>("FaceIdService:SecurityCode").Returns("GHstHchTdpn9L83e");
            configuration.GetValue<string>("FaceIdService:PathImage").Returns("https://checkin.nccsoft.vn/v1/upload-image?pathValue=");
            

            var faceIdService = Substitute.For<FaceIdService>(httpClientFaceId, loggerFaceIdService, settingManager, _workScope);
            _timekeepingServices = Substitute.For<TimekeepingServices>(komuService, trackerService, _workScope, faceIdService);
            _timekeepingServices.SettingManager = settingManager;
            _timekeepingServices.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();

            _timekeeping = new TimekeepingAppService(_timekeepingServices, _workScope)
            {
                ObjectMapper = Resolve<IObjectMapper>(),
                AbpSession = Resolve<IAbpSession>(),
                SettingManager = settingManager,
                UnitOfWorkManager = Resolve<IUnitOfWorkManager>(),
            };
        }

        [Fact]
        public async Task GetAllPagging_Test1()
        {
            var expectTotalCount = 1;
            var expectItemCount = 1;

            //Total < max
            var gridParam = new GridParam
            {
                MaxResultCount = 10,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.GetAllPagging(gridParam, 2022, 12, 1, 1);
                Assert.Equal(expectTotalCount, result.TotalCount);
                Assert.Equal(expectItemCount, result.Items.Count);

                result.Items.First().UserId.ShouldBe(1);
                result.Items.First().UserName.ShouldBe("admin admin");
                result.Items.First().UserEmail.ShouldBe("admin@aspnetboilerplate.com");
            });
        }

        [Fact]
        public async Task GetAllPagging_Test2()
        {
            var expectTotalCount = 1;
            var expectItemCount = 1;

            //Total < max, skip < total
            var gridParam = new GridParam
            {
                MaxResultCount = 10,
                SkipCount = 0,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.GetAllPagging(gridParam, 2022, 12, 1, 1);
                Assert.Equal(expectTotalCount, result.TotalCount);
                Assert.Equal(expectItemCount, result.Items.Count);

                result.Items.First().UserId.ShouldBe(1);
                result.Items.First().UserName.ShouldBe("admin admin");
                result.Items.First().UserEmail.ShouldBe("admin@aspnetboilerplate.com");
            });
        }

        [Fact]
        public async Task GetAllPagging_Test3()
        {
            var expectTotalCount = 1;
            var expectItemCount = 0;

            //Total < max, skip > total
            var gridParam = new GridParam
            {
                MaxResultCount = 10,
                SkipCount = 10,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.GetAllPagging(gridParam, 2022, 12, 1, 1);
                Assert.Equal(expectTotalCount, result.TotalCount);
                Assert.Equal(expectItemCount, result.Items.Count);
            });
        }

        [Fact]
        public async Task GetAllPagging_Test4()
        {
            var expectTotalCount = 1;
            var expectItemCount = 0;

            //Total > max
            var gridParam = new GridParam
            {
                MaxResultCount = 0,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.GetAllPagging(gridParam, 2022, 12, 1, 1);
                Assert.Equal(expectTotalCount, result.TotalCount);
                Assert.Equal(expectItemCount, result.Items.Count);
            });
        }

        [Fact]
        public async Task GetDetailTimekeeping_Test1()
        {
            var expectTotalCount = 1;

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.GetDetailTimekeeping(2022, 12, 26, 4, 2, false, false, CheckInCheckOutPunishmentType.NoPunish);
                Assert.Equal(expectTotalCount, result.Count);

                result.Last().TimekeepingId.ShouldBe(22);
                result.Last().UserId.ShouldBe(4);
                result.Last().UserName.ShouldBe("Toại Nguyễn Công");
                result.Last().UserEmail.ShouldBe("toai.nguyencong@ncc.asia");
                result.Last().CheckIn.ShouldBe("10:18");
                result.Last().CheckOut.ShouldBe("20:07");
                result.Last().ResultCheckIn.ShouldBe(0);
                result.Last().ResultCheckOut.ShouldBe(0);
                result.Last().Date.ShouldBe(new DateTime(2022,12,26).Date);
                result.Last().Status.ShouldBe(PunishmentStatus.Normal);
                result.Last().StatusPunish.ShouldBe(CheckInCheckOutPunishmentType.NoPunish);
                result.Last().MoneyPunish.ShouldBe(0);
                result.Last().NoteReply.ShouldBe("Email not match");
            });
        }

        [Fact]
        public async Task GetDetailTimekeeping_Test2()
        {
            var expectTotalCount = 0;

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.GetDetailTimekeeping(3077, 12, 26, 4, 2, false, false, CheckInCheckOutPunishmentType.NoPunish);
                Assert.Equal(expectTotalCount, result.Count);
            });
        }

        [Fact]
        public async Task GetMyDetails_Test1()
        {
            var expectTotalCount = 1;

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.GetMyDetails(2022, 12);
                Assert.Equal(expectTotalCount, result.Count);

                result.Last().TimekeepingId.ShouldBe(1);
                result.Last().UserId.ShouldBe(1);
                result.Last().UserName.ShouldBe("admin admin");
                result.Last().UserEmail.ShouldBe("admin@aspnetboilerplate.com");
                result.Last().Date.ShouldBe(new DateTime(2022,12,26).Date);
                result.Last().Status.ShouldBe(PunishmentStatus.Normal);
                result.Last().UserNote.ShouldBeNullOrEmpty();
            });
        }

        [Fact]
        public async Task UpdateTimekeeping_Should_Update_Valid_Timekeeping()
        {
            var timekeeping = new Timekeeping
            {
                Id = 91,
                RegisterCheckIn = "09:00",
                RegisterCheckOut = "18:00",
                CheckIn = "09:13",
                CheckOut = "19:04"
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.UpdateTimekeeping(timekeeping);
                result.ShouldNotBeNull();
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var updatedTimekeeping = await _workScope.GetAsync<Timekeeping>(timekeeping.Id);

                updatedTimekeeping.Id.ShouldBe(timekeeping.Id);
                updatedTimekeeping.UserId.ShouldBe(19);
                updatedTimekeeping.RegisterCheckIn.ShouldBe("09:00");
                updatedTimekeeping.RegisterCheckOut.ShouldBe("18:00");
                updatedTimekeeping.CheckIn.ShouldBe("09:13");
                updatedTimekeeping.CheckOut.ShouldBe("19:04");
                updatedTimekeeping.DateAt.Date.ShouldBe(new DateTime(2022,12,26).Date);
            });
        }

        [Fact]
        public async Task UpdateTimekeeping_Should_Not_Update_Timekeeping_Has_RegisterCheckin_Or_RegisterCheckout_Is_null()
        {
            var timekeeping = new Timekeeping
            {
                Id = 100,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _timekeeping.UpdateTimekeeping(timekeeping);
                });
                Assert.Equal("RegisterCheckIn or RegisterCheckOut is null or empty", exception.Message);
            });
        }

        [Fact]
        public async Task UpdateTimekeeping_Should_Not_Update_Timekeeping_Id_Lower_Than_0()
        {
            var timekeeping = new Timekeeping
            {
                Id = -1,
                RegisterCheckIn = "09:00",
                RegisterCheckOut = "18:00"
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _timekeeping.UpdateTimekeeping(timekeeping);
                });
                Assert.Equal("Timekeeping id = 0 was not found!", exception.Message);
            });
        }

        [Fact]
        public async Task UpdateTimekeeping_Should_Not_Update_Timekeeping_Not_Exist()
        {
            var timekeeping = new Timekeeping
            {
                Id = 9999,
                RegisterCheckIn = "09:00",
                RegisterCheckOut = "18:00"
            };

            await WithUnitOfWorkAsync(async () =>
            {
                await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                {
                    await _timekeeping.UpdateTimekeeping(timekeeping);
                }); 
            });
        }

        [Fact]
        public async Task UpdateTimekeeping_Should_Not_Update_Timekeeping_Wrong_Checkin_Checkout_Time_Format()
        {
            var timekeeping = new Timekeeping
            {
                Id = 1,
                RegisterCheckIn = "09:00",
                RegisterCheckOut = "18:00",
                CheckIn = "a09:13",
                CheckOut = "a19:00"
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _timekeeping.UpdateTimekeeping(timekeeping);
                });
                Assert.Equal("Wrong time format (must be HH:mm)", exception.Message);
            });
        }

        [Fact]
        public async Task UserKhieuLai_Should_khieuLai()
        {
            var userNote = new TimekeepingUserNoteDto
            {
                Id = 1,
                UserNote = "WFH",
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var timekeeping = await _timekeeping.UserKhieuLai(userNote);
                timekeeping.ShouldNotBeNull();
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var updatedTimekeeping = await _workScope.GetAsync<Timekeeping>(userNote.Id);

                updatedTimekeeping.Id.ShouldBe(userNote.Id);
                updatedTimekeeping.UserNote.ShouldBe(userNote.UserNote);
            });

        }


        [Fact]
        public async Task UserKhieuLai_Should_Not_KhieuLai()
        {
            var userNote = new TimekeepingUserNoteDto
            {
                Id = 91,
                UserNote = "WFH",
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                  await _timekeeping.UserKhieuLai(userNote);
                });
                Assert.Equal("Bạn chỉ có thể khiếu lại cho bản ghi của mình", exception.Message);
            });

        }

        [Fact]
        public async Task TraLoiKhieuLai_Test()
        {
            var replyNote = new TimekeepingDto
            {
                Id = 1,
                NoteReply = "Khong duoc xac nhan",
                IsPunishedCheckIn = true,
            };

            await WithUnitOfWorkAsync(async () =>
            {
                var timekeeping = await _timekeeping.TraLoiKhieuLai(replyNote);
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var updatedTimekeeping = await _workScope.GetAsync<Timekeeping>(replyNote.Id);

                updatedTimekeeping.Id.ShouldBe(replyNote.Id);
                updatedTimekeeping.NoteReply.ShouldBe(replyNote.NoteReply);
            });
        }

        [Fact]
        public async Task AddTimekeepingByDay_Should_Add()
        {
            var date = "2022/12/1";
            var expectId = 298;
            var expectTotalCount = 327;

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.AddTimekeepingByDay(date);
                Assert.Equal(expectTotalCount, result.Count);
            });

            await WithUnitOfWorkAsync(async () =>
            {
                var addedTimekeeping = await _workScope.GetAsync<Timekeeping>(expectId);

                addedTimekeeping.Id.ShouldBe(expectId);
                addedTimekeeping.UserId.ShouldBe(1);
                addedTimekeeping.UserEmail.ShouldBe("admin@aspnetboilerplate.com");
                addedTimekeeping.RegisterCheckIn.ShouldBe("08:30");
                addedTimekeeping.RegisterCheckOut.ShouldBe("17:30");
                addedTimekeeping.DateAt.Date.ShouldBe(new DateTime(2022, 12, 1).Date);
                addedTimekeeping.IsPunishedCheckIn.ShouldBe(true);
                addedTimekeeping.IsPunishedCheckOut.ShouldBe(true);
                addedTimekeeping.IsLocked.ShouldBe(false);
                addedTimekeeping.IsDeleted.ShouldBe(false);
            });
        }

        [Fact]
        public async Task AddTimekeepingByDay_Should_Not_Add_Null_Date()
        {
            var date = "";

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _timekeeping.AddTimekeepingByDay(date);
                });
                Assert.Equal("Selected date is null!", exception.Message);
            });
        }

        [Fact]
        public async Task AddTimekeepingByDay_Should_Not_Add_Greater_Date_Than_Current()
        {
            var date = "3077/10/30";

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _timekeeping.AddTimekeepingByDay(date);
                });
                Assert.Equal("The selected date cannot greater than the current date!", exception.Message);
            });
        }

        [Fact]
        public async Task AddTimekeepingByDay_Should_Not_Add_Date_Off()
        {
            var date = "2022/12/4";

            await WithUnitOfWorkAsync(async () =>
            {
                var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                {
                    await _timekeeping.AddTimekeepingByDay(date);
                });
                Assert.Equal("12/04/2022 00:00:00 is Off Date => stop", exception.Message);
            });
        }

        //[Fact]
        //public async Task NoticePunishUserCheckInOut_Should_Notice()
        //{
        //    var date = DateTime.Parse("2022/12/27");

        //    await WithUnitOfWorkAsync(async () =>
        //    {
        //        var result = await _timekeeping.NoticePunishUserCheckInOut(date);
        //        Assert.NotNull(result);
        //    });
        //}

        [Fact]
        public async Task NoticePunishUserCheckInOut_Should_Not_Notice_Date_Off()
        {
            var date = DateTime.Parse("2022/12/25");

            await WithUnitOfWorkAsync(async () =>
            {
                var result = await _timekeeping.NoticePunishUserCheckInOut(date);
                Assert.Null(result);
            });
        }
    }
}
