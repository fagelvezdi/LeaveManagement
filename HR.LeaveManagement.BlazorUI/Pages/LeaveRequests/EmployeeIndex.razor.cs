using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HR.LeaveManagement.BlazorUI.Pages.LeaveRequests
{
    public partial class EmployeeIndex
    {
        [Inject] ILeaveRequestService LeaveRequestService {  get; set; }
        
        [Inject] IJSRuntime Js {  get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }

        public EmployeeLeaveRequestViewVM Model { get; set; } = new();

        public string Message { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Model = await LeaveRequestService.GetUserLeaveRequests();
        }

        async Task CancelRequestAsync(int id)
        {
            var confirm = await Js.InvokeAsync<bool>("confirm", "Do you want to cancel this request?");
            if (confirm)
            {
                var response = await LeaveRequestService.CancelLeaveRequest(id);
                if (response.Success)
                {
                    StateHasChanged();
                }
                else
                {
                    Message = response.Message;
                }
            }
        }
    }
}