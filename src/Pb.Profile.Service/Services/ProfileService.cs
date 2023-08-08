using System.Collections;
using Grpc.Core;
using Pb.Profile.Service;
using Pb.Profile.Service.Models;

namespace Pb.Profile.Service.Services;

public class ProfileService : Profile.ProfileBase
{
    private readonly ILogger<ProfileService> _log;
    private readonly IDictionary<string, Hotel> _profiles;

    public ProfileService(ILogger<ProfileService> log, IHotelsLoader hotelLoader)
    {
        _log = log;
        _profiles = InitializeProfiles(hotelLoader.Hotels);
    }

    public override Task<ProfileResult> GetProfiles(ProfileRequest request, ServerCallContext context)
    {
        return Task.FromResult(new ProfileResult()
            {
                Hotels =
                {
                    _profiles
                        .Where(p => request.HotelIds.Contains(p.Key))
                        .Select(p => p.Value)
                }
            }
        );
    }

    private Dictionary<string, Hotel> InitializeProfiles(Hotel[] hotels)
    {
        var profiles = new Dictionary<string, Hotel>();

        foreach (var hotel in hotels)
        {
            profiles[hotel.Id] = hotel;
        }

        return profiles;
    }
}