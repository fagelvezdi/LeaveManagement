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
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using Blazored.Toast.Services;

namespace HR.LeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Create
    {
        [Inject]
        NavigationManager NavManager { get; set; }
        
        [Inject]
        ILeaveTypeService Client { get; set; }

        [Inject]
        IToastService ToastService { get; set; }
        
        public string Message { get; private set; }

        LeaveTypeVM leaveType = new();
        async Task CreateLeaveType()
        {
            var response = await Client.CreateLeaveType(leaveType);
            if (response.Success)
            {
                ToastService.ShowSuccess("Leave type created successfully!");
                NavManager.NavigateTo("/leavetypes/");
            }
            Message = response.Message;
        }
    }
}