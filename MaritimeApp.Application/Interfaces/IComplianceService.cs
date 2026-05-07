using MaritimeApp.Application.DTOs;

namespace MaritimeApp.Application.Interfaces;
public interface IComplianceService
{
    Task<ComplianceDashboardDto> GetDashboardAsync(Guid? shipId = null);
    Task<ShipComplianceDto> GetShipComplianceAsync(Guid shipId);
}