using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using HR.LeaveManagement.BlazorUI;
using HR.LeaveManagement.BlazorUI.Pages.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Shared;
using HR.LeaveManagement.BlazorUI.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Blazored.Toast;
using Blazored.Toast.Services;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Models.LeaveRequests;
using HR.LeaveManagement.BlazorUI.Services;
using System.Reflection;

namespace HR.LeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Edit
    {
        [Inject]
        ILeaveTypeService LeaveTypeService { get; set; }

        [Inject]
        NavigationManager NavManager { get; set; }

        [Inject]
        IToastService ToastService { get; set; }

        public string Message { get; private set; }

        LeaveTypeVM LeaveType = new();

        [Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            LeaveType = await LeaveTypeService.GetLeaveTypeDetails(Id);
        }

        async Task UpdateLeaveType()
        {
            var response = await LeaveTypeService.UpdateLeaveType(Id, LeaveType);

            if(response.Success)
            {
                ToastService.ShowSuccess(response.Message);
                NavManager.NavigateTo("/leaveTypes/");
            }
        }
    }
}