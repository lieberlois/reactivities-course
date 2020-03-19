using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    public class Register
    {
        public class Command : IRequest<User>
        {
            public string DisplayName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly DataContext _dataContext;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(DataContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                this._jwtGenerator = jwtGenerator;
                this._userManager = userManager;
                this._dataContext = context;

            }

            public async Task<User> Handle(Command command, CancellationToken cancellationToken)
            {

                if (await _dataContext.Users.Where(x => x.Email == command.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new {Email = "Email already exists"});

                if (await _dataContext.Users.Where(x => x.UserName == command.Username).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new {Email = "Username already exists"});

                var user = new AppUser {
                    DisplayName = command.DisplayName,
                    Email = command.Email,
                    UserName = command.Username
                };

                var result = await _userManager.CreateAsync(user, command.Password);

                if (result.Succeeded)
                    return new User {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Image = null
                    };

                throw new Exception("Problem creating user.");
            }
        }
    }
}