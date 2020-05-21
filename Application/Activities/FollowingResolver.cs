using System.Linq;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class FollowingResolver : IValueResolver<UserActivity, AttendeeDto, bool>
    {
        private readonly DataContext _dataContext;
        private readonly IUserAccessor _userAccessor;
        public FollowingResolver(DataContext dataContext, IUserAccessor userAccessor)
        {
            this._userAccessor = userAccessor;
            this._dataContext = dataContext;
        }

        public bool Resolve(UserActivity source, AttendeeDto destination, bool destMember, ResolutionContext context)
        {
            var currentUser = _dataContext.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername()).Result;

            return currentUser.Followings.Any(x => x.TargetId == source.AppUserId);
        }
    }
}