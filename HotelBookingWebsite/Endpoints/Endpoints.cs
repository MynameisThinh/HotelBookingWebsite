using HotelBookingWebsite.Constants;
using HotelBookingWebsite.Data.Entities;
using HotelBookingWebsite.Services;

namespace HotelBookingWebsite.Endpoints
{
    public static class Endpoints
    {
        public static IEndpointRouteBuilder MapCustomEndpoints(this IEndpointRouteBuilder builder)
        {
            var staffAdmin = builder.MapGroup("staff-admin")
                                    .RequireAuthorization(authPolicyBuilder =>
                                    {
                                        authPolicyBuilder.RequireRole(RoleType.Admin.ToString(),RoleType.Staff.ToString());
                                    });
            staffAdmin.MapPost("/manage-sideoptions/delete/{SideOptionId:int}",
                async (int SideOptionId, ISideOptionService sideoptionService) =>
                {
                    await sideoptionService.DeleteSideOptionAsync(SideOptionId);
                    return TypedResults.LocalRedirect("~/staff-admin/manage-sideoptions");
                });
            return builder;
        }
    }
}
