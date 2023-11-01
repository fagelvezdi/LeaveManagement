using Blazored.Toast.Services;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using Microsoft.AspNetCore.Components;

namespace HR.LeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILeaveTypeService LeaveTypeService { get; set; }

        [Inject]
        public ILeaveAllocationService LeaveAllocationService { get; set; }

        [Inject]
        IToastService ToastService { get; set; }

        public List<LeaveTypeVM> LeaveTypes { get; private set; } = new List<LeaveTypeVM>();

        public string Message { get; set; } = string.Empty;

        protected void CreateLeaveType()
        {
            NavigationManager.NavigateTo("/leaveTypes/create/");
        }

        protected void AllocateLeaveType(int id)
        {
            //Use leave allocation Service here
            LeaveAllocationService.CreateLeaveAllocations(id);
        }

        protected void EditLeaveType(int id)
        {
            NavigationManager.NavigateTo($"/leaveTypes/edit/{id}");
        }

        protected async Task DeleteLeaveType(int id)
        {
            var response = await LeaveTypeService.DeleteLeaveType(id);
            if (response.Success)
            {
                ToastService.ShowSuccess("Leave Type Deleted Successfully!");
                await OnInitializedAsync();
            }
            else
            {
                Message = response.Message;
            }

        }

        protected void DetailsLeaveType(int id)
        {
            NavigationManager.NavigateTo($"/leaveTypes/details/{id}");
        }

        protected override async Task OnInitializedAsync()
        {
            LeaveTypes = await LeaveTypeService.GetLeaveTypes();
        }
    }
}