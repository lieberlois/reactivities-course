using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<ActivityDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this._mapper = mapper;
                this._dataContext = context;
            }
            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                // Note: Manual Mapping is very time consuming, hence the usage of AutoMapper
                var activity = await _dataContext.Activities
                    // .Include(x => x.UserActivities)
                    // .ThenInclude(x => x.AppUser)
                    // .SingleOrDefaultAsync(x => x.Id == request.Id);
                    .FindAsync(request.Id);

                if (activity == null)
                    throw new RestException(HttpStatusCode.NotFound, new { activity = "Not found." });

                var mappedActivity = _mapper.Map<Activity, ActivityDto>(activity);

                return mappedActivity;
            }
        }
    }
}