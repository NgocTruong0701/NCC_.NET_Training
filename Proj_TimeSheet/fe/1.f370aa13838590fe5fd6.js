(window.webpackJsonp=window.webpackJsonp||[]).push([[1],{as4P:function(e,t,n){"use strict";n.d(t,"a",function(){return a});var a={EnumProjectStatus:{Active:0,Deactive:1,All:2},EnumProjectType:{Timeandmaterials:0,Fixedfee:1,Nonbillable:2,ODC:3,Product:4,Training:5,NoSalary:6},EnumTaskType:{Commontask:0,Orthertask:1},EnumUserType:{Member:0,PM:1,Shadow:2,DeActive:3},EnumTypeOfWork:{All:-1,Normalworkinghours:0,Overtime:1},TimesheetStatus:{All:-1,Draft:0,Pending:1,Approve:2,Reject:3},EnumDayOfWeek:{Monday:0,Tuesday:1,Wednesday:2,Thursday:3,Friday:4,Saturday:5,Sunday:6},EnumDayOfWeekByGetDay:{Monday:1,Tuesday:2,Wednesday:3,Thursday:4,Friday:5,Saturday:6,Sunday:0},TimesheetViewBy:{Project:0,People:1},TypeViewHomePage:{Week:0,Month:1,Quater:2,Year:3,AllTime:4,CustomTime:5},MyTimesheetView:{Day:0,Week:1},MAX_WORKING_TIME:960,CHECK_STATUS:{CHECKED_NONE:0,CHECKED_SOME:1,CHECKED_ALL:2},BRANCH:{HN:0,DN:1,HCM:2,Vinh:3},LEVEL:{Intern_0:0,Intern_1:1,Intern_2:2,Intern_3:3,"Fresher-":4,Fresher:5,"Fresher+":6},TYPE:{Staff:0,Collaborator:2},HISTORYLEVEL:{Intern_0:0,Intern_1:1,Intern_2:2,Intern_3:3,"Fresher-":4,Fresher:5,"Fresher+":6},AbsenceStatus:{New:0,Pending:1,Approved:2,Rejected:3},ReviewStatus:{Draft:0,Reviewed:1,Approved:2,"Sent Email":3,Rejected:-1},AbsenceType:{FullDay:1,Morning:2,Afternoon:3,Custom:4},DayAbsenceType:{Off:0,Onsite:1,Remote:2},ListYear:[(new Date).getFullYear()-5,(new Date).getFullYear()-4,(new Date).getFullYear()-3,(new Date).getFullYear()-2,(new Date).getFullYear()-1,(new Date).getFullYear(),(new Date).getFullYear()+1],CHANGE_LEVEL:[{value:1,text:"Level up"},{value:2,text:"Level not change"}],EnumTypeWork:{Temp:!0,Offical:!1},AbsenceStatusFilter:{All:-1,"Pending or Approved":0,Pending:1,Approved:2,Rejected:3},MyTimesheetStatusFilter:{All:-1,New:0,"Pending or Approved":1,Pending:2,Approved:3,Rejected:4},FILTER_DEFAULT:{All:-1},EnumUserStatus:{Active:!0,InActive:!1},OnDayType:{BeginOfDay:1,EndOfDay:3},CellColor:{Normal:0,Begin:1,Staff:2,End:3,BeginHasRivew:4,EndHasRivew:5,BeginAndEnd:6,BeginAndStaff:7},TsStatusFilter:{Approved:1,"Pending and Approved":2},CheckInFilter:{All:-1,"No Check In":1,"No Check Out":2,"No Check In & No Check Out":3,"No Check In & No Check Out but have TS":4},HaveCheckInFilter:{All:-1,"Have Check In":1,"Have Check Out":2,"Have Check In & Have Check Out":3,"Have Check In or Have Check Out":4,"No Check In & No Check Out":5},PunishRules:[{name:"Kh\xf4ng ph\u1ea1t",value:0},{name:"\u0110i mu\u1ed9n",value:1},{name:"Kh\xf4ng CheckIn",value:2},{name:"Kh\xf4ng CheckOut",value:3},{name:"\u0110i mu\u1ed9n v\xe0 Kh\xf4ng CheckOut",value:4},{name:"Kh\xf4ng CheckIn v\xe0 kh\xf4ng CheckOut",value:5}]}},lxwe:function(e,t,n){"use strict";n.d(t,"a",function(){return i});var a=n("as4P"),i={EnumProjectStatus:[{value:a.a.EnumProjectStatus.Active,name:"Active"},{value:a.a.EnumProjectStatus.Deactive,name:"Deactive"},{value:a.a.EnumProjectStatus.All,name:"All"}],EnumProjectType:{Timeandmaterials:0,Fixedfee:1,Nonbillable:2},EnumUserType:[{value:a.a.EnumUserType.Member,name:"Member"},{value:a.a.EnumUserType.PM,name:"PM"},{value:a.a.EnumUserType.Shadow,name:"Shadow"},{value:a.a.EnumUserType.DeActive,name:"Deactive"}],EnumTaskType:[{value:a.a.EnumTaskType.Commontask,name:"Common Task"},{value:a.a.EnumTaskType.Orthertask,name:"Other Task"}],EnumTypeOfWork:[{value:a.a.EnumTypeOfWork.Normalworkinghours,name:"Normal working hours"},{value:a.a.EnumTypeOfWork.Overtime,name:"Overtime"}],TimesheetStatus:[{value:a.a.TimesheetStatus.All,name:"All"},{value:a.a.TimesheetStatus.Pending,name:"Pending"},{value:a.a.TimesheetStatus.Approve,name:"Approved"},{value:a.a.TimesheetStatus.Reject,name:"Rejected"}],EnumDayOfWeek:[{value:a.a.EnumDayOfWeek.Monday,name:"Monday"},{value:a.a.EnumDayOfWeek.Tuesday,name:"Tuesday"},{value:a.a.EnumDayOfWeek.Wednesday,name:"Wednesday"},{value:a.a.EnumDayOfWeek.Thursday,name:"Thursday"},{value:a.a.EnumDayOfWeek.Friday,name:"Friday"},{value:a.a.EnumDayOfWeek.Saturday,name:"Saturday"},{value:a.a.EnumDayOfWeek.Sunday,name:"Sunday"}],TimesheetViewBys:[{value:a.a.TimesheetViewBy.Project,name:"Project"},{value:a.a.TimesheetViewBy.People,name:"People"}],TimesheetSupervisiorViewBys:[{value:a.a.TimesheetViewBy.Project,name:"Project"},{value:a.a.TimesheetViewBy.People,name:"People"}],TypeViewHomePage:[{value:a.a.TypeViewHomePage.Week,name:"Week"},{value:a.a.TypeViewHomePage.Month,name:"Month"},{value:a.a.TypeViewHomePage.Quater,name:"Quarter"},{value:a.a.TypeViewHomePage.Year,name:"Year"},{value:a.a.TypeViewHomePage.AllTime,name:"All Time"},{value:a.a.TypeViewHomePage.CustomTime,name:"Custom Time"}],EnumTypeWork:[{value:a.a.EnumTypeWork.Temp,name:"Temp"},{value:a.a.EnumTypeWork.Offical,name:"Offical"}]}},ubVL:function(e,t,n){"use strict";n.d(t,"a",function(){return h}),n.d(t,"b",function(){return p});var a=n("YSh2"),i=n("K9Ia"),o=n("vubp"),s=n("ny24"),r=n("t9fZ"),l=n("CcnG"),u=n("Wf4p"),c=n("uGex"),h=function(){function e(e,t,n,a){void 0===a&&(a=null),this.matSelect=e,this.changeDetectorRef=t,this._viewportRuler=n,this.matOption=a,this.placeholderLabel="Suche",this.type="text",this.noEntriesFoundLabel="Keine Optionen gefunden",this.clearSearchInput=!0,this.searching=!1,this.disableInitialFocus=!1,this.preventHomeEndKeyPropagation=!1,this.disableScrollToActiveOnOptionsChanged=!1,this.ariaLabel="dropdown search",this.showToggleAllCheckbox=!1,this.toggleAllCheckboxChecked=!1,this.toggleAllCheckboxIndeterminate=!1,this.toggleAll=new l.EventEmitter,this.onChange=function(e){},this.onTouched=function(e){},this.overlayClassSet=!1,this.change=new l.EventEmitter,this._onDestroy=new i.a}return Object.defineProperty(e.prototype,"isInsideMatOption",{get:function(){return!!this.matOption},enumerable:!0,configurable:!0}),Object.defineProperty(e.prototype,"value",{get:function(){return this._value},enumerable:!0,configurable:!0}),e.prototype.ngOnInit=function(){var e=this,t="mat-select-search-panel";this.matSelect.panelClass?Array.isArray(this.matSelect.panelClass)?this.matSelect.panelClass.push(t):"string"==typeof this.matSelect.panelClass?this.matSelect.panelClass=[this.matSelect.panelClass,t]:"object"==typeof this.matSelect.panelClass&&(this.matSelect.panelClass[t]=!0):this.matSelect.panelClass=t,this.matOption&&(this.matOption.disabled=!0,this.matOption._getHostElement().classList.add("contains-mat-select-search")),this.matSelect.openedChange.pipe(Object(o.a)(1),Object(s.a)(this._onDestroy)).subscribe(function(t){t?(e.updateInputWidth(),e.disableInitialFocus||e._focus()):e.clearSearchInput&&e._reset()}),this.matSelect.openedChange.pipe(Object(r.a)(1)).pipe(Object(s.a)(this._onDestroy)).subscribe(function(){e.matSelect._keyManager?e.matSelect._keyManager.change.pipe(Object(s.a)(e._onDestroy)).subscribe(function(){return e.adjustScrollTopToFitActiveOptionIntoView()}):console.log("_keyManager was not initialized."),e._options=e.matSelect.options,e._options.changes.pipe(Object(s.a)(e._onDestroy)).subscribe(function(){var t=e.matSelect._keyManager;t&&e.matSelect.panelOpen&&setTimeout(function(){t.setFirstItemActive(),e.updateInputWidth(),e.matOption&&(e._noEntriesFound()?e.matOption._getHostElement().classList.add("mat-select-search-no-entries-found"):e.matOption._getHostElement().classList.remove("mat-select-search-no-entries-found")),e.disableScrollToActiveOnOptionsChanged||e.adjustScrollTopToFitActiveOptionIntoView()},1)})}),this.change.pipe(Object(s.a)(this._onDestroy)).subscribe(function(){e.changeDetectorRef.detectChanges()}),this._viewportRuler.change().pipe(Object(s.a)(this._onDestroy)).subscribe(function(){e.matSelect.panelOpen&&e.updateInputWidth()}),this.initMultipleHandling()},e.prototype._emitSelectAllBooleanToParent=function(e){this.toggleAll.emit(e)},e.prototype.ngOnDestroy=function(){this._onDestroy.next(),this._onDestroy.complete()},e.prototype.ngAfterViewInit=function(){var e=this;setTimeout(function(){e.setOverlayClass()}),this.matSelect.openedChange.pipe(Object(r.a)(1),Object(s.a)(this._onDestroy)).subscribe(function(){e.matSelect.options.changes.pipe(Object(s.a)(e._onDestroy)).subscribe(function(){e.changeDetectorRef.markForCheck()})})},e.prototype._isToggleAllCheckboxVisible=function(){return this.matSelect.multiple&&this.showToggleAllCheckbox},e.prototype._handleKeydown=function(e){(e.key&&1===e.key.length||e.keyCode>=a.a&&e.keyCode<=a.q||e.keyCode>=a.r&&e.keyCode<=a.j||e.keyCode===a.n||this.preventHomeEndKeyPropagation&&(e.keyCode===a.h||e.keyCode===a.e))&&e.stopPropagation()},e.prototype.writeValue=function(e){e!==this._value&&(this._value=e,this.change.emit(e))},e.prototype.onInputChange=function(e){e!==this._value&&(this.initMultiSelectedValues(),this._value=e,this.onChange(e),this.change.emit(e))},e.prototype.onBlur=function(e){this.writeValue(e),this.onTouched()},e.prototype.registerOnChange=function(e){this.onChange=e},e.prototype.registerOnTouched=function(e){this.onTouched=e},e.prototype._focus=function(){if(this.searchSelectInput&&this.matSelect.panel){var e=this.matSelect.panel.nativeElement,t=e.scrollTop;this.searchSelectInput.nativeElement.focus(),e.scrollTop=t}},e.prototype._reset=function(e){this.searchSelectInput&&(this.searchSelectInput.nativeElement.value="",this.onInputChange(""),this.matOption&&!e&&this.matOption._getHostElement().classList.remove("mat-select-search-no-entries-found"),e&&this._focus())},e.prototype.setOverlayClass=function(){var e=this;if(!this.overlayClassSet){var t=["cdk-overlay-pane-select-search"];this.matOption||t.push("cdk-overlay-pane-select-search-with-offset"),this.matSelect.overlayDir.attach.pipe(Object(s.a)(this._onDestroy)).subscribe(function(){for(var n,a=e.searchSelectInput.nativeElement;a=a.parentElement;)if(a.classList.contains("cdk-overlay-pane")){n=a;break}n&&t.forEach(function(e){n.classList.add(e)})}),this.overlayClassSet=!0}},e.prototype.initMultipleHandling=function(){var e=this;this.matSelect.valueChange.pipe(Object(s.a)(this._onDestroy)).subscribe(function(t){if(e.matSelect.multiple){var n=!1;if(e._value&&e._value.length&&e.previousSelectedValues&&Array.isArray(e.previousSelectedValues)){t&&Array.isArray(t)||(t=[]);var a=e.matSelect.options.map(function(e){return e.value});e.previousSelectedValues.forEach(function(e){-1===t.indexOf(e)&&-1===a.indexOf(e)&&(t.push(e),n=!0)})}n&&e.matSelect._onChange(t),e.previousSelectedValues=t}})},e.prototype.adjustScrollTopToFitActiveOptionIntoView=function(){if(this.matSelect.panel&&this.matSelect.options.length>0){var e=this.getMatOptionHeight(),t=this.matSelect._keyManager.activeItemIndex||0,n=Object(u.B)(t,this.matSelect.options,this.matSelect.optionGroups),a=(this.matOption?-1:0)+n+t,i=this.matSelect.panel.nativeElement.scrollTop,o=this.innerSelectSearch.nativeElement.offsetHeight,s=Math.floor((c.f-o)/e),r=Math.round((i+o)/e)-1;r>=a?this.matSelect.panel.nativeElement.scrollTop=a*e:r+s<=a&&(this.matSelect.panel.nativeElement.scrollTop=(a+1)*e-(c.f-o))}},e.prototype.updateInputWidth=function(){if(this.innerSelectSearch&&this.innerSelectSearch.nativeElement){for(var e,t=this.innerSelectSearch.nativeElement;t=t.parentElement;)if(t.classList.contains("mat-select-panel")){e=t;break}e&&(this.innerSelectSearch.nativeElement.style.width=e.clientWidth+"px")}},e.prototype.getMatOptionHeight=function(){return this.matSelect.options.length>0?this.matSelect.options.first._getHostElement().getBoundingClientRect().height:0},e.prototype.initMultiSelectedValues=function(){this.matSelect.multiple&&!this._value&&(this.previousSelectedValues=this.matSelect.options.filter(function(e){return e.selected}).map(function(e){return e.value}))},e.prototype._noEntriesFound=function(){if(this._options)return this.matOption?this.noEntriesFoundLabel&&this.value&&1===this._options.length:this.noEntriesFoundLabel&&this.value&&0===this._options.length},e}(),p=function(){return function(){}}()},wzdi:function(e,t,n){"use strict";n.d(t,"a",function(){return y});var a=n("CcnG"),i=n("nS6G"),o=n("tnCA"),s=n("UtOw"),r=n("dxxL"),l=n("Yap4"),u=n("WTR0"),c=n("wlJd"),h=n("wtux"),p=n("tvsI"),m=n("as4P"),v=n("lxwe"),d=n("DBBP"),y=function(){function e(e){this.subscriptions=[],this.APP_CONSTANT=m.a,this.APP_CONFIG=v.a,this.UserType={Staff:0,Intern:1,Collaborator:2},this.Level={Intern_0:0,Intern_1:1,Intern_2:2,Intern_3:3,FresherMinus:4,Fresher:5,FresherPlus:6,JuniorMinus:7,Junior:8,JuniorPlus:9,MiddleMinus:10,Middle:11,MiddlePlus:12,SeniorMinus:13,Senior:14,SeniorPlus:15},this.userLevels=[{value:this.Level.Intern_0,name:"Intern_0",style:{"background-color":"#B2BEB5"}},{value:this.Level.Intern_1,name:"Intern_1",style:{"background-color":"#8F9779"}},{value:this.Level.Intern_2,name:"Intern_2",style:{"background-color":"#665D1E"}},{value:this.Level.Intern_3,name:"Intern_3",style:{"background-color":"#777"}},{value:this.Level.FresherMinus,name:"Fresher-",style:{"background-color":"#2196f3"}},{value:this.Level.Fresher,name:"Fresher",style:{"background-color":"#89CFF0"}},{value:this.Level.FresherPlus,name:"Fresher+",style:{"background-color":"#318CE7"}},{value:this.Level.JuniorMinus,name:"Junior-",style:{"background-color":"#BFAFB2"}},{value:this.Level.Junior,name:"Junior",style:{"background-color":"#A57164"}},{value:this.Level.JuniorPlus,name:"Junior+",style:{"background-color":"#3B2F2F"}},{value:this.Level.MiddleMinus,name:"Middle-",style:{"background-color":"#A4C639"}},{value:this.Level.Middle,name:"Middle",style:{"background-color":"#8DB600"}},{value:this.Level.MiddlePlus,name:"Middle+",style:{"background-color":"#008000"}},{value:this.Level.SeniorMinus,name:"Senior-",style:{"background-color":"#F19CBB"}},{value:this.Level.Senior,name:"Senior",style:{"background-color":"#AB274F"}},{value:this.Level.SeniorPlus,name:"Senior+",style:{"background-color":"#E52B50"}}],this.localizationSourceName=i.a.localization.defaultLocalizationSourceName,this.localization=e.get(o.a),this.permission=e.get(s.a),this.feature=e.get(r.a),this.notify=e.get(l.a),this.setting=e.get(u.a),this.message=e.get(c.a),this.multiTenancy=e.get(h.a),this.appSession=e.get(p.a),this.elementRef=e.get(a.ElementRef)}return e.prototype.l=function(e){for(var t=[],n=1;n<arguments.length;n++)t[n-1]=arguments[n];var a=this.localization.localize(e,this.localizationSourceName);return a||(a=e),t&&t.length?(t.unshift(a),abp.utils.formatString.apply(this,t)):a},e.prototype.getVersionReleaseDate=function(){return this.appSession.application.version+this.appSession.application.releaseDate},e.prototype.isGranted=function(e){return this.permission.isGranted(e)},e.prototype.convertMinuteToHour=function(e){return Object(d.e)(e)},e.prototype.convertHourtoMinute=function(e){return Object(d.c)(e)},e.prototype.convertFloatHourToMinute=function(e){return Object(d.b)(e)},e.prototype.convertMinuteToFloat=function(e){return Object(d.d)(e)},e.prototype.getAvatar=function(e){return e.avatarFullPath?e.avatarFullPath:1==e.sex?"assets/images/women.png":"assets/images/men.png"},e.prototype.convertEnumToDropdown=function(e){return Object.keys(e).filter(function(e){return isNaN(Number(e))}).map(function(t){return{key:t,value:e[t]}})},e.prototype.getStarColorforReviewInternCapability=function(e,t){return e<2.5?"grey":e<3.5?"yellow":e<4.5?"orange":""},e}()}}]);